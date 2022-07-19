using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Comparer;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class OverallResultCalculationService : IOverallResultCalculationService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IRepository<OverallGradeLookup> _overallGradeLookupRepository;
        private readonly IOverallResultCalculationRepository _overallGradeCalculationRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;
        private readonly IRepository<OverallResult> _overallResultRepository;
        private readonly IMapper _mapper;

        public OverallResultCalculationService(
            ResultsAndCertificationConfiguration configuration,
            IRepository<TlLookup> tlLookupRepository,
            IRepository<OverallGradeLookup> overallGradeLookupRepository,
            IOverallResultCalculationRepository overallGradeCalculationRepository,
            IRepository<AssessmentSeries> assessmentSeriesRepository,
            IRepository<OverallResult> overallResultRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _tlLookupRepository = tlLookupRepository;
            _overallGradeLookupRepository = overallGradeLookupRepository;
            _overallGradeCalculationRepository = overallGradeCalculationRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _overallResultRepository = overallResultRepository;
            _mapper = mapper;
        }

        public async Task<AssessmentSeries> GetResultCalculationAssessmentAsync(DateTime runDate)
        {
            var assessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => a.ComponentType == ComponentType.Core && runDate >= a.StartDate && runDate <= a.EndDate);
            if (currentAssessmentSeries == null)
                throw new Exception($"There is no AssessmentSeries available for the date {runDate}");

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => a.ComponentType == ComponentType.Core && dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            // Dev note: This method left to test from api end-point
            var previousAssessment = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) - 1;

            //var result = await CalculateOverallResultsAsync(runDate);

            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, previousAssessment?.ResultCalculationYear ?? 0);
        }

        public async Task<List<OverallResultResponse>> CalculateOverallResultsAsync(DateTime runDate)
        {
            var response = new List<OverallResultResponse>();
            var resultCalculationAssessment = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (resultCalculationAssessment.ResultCalculationYear ?? 0) - (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? 
                Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) + 1;

            var learners = await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, resultCalculationAssessment.ResultCalculationYear ?? 0);

            if (learners == null || !learners.Any())
                return null;

            var tlLookupData = await GetTlLookupData();
            var overallGradeLookupData = await GetOverallGradeLookupData();

            var batchSize = _configuration.OverallResultBatchSettings.BatchSize <= 0 ? Constants.OverallResultDefaultBatchSize : _configuration.OverallResultBatchSettings.BatchSize;
            var batchesToProcess = (int)Math.Ceiling(learners.Count / (decimal)batchSize);

            for (var batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                var leanersToProcess = learners.Skip(batchIndex * batchSize).Take(batchSize);
                response.Add(await ProcessOverallResults(leanersToProcess, tlLookupData, overallGradeLookupData, resultCalculationAssessment));
            }

            return response;
        }

        private async Task<OverallResultResponse> ProcessOverallResults(IEnumerable<TqRegistrationPathway> learnerPathways, List<TlLookup> tlLookup, List<OverallGradeLookup> overallGradeLookupData, AssessmentSeries assessmentSeries)
        {
            var reconciledLearnerRecords = ReconcileLearnersData(learnerPathways, tlLookup, overallGradeLookupData, assessmentSeries);

            var totalRecords = learnerPathways.Count();
            var updatedRecords = reconciledLearnerRecords.Count(x => x.Id != 0);
            var newRecords = reconciledLearnerRecords.Count(x => x.Id == 0) - updatedRecords;
            var unChangedRecords = totalRecords - (updatedRecords + newRecords);

            // Save.
            var isSuccess = await SaveOverallResultsAsync(reconciledLearnerRecords);

            return new OverallResultResponse { IsSuccess = isSuccess, TotalRecords = totalRecords, UpdatedRecords = updatedRecords, NewRecords = newRecords, UnChangedRecords = unChangedRecords, SavedRecords = isSuccess ? reconciledLearnerRecords.Count : 0 };
        }

        public PrintCertificateType? GetPrintCertificateType(List<TlLookup> overallResultLookupData, string overallGrade)
        {
            if (string.IsNullOrWhiteSpace(overallGrade))
                return null;

            var isValidOverallGrade = overallResultLookupData.Any(o => o.Value.Equals(overallGrade, StringComparison.InvariantCultureIgnoreCase));

            if (!isValidOverallGrade)
                return null;

            var unclassifiedGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var noResultGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var partialAchievementGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (overallGrade.Equals(unclassifiedGrade, StringComparison.InvariantCultureIgnoreCase) ||
                overallGrade.Equals(noResultGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else if (overallGrade.Equals(partialAchievementGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                return PrintCertificateType.StatementOfAchievement;
            }
            else
            {
                return PrintCertificateType.Certificate;
            }
        }

        public static CalculationStatus GetCalculationStatus(List<TlLookup> overallResultLookupData, string overallGrade, PrsStatus? pathwayResultPrsStatus, PrsStatus? specialismResultPrsStatus)
        {
            var unclassifiedGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var noResultGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var partialAchievementGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            CalculationStatus calculationStatus;
            if (overallGrade.Equals(unclassifiedGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                calculationStatus = CalculationStatus.Unclassified;
            }
            else if (overallGrade.Equals(noResultGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                calculationStatus = CalculationStatus.NoResult;
            }
            else if (overallGrade.Equals(partialAchievementGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                if (pathwayResultPrsStatus == PrsStatus.UnderReview || specialismResultPrsStatus == PrsStatus.UnderReview)
                {
                    calculationStatus = CalculationStatus.PartiallyCompletedRommRaised;
                }
                else if (pathwayResultPrsStatus == PrsStatus.BeingAppealed || specialismResultPrsStatus == PrsStatus.BeingAppealed)
                {
                    calculationStatus = CalculationStatus.PartiallyCompletedAppealRaised;
                }
                else
                {
                    calculationStatus = CalculationStatus.PartiallyCompleted;
                }
            }
            else
            {
                if (pathwayResultPrsStatus == PrsStatus.UnderReview || specialismResultPrsStatus == PrsStatus.UnderReview)
                {
                    calculationStatus = CalculationStatus.CompletedRommRaised;
                }
                else if (pathwayResultPrsStatus == PrsStatus.BeingAppealed || specialismResultPrsStatus == PrsStatus.BeingAppealed)
                {
                    calculationStatus = CalculationStatus.CompletedAppealRaised;
                }
                else
                {
                    calculationStatus = CalculationStatus.Completed;
                }
            }

            return calculationStatus;
        }

        public TqPathwayResult GetHighestPathwayResult(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqPathwayAssessments.Any())
                return null;

            var pathwayHigherResult = learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return pathwayHigherResult;
        }

        public TqSpecialismResult GetHighestSpecialismResult(TqRegistrationSpecialism specialism)
        {
            if (specialism == null || !specialism.TqSpecialismAssessments.Any())
                return null;

            var specialismHigherResult = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return specialismHigherResult;
        }

        public List<TqSpecialismResult> GetHighestSpecialismResult(TqRegistrationPathway learnerPathway)
        {
            var specialismResults = new List<TqSpecialismResult>();
            foreach (var specialism in learnerPathway.TqRegistrationSpecialisms.Where(specialism => specialism.TqSpecialismAssessments.Any()))
            {
                var specialismHigherResult = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();
                specialismResults.Add(specialismHigherResult);
            }

            return specialismResults;
        }

        public string GetOverAllGrade(List<TlLookup> overallResultLookup, List<OverallGradeLookup> overallGradeLookup, int tlPathwayId, int? pathwayGradeId, int? speciailsmGradeId, IndustryPlacementStatus ipStatus)
        {
            var isPathwayGradeUnclassified = pathwayGradeId.HasValue ? overallResultLookup.Any(o => o.Id == pathwayGradeId.Value && o.Code.Equals(Constants.PathwayComponentGradeUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase)) : false;
            var isSpecialismGradeUnclassified = speciailsmGradeId.HasValue ? overallResultLookup.Any(o => o.Id == speciailsmGradeId.Value && o.Code.Equals(Constants.SpecialismComponentGradeUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase)) : false;

            var overallResultUnClassified = overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var overallResultXNoResult = overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var overallResultPartialAchievement = overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                if (pathwayGradeId.HasValue && speciailsmGradeId.HasValue && !isPathwayGradeUnclassified && !isSpecialismGradeUnclassified)
                {
                    var overallGrade = overallGradeLookup.FirstOrDefault(o => o.TlPathwayId == tlPathwayId && o.TlLookupCoreGradeId == pathwayGradeId && o.TlLookupSpecialismGradeId == speciailsmGradeId);
                    return overallGrade?.TlLookupOverallGrade?.Value;
                }
                else
                {
                    return overallResultPartialAchievement;
                }
            }
            else
            {
                // if ipStatus is NotSpecified || ipStatus is NotCompleted then apply below logic

                if (pathwayGradeId.HasValue && speciailsmGradeId.HasValue)
                {
                    return isPathwayGradeUnclassified && isSpecialismGradeUnclassified ? overallResultUnClassified : overallResultPartialAchievement;
                }
                else if (pathwayGradeId.HasValue && !speciailsmGradeId.HasValue)
                {
                    return isPathwayGradeUnclassified ? overallResultUnClassified : overallResultPartialAchievement;
                }
                else if (!pathwayGradeId.HasValue && speciailsmGradeId.HasValue)
                {
                    return isSpecialismGradeUnclassified ? overallResultUnClassified : overallResultPartialAchievement;
                }
                else
                {
                    // if pathwayGradeId is null && speciailsmGradeId is null return X - no result
                    return overallResultXNoResult;
                }
            }
        }

        public IList<OverallResult> ReconcileLearnersData(IEnumerable<TqRegistrationPathway> learnerPathways, List<TlLookup> tlLookup, List<OverallGradeLookup> overallGradeLookupData, AssessmentSeries assessmentSeries)
        {
            var overallResults = new List<OverallResult>();

            foreach (var pathway in learnerPathways)
            {
                var specialism = pathway.TqRegistrationSpecialisms.FirstOrDefault();

                var pathwayResult = GetHighestPathwayResult(pathway);
                var specialismResult = GetHighestSpecialismResult(specialism); // as we are not dealing with couplet specialisms as of now
                var ipStatus = GetIndustryPlacementStatus(pathway);

                var overallGrade = GetOverAllGrade(tlLookup, overallGradeLookupData, pathway.TqProvider.TqAwardingOrganisation.TlPathwayId, pathwayResult?.TlLookupId, specialismResult?.TlLookupId, ipStatus);

                if (string.IsNullOrWhiteSpace(overallGrade))
                    throw new ApplicationException("OverallGrade cannot be null");

                var pathwayResultPrsStatus = HasAnyPathwayResultPrsStatusOutstanding(pathway);
                var specialismResultPrsStatus = HasAnySpecialismResultPrsStatusOutstanding(specialism);
                var calculationStatus = GetCalculationStatus(tlLookup, overallGrade, pathwayResultPrsStatus, specialismResultPrsStatus);
                var certificateType = GetPrintCertificateType(tlLookup, overallGrade);

                var overallResultDetails = new OverallResultDetail
                {
                    TlevelTitle = pathway.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                    PathwayName = pathway.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    PathwayLarId = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    PathwayResult = pathwayResult?.TlLookup?.Value,
                    SpecialismDetails = new List<OverallSpecialismDetail>
                        {
                            new OverallSpecialismDetail
                            {
                                SpecialismName = specialism?.TlSpecialism?.Name,
                                SpecialismLarId = specialism?.TlSpecialism?.LarId,
                                SpecialismResult = specialismResult?.TlLookup?.Value
                            }
                        },
                    IndustryPlacementStatus = GetIndustryPlacementStatusDisplayName(ipStatus),
                    OverallResult = overallGrade
                };

                var overallResult = new OverallResult
                {
                    TqRegistrationPathwayId = pathway.Id,
                    Details = JsonConvert.SerializeObject(overallResultDetails),
                    ResultAwarded = overallGrade,
                    CalculationStatus = calculationStatus,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    PrintAvailableFrom = certificateType != null ? assessmentSeries.AppealEndDate.AddDays(1) : null,
                    IsOptedin = true,
                    CertificateType = certificateType,
                    StartDate = DateTime.UtcNow,
                    CreatedBy = Constants.DefaultPerformedBy
                };

                var existingOverallResult = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                if (existingOverallResult == null)
                    overallResults.Add(overallResult);
                else
                {
                    if (IsOverallResultChangedFromPrevious(overallResult, existingOverallResult))
                    {
                        existingOverallResult.IsOptedin = false;
                        existingOverallResult.EndDate = DateTime.UtcNow;
                        existingOverallResult.ModifiedOn = DateTime.UtcNow;
                        existingOverallResult.ModifiedBy = Constants.DefaultPerformedBy;
                        overallResults.Add(existingOverallResult);

                        overallResults.Add(overallResult);
                    }
                }
            }

            return overallResults;
        }

        public async Task<bool> SaveOverallResultsAsync(IList<OverallResult> overallResults)
        {
            return await _overallResultRepository.UpdateManyAsync(overallResults) > 0;
        }

        public IndustryPlacementStatus GetIndustryPlacementStatus(TqRegistrationPathway pathway)
        {
            return pathway.IndustryPlacements.Any() ? pathway.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;
        }

        public string GetIndustryPlacementStatusDisplayName(IndustryPlacementStatus ipStatus)
        {
            if (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return ipStatus.GetDisplayName();

            return IndustryPlacementStatus.NotCompleted.GetDisplayName();
        }

        public async Task<IList<DownloadOverallResultsData>> DownloadOverallResultsDataAsync(long providerUkprn)
        {
            // 1. Get PublishDate from previous assessment
            var previousAssessment = await GetResultCalculationAssessmentAsync(DateTime.UtcNow);
            var resultPublishDate = previousAssessment?.ResultPublishDate;
            if (resultPublishDate == null)
                return new List<DownloadOverallResultsData>();

            // 2. Get OverallResults on above PublishDate if date reached
            var overallResults = await _overallResultRepository.GetManyAsync(x =>
                                                x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                                x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == providerUkprn &&
                                                x.PublishDate == resultPublishDate && DateTime.Today >= resultPublishDate,
                                                incl => incl.TqRegistrationPathway.TqRegistrationProfile)
                                        .OrderBy(x => x.TqRegistrationPathway.TqRegistrationProfile.Lastname)
                                        .ToListAsync();

            return _mapper.Map<IList<DownloadOverallResultsData>>(overallResults);
        }

        private async Task<List<TlLookup>> GetTlLookupData()
        {
            return await _tlLookupRepository.GetManyAsync().ToListAsync();
        }

        private async Task<List<OverallGradeLookup>> GetOverallGradeLookupData()
        {
            return await _overallGradeLookupRepository.GetManyAsync(navigationPropertyPath: new Expression<Func<OverallGradeLookup, object>>[]
                        {
                            o => o.TlLookupOverallGrade
                        }).ToListAsync();
        }

        public PrsStatus? HasAnyPathwayResultPrsStatusOutstanding(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqPathwayAssessments.Any())
                return null;

            if (learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).Any(x => x.PrsStatus == PrsStatus.UnderReview))
            {
                return PrsStatus.UnderReview;
            }
            else if (learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).Any(x => x.PrsStatus == PrsStatus.BeingAppealed))
            {
                return PrsStatus.BeingAppealed;
            }
            else
            {
                return null;
            }
        }

        public PrsStatus? HasAnySpecialismResultPrsStatusOutstanding(TqRegistrationSpecialism specialism)
        {
            if (specialism == null || !specialism.TqSpecialismAssessments.Any())
                return null;

            if (specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).Any(x => x.PrsStatus == PrsStatus.UnderReview))
            {
                return PrsStatus.UnderReview;
            }
            else if (specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).Any(x => x.PrsStatus == PrsStatus.BeingAppealed))
            {
                return PrsStatus.BeingAppealed;
            }
            else
            {
                return null;
            }
        }

        private bool HasAnySpecialismResultInRommOrAppeal(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqRegistrationSpecialisms.SelectMany(specialism => specialism.TqSpecialismAssessments).Any())
                return false;

            return learnerPathway.TqRegistrationSpecialisms.SelectMany(specialism => specialism.TqSpecialismAssessments).SelectMany(x => x.TqSpecialismResults).Any(x => x.PrsStatus == PrsStatus.UnderReview || x.PrsStatus == PrsStatus.BeingAppealed);
        }

        private bool IsOverallResultChangedFromPrevious(OverallResult latestOverallResult, OverallResult existingOverallResult)
        {
            var overallResultComparer = new OverallResultEqualityComparer();
            var existingResult = new List<OverallResult> { existingOverallResult };
            var latestResult = new List<OverallResult> { latestOverallResult };

            // List Intersect used to rightly refer the Comparer.
            return !existingResult.Intersect(latestResult, overallResultComparer).Any();
        }
    }
}
