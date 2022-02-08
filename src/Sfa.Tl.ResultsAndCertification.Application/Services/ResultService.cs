using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultService : IResultService
    {
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IRepository<TqPathwayResult> _pathwayResultRepository;
        private readonly IRepository<TqSpecialismResult> _specialismResultRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ResultService(IRepository<AssessmentSeries> assessmentSeriesRepository,
            IRepository<TlLookup> tlLookupRepository,
            IResultRepository resultRepository,
            IRepository<TqPathwayResult> pathwayResultRepository,
            IRepository<TqSpecialismResult> specialismResultRepository,
            IMapper mapper,
            ILogger<ResultService> logger)
        {
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _tlLookupRepository = tlLookupRepository;
            _resultRepository = resultRepository;
            _pathwayResultRepository = pathwayResultRepository;
            _specialismResultRepository = specialismResultRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<ResultRecordResponse>> ValidateResultsAsync(long aoUkprn, IEnumerable<ResultCsvRecordResponse> csvResults)
        {
            var response = new List<ResultRecordResponse>();
            var dbRegistrations = await _resultRepository.GetBulkResultsAsync(aoUkprn, csvResults.Select(x => x.Uln));
            var dbAssessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var tlLookup = await _tlLookupRepository.GetManyAsync().ToListAsync();

            foreach (var result in csvResults)
            {
                // 1. ULN not recognised with AO
                var dbRegistration = dbRegistrations.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == result.Uln);
                if (dbRegistration == null)
                {
                    response.Add(AddStage3ValidationError(result.RowNum, result.Uln, ValidationMessages.UlnNotRegistered));
                    continue;
                }

                // 2. ULN is withdrawn
                var isWithdrawn = dbRegistration.Status == RegistrationPathwayStatus.Withdrawn;
                if (isWithdrawn)
                {
                    response.Add(AddStage3ValidationError(result.RowNum, result.Uln, ValidationMessages.CannotAddResultToWithdrawnRegistration));
                    continue;
                }

                var validationErrors = new List<BulkProcessValidationError>();
                var isCoreAndAssesssmentSeriesValid = true;

                // 3. Core Code is incorrect
                if (!string.IsNullOrWhiteSpace(result.CoreCode))
                {
                    var isValidCoreCode = dbRegistration.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(result.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidCoreCode)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreComponentCode));
                        isCoreAndAssesssmentSeriesValid = false;
                    }
                }

                // 4. Core Assessment Series does not exists
                if (!string.IsNullOrWhiteSpace(result.CoreAssessmentSeries))
                {
                    var isSeriesFound = dbAssessmentSeries.Any(x => x.ComponentType == ComponentType.Core && x.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (!isSeriesFound)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreAssessmentSeriesEntry));
                        isCoreAndAssesssmentSeriesValid = false;
                    }
                }

                // 5. Core Grade not valid - needs to be A* to E, or Unclassified
                var pathwayComponentGrades = tlLookup.Where(lr => lr.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (!string.IsNullOrWhiteSpace(result.CoreGrade))
                {
                    var isValidCoreComponentGrade = pathwayComponentGrades.Any(pcg => pcg.Value.Equals(result.CoreGrade, StringComparison.InvariantCultureIgnoreCase));
                    if (!isValidCoreComponentGrade)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreComponentGrade));
                }

                // 6. Core No assessment entry is currently active
                var hasActiveCoreAssessmentEntry = true;
                if (!string.IsNullOrWhiteSpace(result.CoreCode))
                {
                    var hasAnyActiveCoreAssessment = dbRegistration.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.EndDate == null);
                    if (!hasAnyActiveCoreAssessment)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.NoCoreAssessmentEntryCurrentlyActive));
                        hasActiveCoreAssessmentEntry = false;
                    }
                }

                // 7. Core Series not matched with reg. 
                if (isCoreAndAssesssmentSeriesValid && hasActiveCoreAssessmentEntry && !string.IsNullOrWhiteSpace(result.CoreAssessmentSeries))
                {
                    var hasAssessmentSeriesMatchTheSeriesOnRegistrationCore = dbRegistration.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.EndDate == null && pa.AssessmentSeries.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (!hasAssessmentSeriesMatchTheSeriesOnRegistrationCore)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration));
                }

                // 8. Specialism - Code is not recognised. 
                var isSpecialismAndAssesssmentSeriesValid = true;
                if (result.SpecialismCodes != null && result.SpecialismCodes.Any())
                {
                    var registeredSpecialismsLarIds = dbRegistration.TqRegistrationSpecialisms.Select(x => x.TlSpecialism.LarId);
                    var hasInvalidSpecialismCode = result.SpecialismCodes.Except(registeredSpecialismsLarIds, StringComparer.InvariantCultureIgnoreCase).Any();
                    if (hasInvalidSpecialismCode)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.SpecialismCodeNotRecognised));
                        isSpecialismAndAssesssmentSeriesValid = false;
                    }
                }

                // 9. Specialism - Assessment Series does not exists
                if (!string.IsNullOrWhiteSpace(result.SpecialismAssessmentSeries))
                {
                    var isSeriesFound = dbAssessmentSeries.Any(x => x.ComponentType == ComponentType.Specialism && x.Name.Equals(result.SpecialismAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (!isSeriesFound)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidSpecialismAssessmentSeriesEntry));
                        isSpecialismAndAssesssmentSeriesValid = false;
                    }
                }

                // 10. Specialism - No assessment entry is currently active
                var hasActivSpecialismAssessmentEntry = true;
                if (result.SpecialismCodes != null && result.SpecialismCodes.Any())
                {
                    var hasAnyActivSpecialismAssessmentEntry = dbRegistration.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Any(pa => pa.IsOptedin && pa.EndDate == null);
                    if (!hasAnyActivSpecialismAssessmentEntry)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.NoSpecialismAssessmentEntryCurrentlyActive));
                        hasActivSpecialismAssessmentEntry = false;
                    }
                }

                // 11. Specialism - Assessment entry mapping error 
                if (isSpecialismAndAssesssmentSeriesValid && hasActivSpecialismAssessmentEntry && !string.IsNullOrWhiteSpace(result.SpecialismAssessmentSeries))
                {
                    var hasAssessmentSeriesMatchTheSeriesOnRegistrationSpecialism = dbRegistration.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments)
                                                                                    .Any(sa => sa.IsOptedin && sa.EndDate == null && sa.AssessmentSeries.Name.Equals(result.SpecialismAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));

                    if (!hasAssessmentSeriesMatchTheSeriesOnRegistrationSpecialism)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.SpecialismSeriesDoesNotMatchTheSeriesOnTheRegistration));
                }

                // 12. Specialim - Assessment series is not open 
                if (isSpecialismAndAssesssmentSeriesValid && hasActivSpecialismAssessmentEntry && !string.IsNullOrWhiteSpace(result.SpecialismAssessmentSeries))
                {
                    var isValidNextAssessmentSeries = IsValidNextAssessmentSeries(dbAssessmentSeries.Where(x => x.ComponentType == ComponentType.Specialism).ToList(), dbRegistration.AcademicYear, result.SpecialismAssessmentSeries, Constants.SpecialismAssessmentStartInYears, ComponentType.Specialism);
                    if (!isValidNextAssessmentSeries)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.SpecialismSeriesNotCurrentlyOpen));
                }

                // 13. Specialism - Grade not valid
                var specialismLookupGrades = tlLookup.Where(lr => lr.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (result.SpecialismGrades != null && result.SpecialismGrades.Any())
                {
                    var hasInvalidSpecialismComponentGrade = result.SpecialismGrades.Except(specialismLookupGrades.Select(g => g.Value), StringComparer.InvariantCultureIgnoreCase).Any();
                    if (hasInvalidSpecialismComponentGrade)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.SpecialismGradeIsNotValid));
                }

                if (validationErrors.Any())
                    response.Add(new ResultRecordResponse { ValidationErrors = validationErrors });
                else
                {
                    var pathwayAssessment = dbRegistration.TqPathwayAssessments.FirstOrDefault(pa => pa.IsOptedin && pa.EndDate == null && pa.AssessmentSeries.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    var pathwayComponentGrade = pathwayComponentGrades.FirstOrDefault(pcg => pcg.Value.Equals(result.CoreGrade, StringComparison.InvariantCultureIgnoreCase));

                    var specialismResults = new Dictionary<int, int?>();

                    if (result.SpecialismCodes != null)
                    {
                        foreach (var specialismCode in result.SpecialismCodes?.Select((code, idx) => (code, idx)))
                        {
                            var specialismAssessment = dbRegistration.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments)
                                                                                            .FirstOrDefault(x => x.IsOptedin && x.EndDate == null
                                                                                                && x.TqRegistrationSpecialism.TlSpecialism.LarId.Equals(specialismCode.code, StringComparison.InvariantCultureIgnoreCase)
                                                                                                && x.AssessmentSeries.Name.Equals(result.SpecialismAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));

                            var specialismGrade = specialismLookupGrades.FirstOrDefault(scg => scg.Value.Equals(result.SpecialismGrades[specialismCode.idx], StringComparison.InvariantCultureIgnoreCase));
                            specialismResults.Add(specialismAssessment.Id, specialismGrade?.Id);
                        }
                    }

                    response.Add(new ResultRecordResponse
                    {
                        TqPathwayAssessmentId = !string.IsNullOrWhiteSpace(result.CoreCode) ? pathwayAssessment?.Id : null,
                        PathwayComponentGradeLookupId = !string.IsNullOrWhiteSpace(result.CoreGrade) ? pathwayComponentGrade?.Id : null,
                        SpecialismResults = specialismResults.Any() ? specialismResults : null
                    });
                }
            }
            return response;
        }

        public (IList<TqPathwayResult>, IList<TqSpecialismResult>) TransformResultsModel(IList<ResultRecordResponse> resultsData, string performedBy)
        {
            var pathwayResults = new List<TqPathwayResult>();
            var specialismResults = new List<TqSpecialismResult>();


            foreach (var (result, index) in resultsData.Select((value, i) => (value, i)))
            {
                if (result.TqPathwayAssessmentId.HasValue && result.TqPathwayAssessmentId.Value > 0)
                {
                    pathwayResults.Add(new TqPathwayResult
                    {
                        Id = index - Constants.PathwayResultsStartIndex,
                        TqPathwayAssessmentId = result.TqPathwayAssessmentId.Value,
                        TlLookupId = result.PathwayComponentGradeLookupId ?? 0,
                        StartDate = DateTime.UtcNow,
                        IsOptedin = true,
                        IsBulkUpload = true,
                        CreatedBy = performedBy,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                if (result.SpecialismResults != null)
                {
                    foreach (var specialismResult in result.SpecialismResults)
                    {
                        specialismResults.Add(new TqSpecialismResult
                        {
                            Id = index - Constants.SpecialismResultsStartIndex,
                            TqSpecialismAssessmentId = specialismResult.Key,
                            TlLookupId = specialismResult.Value ?? 0,
                            StartDate = DateTime.UtcNow,
                            IsOptedin = true,
                            IsBulkUpload = true,
                            CreatedBy = performedBy,
                            CreatedOn = DateTime.UtcNow
                        });
                    }
                }
            }

            return (pathwayResults, specialismResults);
        }

        public async Task<ResultProcessResponse> CompareAndProcessResultsAsync(IList<TqPathwayResult> pathwayResultsToProcess, IList<TqSpecialismResult> specialismResultsToProcess)
        {
            var response = new ResultProcessResponse();

            // Prepare Pathway Results
            var newOrAmendedPathwayResultRecords = await PrepareNewAndAmendedPathwayResults(pathwayResultsToProcess, response);

            // Prepare Specialism Results
            var newOrAmendedSpecialismResultRecords = await PrepareNewAndAmendedSpecialismResults(specialismResultsToProcess, response);

            if (response.IsValid)
                response.IsSuccess = await _resultRepository.BulkInsertOrUpdateResults(newOrAmendedPathwayResultRecords, newOrAmendedSpecialismResultRecords);

            return response;
        }

        private async Task<List<TqPathwayResult>> PrepareNewAndAmendedPathwayResults(IList<TqPathwayResult> pathwayResultsToProcess, ResultProcessResponse response)
        {
            var pathwayResultComparer = new TqPathwayResultEqualityComparer();
            var amendedPathwayResults = new List<TqPathwayResult>();
            var newAndAmendedPathwayResultRecords = new List<TqPathwayResult>();

            var existingPathwayResultsFromDb = await _resultRepository.GetBulkPathwayResultsAsync(pathwayResultsToProcess);
            var newPathwayResults = pathwayResultsToProcess.Except(existingPathwayResultsFromDb, pathwayResultComparer).ToList();
            var matchedPathwayResults = pathwayResultsToProcess.Intersect(existingPathwayResultsFromDb, pathwayResultComparer).ToList();
            var unchangedPathwayResults = matchedPathwayResults.Intersect(existingPathwayResultsFromDb, new TqPathwayResultRecordEqualityComparer()).ToList();
            var hasAnyMatchedPathwayResultsToProcess = matchedPathwayResults.Count != unchangedPathwayResults.Count;

            if (hasAnyMatchedPathwayResultsToProcess)
            {
                amendedPathwayResults = matchedPathwayResults.Except(unchangedPathwayResults, pathwayResultComparer).ToList();
                amendedPathwayResults.ForEach(amendedPathwayResult =>
                {
                    var existingPathwayResult = existingPathwayResultsFromDb.FirstOrDefault(existingPathwayResult => existingPathwayResult.TqPathwayAssessmentId == amendedPathwayResult.TqPathwayAssessmentId);
                    if (existingPathwayResult != null)
                    {
                        var isAppealDatePassed = DateTime.Today > existingPathwayResult.TqPathwayAssessment.AssessmentSeries.AppealEndDate.Date;
                        if (isAppealDatePassed || existingPathwayResult.PrsStatus == PrsStatus.Final)
                        {
                            response.ValidationErrors.Add(GetResultValidationError(existingPathwayResult.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.ResultIsInFinal));
                            return;
                        }

                        // Validation: Result should not be in BeingAppealed Status.
                        if (existingPathwayResult.PrsStatus == PrsStatus.BeingAppealed)
                        {
                            response.ValidationErrors.Add(GetResultValidationError(existingPathwayResult.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.ResultCannotBeInBeingAppealedStatus));
                            return;
                        }

                        var hasPathwayResultChanged = amendedPathwayResult.TlLookupId != existingPathwayResult.TlLookupId;

                        if (hasPathwayResultChanged)
                        {
                            existingPathwayResult.IsOptedin = false;
                            existingPathwayResult.EndDate = DateTime.UtcNow;
                            existingPathwayResult.ModifiedBy = amendedPathwayResult.CreatedBy;
                            existingPathwayResult.ModifiedOn = DateTime.UtcNow;

                            newAndAmendedPathwayResultRecords.Add(existingPathwayResult);

                            if (amendedPathwayResult.TqPathwayAssessmentId > 0 && amendedPathwayResult.TlLookupId > 0)
                                newAndAmendedPathwayResultRecords.Add(amendedPathwayResult);
                        }
                    }
                });
            }

            if (response.IsValid && newPathwayResults.Any())
                newAndAmendedPathwayResultRecords.AddRange(newPathwayResults.Where(p => p.TqPathwayAssessmentId > 0 && p.TlLookupId > 0));

            return newAndAmendedPathwayResultRecords;
        }

        private async Task<List<TqSpecialismResult>> PrepareNewAndAmendedSpecialismResults(IList<TqSpecialismResult> specialismResultsToProcess, ResultProcessResponse response)
        {
            var specialismResultComparer = new TqSpecialismResultEqualityComparer();
            var amendedSpecialismResults = new List<TqSpecialismResult>();
            var newAndAmendedSpecialismResultRecords = new List<TqSpecialismResult>();

            var existingSpecialismResultsFromDb = await _resultRepository.GetBulkSpecialismResultsAsync(specialismResultsToProcess);
            var newSpecialismResults = specialismResultsToProcess.Except(existingSpecialismResultsFromDb, specialismResultComparer).ToList();
            var matchedSpecialismResults = specialismResultsToProcess.Intersect(existingSpecialismResultsFromDb, specialismResultComparer).ToList();
            var unchangedSpecialismResults = matchedSpecialismResults.Intersect(existingSpecialismResultsFromDb, new TqSpecialismResultRecordEqualityComparer()).ToList();
            var hasAnyMatchedSpecialismResultsToProcess = matchedSpecialismResults.Count != unchangedSpecialismResults.Count;

            if (hasAnyMatchedSpecialismResultsToProcess)
            {
                amendedSpecialismResults = matchedSpecialismResults.Except(unchangedSpecialismResults, specialismResultComparer).ToList();
                amendedSpecialismResults.ForEach(amendedSpecialismResult =>
                {
                    var existingSpecialismResult = existingSpecialismResultsFromDb.FirstOrDefault(existingSpecialismResult => existingSpecialismResult.TqSpecialismAssessmentId == amendedSpecialismResult.TqSpecialismAssessmentId);
                    if (existingSpecialismResult != null)
                    {
                        var isAppealDatePassed = DateTime.Today > existingSpecialismResult.TqSpecialismAssessment.AssessmentSeries.AppealEndDate.Date;
                        if (isAppealDatePassed || existingSpecialismResult.PrsStatus == PrsStatus.Final)
                        {
                            response.ValidationErrors.Add(GetResultValidationError(existingSpecialismResult.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.ResultIsInFinal));
                            return;
                        }

                        // Validation: Result should not be in BeingAppealed Status.
                        if (existingSpecialismResult.PrsStatus == PrsStatus.BeingAppealed)
                        {
                            response.ValidationErrors.Add(GetResultValidationError(existingSpecialismResult.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.ResultCannotBeInBeingAppealedStatus));
                            return;
                        }

                        var hasSpecialismResultChanged = amendedSpecialismResult.TlLookupId != existingSpecialismResult.TlLookupId;

                        if (hasSpecialismResultChanged)
                        {
                            existingSpecialismResult.IsOptedin = false;
                            existingSpecialismResult.EndDate = DateTime.UtcNow;
                            existingSpecialismResult.ModifiedBy = amendedSpecialismResult.CreatedBy;
                            existingSpecialismResult.ModifiedOn = DateTime.UtcNow;

                            newAndAmendedSpecialismResultRecords.Add(existingSpecialismResult);

                            if (amendedSpecialismResult.TqSpecialismAssessmentId > 0 && amendedSpecialismResult.TlLookupId > 0)
                                newAndAmendedSpecialismResultRecords.Add(amendedSpecialismResult);
                        }
                    }
                });
            }

            if (response.IsValid && newSpecialismResults.Any())
                newAndAmendedSpecialismResultRecords.AddRange(newSpecialismResults.Where(p => p.TqSpecialismAssessmentId > 0 && p.TlLookupId > 0));

            return newAndAmendedSpecialismResultRecords;
        }

        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistration = await _resultRepository.GetResultsAsync(aoUkprn, profileId);

            if (tqRegistration == null || (status != null && tqRegistration.Status != status)) return null;

            return _mapper.Map<ResultDetails>(tqRegistration);
        }

        public async Task<AddResultResponse> AddResultAsync(AddResultRequest request)
        {
            // Validate
            var tqRegistrationPathway = await _resultRepository.GetResultsAsync(request.AoUkprn, request.ProfileId);
            var isValid = IsValidAddResultRequestAsync(tqRegistrationPathway, request);
            if (!isValid)
                return new AddResultResponse { IsSuccess = false };

            int status = 0;
            if (request.ComponentType == ComponentType.Core)
            {
                status = await _pathwayResultRepository.CreateAsync(new TqPathwayResult
                {
                    TqPathwayAssessmentId = request.AssessmentId,
                    TlLookupId = request.LookupId,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }
            else if (request.ComponentType == ComponentType.Specialism)
            {
                status = await _specialismResultRepository.CreateAsync(new TqSpecialismResult
                {
                    TqSpecialismAssessmentId = request.AssessmentId,
                    TlLookupId = request.LookupId,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }
            return new AddResultResponse { Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ProfileId = request.ProfileId, IsSuccess = status > 0 };
        }

        public async Task<ChangeResultResponse> ChangeResultAsync(ChangeResultRequest request)
        {
            var isSuccess = request.ComponentType switch
            {
                ComponentType.Core => await ProcessChangeCoreResultAsync(request),
                ComponentType.Specialism => await ProcessChangeSpecialismResultAsync(request),
                _ => false,
            };
            return new ChangeResultResponse { Uln = request.Uln, ProfileId = request.ProfileId, IsSuccess = isSuccess };
        }

        private async Task<bool> ProcessChangeCoreResultAsync(ChangeResultRequest request)
        {
            var existingPathwayResult = await _pathwayResultRepository.GetFirstOrDefaultAsync(pr => pr.Id == request.ResultId
                                                                                              && pr.EndDate == null && pr.IsOptedin
                                                                                              && pr.TqPathwayAssessment.EndDate == null && pr.IsOptedin
                                                                                              && pr.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                                              && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                              && pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn);

            if (existingPathwayResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Pathway Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: ProcessCoreResultAsync({request})");
                return false;
            }

            var pathwayResultsToUpdate = new List<TqPathwayResult>();

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = DateTime.UtcNow;
            existingPathwayResult.ModifiedBy = request.PerformedBy;
            existingPathwayResult.ModifiedOn = DateTime.UtcNow;

            pathwayResultsToUpdate.Add(existingPathwayResult);

            if (request.LookupId.HasValue && request.LookupId > 0)
            {
                pathwayResultsToUpdate.Add(new TqPathwayResult
                {
                    TqPathwayAssessmentId = existingPathwayResult.TqPathwayAssessmentId,
                    TlLookupId = request.LookupId.Value,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }

            return await _pathwayResultRepository.UpdateManyAsync(pathwayResultsToUpdate) > 0;
        }

        private async Task<bool> ProcessChangeSpecialismResultAsync(ChangeResultRequest request)
        {
            var existingSpecialismResult = await _specialismResultRepository.GetFirstOrDefaultAsync(pr => pr.Id == request.ResultId
                                                                                                    && pr.EndDate == null && pr.IsOptedin
                                                                                                    && pr.TqSpecialismAssessment.EndDate == null && pr.IsOptedin
                                                                                                    && pr.TqSpecialismAssessment.TqRegistrationSpecialism.EndDate == null && pr.TqSpecialismAssessment.TqRegistrationSpecialism.IsOptedin
                                                                                                    && pr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                                                    && pr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                                    && pr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn);

            if (existingSpecialismResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Specialism Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: ProcessSpecialismResultAsync({request})");
                return false;
            }

            var specialismResultsToUpdate = new List<TqSpecialismResult>();

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = DateTime.UtcNow;
            existingSpecialismResult.ModifiedBy = request.PerformedBy;
            existingSpecialismResult.ModifiedOn = DateTime.UtcNow;

            specialismResultsToUpdate.Add(existingSpecialismResult);

            if (request.LookupId.HasValue && request.LookupId > 0)
            {
                specialismResultsToUpdate.Add(new TqSpecialismResult
                {
                    TqSpecialismAssessmentId = existingSpecialismResult.TqSpecialismAssessmentId,
                    TlLookupId = request.LookupId.Value,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }

            return await _specialismResultRepository.UpdateManyAsync(specialismResultsToUpdate) > 0;
        }


        #region Private Methods

        private ResultRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new ResultRecordResponse()
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new BulkProcessValidationError
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private BulkProcessValidationError GetResultValidationError(long uln, string errorMessage)
        {
            return new BulkProcessValidationError
            {
                Uln = uln.ToString(),
                ErrorMessage = errorMessage
            };
        }

        private BulkProcessValidationError BuildValidationError(ResultCsvRecordResponse result, string message)
        {
            return new BulkProcessValidationError { RowNum = result.RowNum.ToString(), Uln = result.Uln.ToString(), ErrorMessage = message };
        }

        private bool IsValidAddResultRequestAsync(TqRegistrationPathway registrationPathway, AddResultRequest addResultRequest)
        {
            // 1. Must be an active registration.
            if (registrationPathway == null || registrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;

            if (addResultRequest.ComponentType == ComponentType.Core)
            {
                var assessmentEntry = registrationPathway.TqPathwayAssessments.FirstOrDefault(p => p.Id == addResultRequest.AssessmentId && p.IsOptedin && p.EndDate == null);

                if (assessmentEntry == null) return false;

                var anyActiveResult = assessmentEntry.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null);
                return !anyActiveResult;
            }
            else if (addResultRequest.ComponentType == ComponentType.Specialism)
            {
                var specialism = registrationPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin &&
                                                                                              s.EndDate == null &&
                                                                                              s.TqSpecialismAssessments.Any(sa => sa.Id == addResultRequest.AssessmentId));

                var assessmentEntry = specialism?.TqSpecialismAssessments?.FirstOrDefault(p => p.Id == addResultRequest.AssessmentId && p.IsOptedin && p.EndDate == null);

                if (assessmentEntry == null) return false;

                var anyActiveResult = assessmentEntry.TqSpecialismResults.Any(x => x.IsOptedin && x.EndDate == null);
                return !anyActiveResult;
            }
            else
            {
                return false;
            }
        }

        private static bool IsValidNextAssessmentSeries(IList<AssessmentSeries> dbAssessmentSeries, int regAcademicYear, string assessmentEntryName, int startYearOffset, ComponentType componentType)
        {
            // TODO: move to common
            var currentDate = DateTime.UtcNow.Date;

            var isValidNextAssessmentSeries = dbAssessmentSeries.Any(s => s.ComponentType == componentType &&
                s.Name.Equals(assessmentEntryName, StringComparison.InvariantCultureIgnoreCase) &&
                currentDate >= s.StartDate && currentDate <= s.EndDate &&
                s.Year > regAcademicYear + startYearOffset && s.Year <= regAcademicYear + Constants.AssessmentEndInYears);

            return isValidNextAssessmentSeries;
        }

        #endregion
    }
}
