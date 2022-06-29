using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
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

        public OverallResultCalculationService(
            ResultsAndCertificationConfiguration configuration,
            IRepository<TlLookup> tlLookupRepository,
            IRepository<OverallGradeLookup> overallGradeLookupRepository,
            IOverallResultCalculationRepository overallGradeCalculationRepository,
            IRepository<AssessmentSeries> assessmentSeriesRepository)
        {
            _configuration = configuration;
            _tlLookupRepository = tlLookupRepository;
            _overallGradeLookupRepository = overallGradeLookupRepository;
            _overallGradeCalculationRepository = overallGradeCalculationRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
        }

        public async Task<AssessmentSeries> GetResultCalculationAssessmentAsync(DateTime runDate)
        {
            var assessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            var currentAssessmentSeries = assessmentSeries.FirstOrDefault(a => runDate >= a.StartDate && runDate <= a.EndDate);
            if (currentAssessmentSeries == null)
                throw new Exception($"There is no AssessmentSeries available for the date {runDate}");

            // Calculate result for recently completed assessment. 
            var dateFromPreviousAssessment = currentAssessmentSeries.StartDate.AddDays(-1);
            var previousAssessment = assessmentSeries.FirstOrDefault(a => dateFromPreviousAssessment >= a.StartDate && dateFromPreviousAssessment <= a.EndDate);

            return previousAssessment;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            // Dev note: This method left to test from api end-point
            var resultCalculationYear = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) - 1;

            return await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, resultCalculationYear?.ResultCalculationYear ?? 0);
        }

        public async Task<bool> CalculateOverallResultsAsync(DateTime runDate)
        {
            var resultCalculationAssessment = await GetResultCalculationAssessmentAsync(runDate);
            var resultCalculationYearFrom = (_configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess <= 0 ? Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess) - 1;
            var learners = await _overallGradeCalculationRepository.GetLearnersForOverallGradeCalculation(resultCalculationYearFrom, resultCalculationAssessment.ResultCalculationYear ?? 0);

            if (learners == null || !learners.Any())
                return true;

            var overallResultLookupData = await GetOverallResultLookupData();
            var overallGradeLookupData = await GetOverallGradeLookupData();

            var tasks = new List<Task>();
            var batchSize = _configuration.OverallResultBatchSettings.BatchSize <= 0 ? Constants.OverallResultDefaultBatchSize : _configuration.OverallResultBatchSettings.BatchSize;            
            var batchesToProcess = (int)Math.Ceiling(learners.Count / (decimal)batchSize);

            for (var batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                var leanersToProcess = learners.Skip(batchIndex * batchSize).Take(batchSize);
                tasks.Add(ProcessOverallResults(leanersToProcess, overallResultLookupData, overallGradeLookupData, resultCalculationAssessment));
            }

            await Task.WhenAll(tasks);
            return true;
        }

        private async Task ProcessOverallResults(IEnumerable<TqRegistrationPathway> learnerPathways, List<TlLookup> overallResultLookupData, List<OverallGradeLookup> overallGradeLookupData, AssessmentSeries assessmentSeries)
        {
            await Task.CompletedTask;

            var overallResults = new List<OverallResult>();

            foreach (var pathway in learnerPathways)
            {
                var specialism = pathway.TqRegistrationSpecialisms.FirstOrDefault();

                var pathwayResult = GetHighestPathwayResult(pathway);                
                var specialismResult = GetHighestSpecialismResult(specialism); // as we are not dealing with couplet specialisms as of now
                var ipStatus = pathway.IndustryPlacements.Any() ? pathway.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified;

                var overallGrade = GetOverAllGrade(overallResultLookupData, overallGradeLookupData, pathway.TqProvider.TqAwardingOrganisation.TlPathwayId, pathwayResult?.TlLookupId, specialismResult?.TlLookupId, ipStatus);

                if (!string.IsNullOrWhiteSpace(overallGrade))
                {
                    var pathwayResultPrsStatus = HasAnyPathwayResultPrsStatusOutstanding(pathway);
                    var specialismResultPrsStatus = HasAnySpecialismResultPrsStatusOutstanding(specialism);
                    var calculationStatus = GetCalculationStatus(overallResultLookupData, overallGrade, pathwayResultPrsStatus, specialismResultPrsStatus);

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
                        OverallResult = overallGrade
                    };

                    var overallResult = new OverallResult
                    {
                        TqRegistrationPathwayId = pathway.Id,
                        Details = JsonConvert.SerializeObject(overallResultDetails),
                        ResultAwarded = overallGrade,
                        CalculationStatus = calculationStatus,
                        PublishDate = assessmentSeries.ResultPublishDate,
                        PrintAvailableFrom = null,
                        StartDate = DateTime.UtcNow,
                        CreatedBy = Constants.DefaultPerformedBy
                    };
                }
                else
                {
                    // TODO : Log
                }
            }

            // Save.
        }

        private static CalculationStatus GetCalculationStatus(List<TlLookup> overallResultLookupData, string overallGrade, PrsStatus? pathwayResultPrsStatus, PrsStatus? specialismResultPrsStatus)
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
            if (pathwayGradeId.HasValue && speciailsmGradeId.HasValue && (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration))
            {
                var overallGrade = overallGradeLookup.FirstOrDefault(o => o.TlPathwayId == tlPathwayId && o.TlLookupCoreGradeId == pathwayGradeId && o.TlLookupSpecialismGradeId == speciailsmGradeId);

                return overallGrade?.TlLookupOverallGrade?.Value;
            }
            else if (!pathwayGradeId.HasValue && !speciailsmGradeId.HasValue && (ipStatus == IndustryPlacementStatus.NotSpecified || ipStatus == IndustryPlacementStatus.NotCompleted))
            {
                // X - no result
                var overallGrade =  overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase));
                return overallGrade?.Value;
            }
            else
            {
                // partial achievement
                var overallGrade = overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase));
                return overallGrade?.Value;
            }
        }

        private async Task<List<TlLookup>> GetOverallResultLookupData()
        {
            //return await _tlLookupRepository.GetManyAsync().ToListAsync();
            return await _tlLookupRepository.GetManyAsync(lkp => lkp.Category.Equals(LookupCategory.OverallResult.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        }

        private async Task<List<OverallGradeLookup>> GetOverallGradeLookupData()
        {
            return await _overallGradeLookupRepository.GetManyAsync(navigationPropertyPath: new Expression<Func<OverallGradeLookup, object>>[]
                        {
                            o => o.TlLookupOverallGrade
                        }).ToListAsync();
        }

        private PrsStatus? HasAnyPathwayResultPrsStatusOutstanding(TqRegistrationPathway learnerPathway)
        {
            if (!learnerPathway.TqPathwayAssessments.Any())
                return null;

            if(learnerPathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).Any(x => x.PrsStatus == PrsStatus.UnderReview))
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

        private PrsStatus? HasAnySpecialismResultPrsStatusOutstanding(TqRegistrationSpecialism specialism)
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
    }
}
