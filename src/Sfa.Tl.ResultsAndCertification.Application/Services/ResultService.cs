using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
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
        private readonly IMapper _mapper;

        public ResultService(IRepository<AssessmentSeries> assessmentSeriesRepository, IRepository<TlLookup> tlLookupRepository, IResultRepository resultRepository, IRepository<TqPathwayResult> pathwayResultRepository, IMapper mapper)
        {
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _tlLookupRepository = tlLookupRepository;
            _resultRepository = resultRepository;
            _pathwayResultRepository = pathwayResultRepository;
            _mapper = mapper;
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
            var newOrAmendedPathwayResultRecords = await PrepareNewAndAmendedPathwayResults(pathwayResultsToProcess);

            // Process Results
            response.IsSuccess = await _resultRepository.BulkInsertOrUpdateResults(newOrAmendedPathwayResultRecords);
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
            if (request.AssessmentEntryType == AssessmentEntryType.Core)
                status = await _pathwayResultRepository.CreateAsync(new TqPathwayResult
                {
                    TqPathwayAssessmentId = request.TqPathwayAssessmentId,
                    TlLookupId = request.TlLookupId,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });

            return new AddResultResponse { Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, IsSuccess = status > 0 };
        }
        
        public async Task<CoreResult> GetCoreResultAsync(long aoUkprn, int profileId, int assessmentId)
        {
            var tqRegistration = await _resultRepository.GetPathwayResultAsync(aoUkprn, profileId, assessmentId);
            return _mapper.Map<CoreResult>(tqRegistration);
        }

        public async Task<IEnumerable<LookupData>> GetLookupDataAsync(int lookupCategory)
        {
            // TODO: validate the aoUkprn -> Is this required?
            var lookupData = _tlLookupRepository.GetManyAsync(x => x.IsActive && x.Category == EnumExtensions.GetDisplayName<LookupCategory>(lookupCategory));
            //TODO: .OrderBy(x => x.Order); 
            return _mapper.Map<IEnumerable<LookupData>>(lookupData);
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

        private BulkProcessValidationError BuildValidationError(ResultCsvRecordResponse result, string message)
        {
            return new BulkProcessValidationError { RowNum = result.RowNum.ToString(), Uln = result.Uln.ToString(), ErrorMessage = message };
        }

        private async Task<List<TqPathwayResult>> PrepareNewAndAmendedPathwayResults(IList<TqPathwayResult> pathwayResultsToProcess)
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

            if (newPathwayResults.Any())
                newAndAmendedPathwayResultRecords.AddRange(newPathwayResults.Where(p => p.TqPathwayAssessmentId > 0 && p.TlLookupId > 0));

            return newAndAmendedPathwayResultRecords;
        }

        private bool IsValidAddResultRequestAsync(TqRegistrationPathway registrationPathway, AddResultRequest addResultRequest)
        {
            // 1. Must be an active registration.
            if (registrationPathway == null || registrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;


            if(addResultRequest.AssessmentEntryType == AssessmentEntryType.Core)
            {
                var assessmentEntry = registrationPathway.TqPathwayAssessments.FirstOrDefault(p => p.Id == addResultRequest.TqPathwayAssessmentId && p.IsOptedin && p.EndDate == null);

                if (assessmentEntry == null) return false;

                var anyActiveResult = assessmentEntry.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null);
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
