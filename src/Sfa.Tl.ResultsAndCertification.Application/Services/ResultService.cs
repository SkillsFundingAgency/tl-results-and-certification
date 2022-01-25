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

                // 4. Assessment Series does not exists
                if (!string.IsNullOrWhiteSpace(result.CoreAssessmentSeries))
                {
                    var isSeriesFound = dbAssessmentSeries.Any(x => x.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (!isSeriesFound)
                    {
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreAssessmentSeriesEntry));
                        isCoreAndAssesssmentSeriesValid = false;
                    }
                }

                // 5. Core component grade not valid - needs to be A* to E, or Unclassified
                var pathwayComponentGrades = tlLookup.Where(lr => lr.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (!string.IsNullOrWhiteSpace(result.CoreGrade))
                {
                    var isValidCoreComponentGrade = pathwayComponentGrades.Any(pcg => pcg.Value.Equals(result.CoreGrade, StringComparison.InvariantCultureIgnoreCase));
                    if (!isValidCoreComponentGrade)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.InvalidCoreComponentGrade));
                }

                // 6. No assessment entry is currently active
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

                // 7. Assessment entry mapping error 
                if (isCoreAndAssesssmentSeriesValid && hasActiveCoreAssessmentEntry && !string.IsNullOrWhiteSpace(result.CoreAssessmentSeries))
                {
                    var hasAssessmentSeriesMatchTheSeriesOnRegistrationCore = dbRegistration.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.EndDate == null && pa.AssessmentSeries.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (!hasAssessmentSeriesMatchTheSeriesOnRegistrationCore)
                        validationErrors.Add(BuildValidationError(result, ValidationMessages.AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration));
                }

                if (validationErrors.Any())
                    response.Add(new ResultRecordResponse { ValidationErrors = validationErrors });
                else
                {
                    var pathwayAssessment = dbRegistration.TqPathwayAssessments.FirstOrDefault(pa => pa.IsOptedin && pa.EndDate == null && pa.AssessmentSeries.Name.Equals(result.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    var pathwayComponentGrade = pathwayComponentGrades.FirstOrDefault(pcg => pcg.Value.Equals(result.CoreGrade, StringComparison.InvariantCultureIgnoreCase));

                    response.Add(new ResultRecordResponse
                    {
                        TqPathwayAssessmentId = !string.IsNullOrWhiteSpace(result.CoreCode) ? pathwayAssessment?.Id : null,
                        PathwayComponentGradeLookupId = !string.IsNullOrWhiteSpace(result.CoreGrade) ? pathwayComponentGrade?.Id : null
                    });
                }
            }
            return response;
        }

        public IList<TqPathwayResult> TransformResultsModel(IList<ResultRecordResponse> resultsData, string performedBy)
        {
            var pathwayResults = new List<TqPathwayResult>();

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
            }
            return pathwayResults;
        }

        public async Task<ResultProcessResponse> CompareAndProcessResultsAsync(IList<TqPathwayResult> pathwayResultsToProcess)
        {
            var response = new ResultProcessResponse();

            // Prepare Pathway Results
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

            if (response.IsValid)
            {
                if (newPathwayResults.Any())
                    newAndAmendedPathwayResultRecords.AddRange(newPathwayResults.Where(p => p.TqPathwayAssessmentId > 0 && p.TlLookupId > 0));

                // Process Results
                response.IsSuccess = await _resultRepository.BulkInsertOrUpdateResults(newAndAmendedPathwayResultRecords);
            }
            
            return response;
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
            else if(request.ComponentType == ComponentType.Specialism)
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
            if (request.ComponentType != ComponentType.Core)
                return new ChangeResultResponse { IsSuccess = false };

            var existingPathwayResult = await _pathwayResultRepository.GetFirstOrDefaultAsync(pr => pr.Id == request.ResultId && pr.EndDate == null && pr.IsOptedin
                                                                         && pr.TqPathwayAssessment.EndDate == null && pr.IsOptedin
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                         && pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn);

            if (existingPathwayResult == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to change Pathway Result for ProfileId = {request.ProfileId} and ResultId = {request.ResultId}. Method: ChangeResultAsync({request})");
                return new ChangeResultResponse { IsSuccess = false };
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

            var isSuccess = await _pathwayResultRepository.UpdateManyAsync(pathwayResultsToUpdate) > 0;

            return new ChangeResultResponse { Uln = request.Uln, ProfileId = request.ProfileId, IsSuccess = isSuccess };
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

            if(addResultRequest.ComponentType == ComponentType.Core)
            {
                var assessmentEntry = registrationPathway.TqPathwayAssessments.FirstOrDefault(p => p.Id == addResultRequest.AssessmentId && p.IsOptedin && p.EndDate == null);

                if (assessmentEntry == null) return false;

                var anyActiveResult = assessmentEntry.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null);
                return !anyActiveResult;
            }
            else if(addResultRequest.ComponentType == ComponentType.Specialism)
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

        #endregion
    }
}
