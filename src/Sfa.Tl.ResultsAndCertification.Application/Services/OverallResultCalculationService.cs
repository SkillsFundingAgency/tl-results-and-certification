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
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class OverallResultCalculationService : IOverallResultCalculationService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository<TlLookup> _tlLookupRepository;
        private readonly IOverallResultCalculationRepository _overallGradeCalculationRepository;
        private readonly IAssessmentSeriesRepository _assessmentSeriesRepository;
        private readonly IOverallResultRepository _overallResultRepository;
        private readonly ISpecialismResultStrategyFactory _specialismResultStrategyFactory;
        private readonly IOverallGradeStrategyFactory _overallGradeStrategyFactory;
        private readonly IPathwayResultConverter _pathwayResultConverter;
        private readonly IIndustryPlacementStatusConverter _industryPlacementStatusConverter;

        private readonly OverallResultEqualityComparer _overallResultComparer = new();

        public OverallResultCalculationService(
            ResultsAndCertificationConfiguration configuration,
            IRepository<TlLookup> tlLookupRepository,
            IOverallResultCalculationRepository overallGradeCalculationRepository,
            IAssessmentSeriesRepository assessmentSeriesRepository,
            IOverallResultRepository overallResultRepository,
            ISpecialismResultStrategyFactory specialismResultStrategyFactory,
            IOverallGradeStrategyFactory overallGradeStrategyFactory,
            IPathwayResultConverter pathwayResultConverter,
            IIndustryPlacementStatusConverter industryPlacementStatusConverter)
        {
            _configuration = configuration;
            _tlLookupRepository = tlLookupRepository;
            _overallGradeCalculationRepository = overallGradeCalculationRepository;
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _overallResultRepository = overallResultRepository;
            _specialismResultStrategyFactory = specialismResultStrategyFactory;
            _overallGradeStrategyFactory = overallGradeStrategyFactory;
            _pathwayResultConverter = pathwayResultConverter;
            _industryPlacementStatusConverter = industryPlacementStatusConverter;
        }

        public async Task<List<OverallResultResponse>> CalculateOverallResultsAsync(DateTime runDate)
        {
            AssessmentSeries previousAssessmentSeries = await GetPreviousAssessmentSeriesAsync(runDate);
            IList<TqRegistrationPathway> registrations = await _overallGradeCalculationRepository.GetRegistrationsForOverallGradeCalculation(previousAssessmentSeries);

            if (registrations.IsNullOrEmpty())
                return null;

            List<TlLookup> tlLookupData = await _tlLookupRepository.GetManyAsync().ToListAsync();

            int batchSize = _configuration.OverallResultBatchSettings.BatchSize <= 0 ? Constants.OverallResultDefaultBatchSize : _configuration.OverallResultBatchSettings.BatchSize;
            int batchesToProcess = (int)Math.Ceiling(registrations.Count / (decimal)batchSize);

            var responses = new List<OverallResultResponse>();

            for (int batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                IEnumerable<TqRegistrationPathway> registrationsToProcess = registrations.Skip(batchIndex * batchSize).Take(batchSize);
                OverallResultResponse response = await ProcessOverallResults(registrationsToProcess, tlLookupData, previousAssessmentSeries);

                responses.Add(response);
            }

            return responses;
        }

        private async Task<AssessmentSeries> GetPreviousAssessmentSeriesAsync(DateTime runDate)
        {
            AssessmentSeries previousAssessmentSeries = await _assessmentSeriesRepository.GetPreviousAssessmentSeriesAsync(runDate);
            return previousAssessmentSeries ?? throw new Exception($"There is no previous assessment or result calculation year not available or result publish date not available.");
        }

        private async Task<OverallResultResponse> ProcessOverallResults(IEnumerable<TqRegistrationPathway> learnerPathways, List<TlLookup> tlLookup, AssessmentSeries assessmentSeries)
        {
            IList<OverallResult> reconciledLearnerRecords = await ReconcileLearnersData(learnerPathways, tlLookup, assessmentSeries);

            int totalRecords = learnerPathways.Count();
            int updatedRecords = reconciledLearnerRecords.Count(x => x.Id != 0);
            int newRecords = reconciledLearnerRecords.Count(x => x.Id == 0) - updatedRecords;
            int unChangedRecords = totalRecords - (updatedRecords + newRecords);

            // Save.
            bool isSuccess = !reconciledLearnerRecords.Any() || await SaveOverallResultsAsync(reconciledLearnerRecords);
            return new OverallResultResponse { IsSuccess = isSuccess, TotalRecords = totalRecords, UpdatedRecords = updatedRecords, NewRecords = newRecords, UnChangedRecords = unChangedRecords, SavedRecords = isSuccess ? reconciledLearnerRecords.Count : 0 };
        }

        private static CertificateStatus? GetCertificateStatus(CalculationStatus calculationStatus)
        {
            switch (calculationStatus)
            {
                case CalculationStatus.Completed:
                case CalculationStatus.PartiallyCompleted:
                    return CertificateStatus.AwaitingProcessing;
                default:
                    return null;
            }
        }

        private static PrintCertificateType? GetPrintCertificateType(List<TlLookup> overallResultLookupData, string overallGrade)
        {
            if (string.IsNullOrWhiteSpace(overallGrade))
                return null;

            var isValidOverallGrade = overallResultLookupData.Any(o => o.Value.Equals(overallGrade, StringComparison.InvariantCultureIgnoreCase));

            if (!isValidOverallGrade)
                return null;

            var qPendingGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultQpendingCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var unclassifiedGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var xNoResultGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var partialAchievementGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (overallGrade.Equals(qPendingGrade, StringComparison.InvariantCultureIgnoreCase) ||
                overallGrade.Equals(unclassifiedGrade, StringComparison.InvariantCultureIgnoreCase) ||
                overallGrade.Equals(xNoResultGrade, StringComparison.InvariantCultureIgnoreCase))
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

        private static CalculationStatus GetCalculationStatus(List<TlLookup> overallResultLookupData, string overallGrade, PrsStatus? pathwayResultPrsStatus, PrsStatus? specialismResultPrsStatus)
        {
            var qPendingGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultQpendingCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var unclassifiedGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var xNoResultGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var partialAchievementGrade = overallResultLookupData.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            CalculationStatus calculationStatus;

            if (overallGrade.Equals(qPendingGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                calculationStatus = CalculationStatus.Qpending;
            }
            else if (overallGrade.Equals(unclassifiedGrade, StringComparison.InvariantCultureIgnoreCase))
            {
                calculationStatus = CalculationStatus.Unclassified;
            }
            else if (overallGrade.Equals(xNoResultGrade, StringComparison.InvariantCultureIgnoreCase))
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

        private TqPathwayResult GetHighestPathwayResult(TqRegistrationPathway learnerPathway)
        {
            return _pathwayResultConverter.Convert(learnerPathway.TqPathwayAssessments, null);
        }

        private async Task<OverallSpecialismResultDetail> GetOverallSpecialismResult(IList<TlLookup> tlLookup, ICollection<TqRegistrationSpecialism> specialisms)
        {
            int numberOfSpecialisms = specialisms == null ? 0 : specialisms.Count;

            ISpecialismResultStrategy specialismResultStrategy = await _specialismResultStrategyFactory.GetSpecialismResultStrategyAsync(tlLookup, numberOfSpecialisms);
            return specialismResultStrategy.GetResult(specialisms);
        }

        private async Task<string> GetOverAllGrade(List<TlLookup> tlLookup, int tlPathwayId, int? pathwayGradeId, int? speciailsmGradeId, IndustryPlacementStatus ipStatus, int academicYear)
        {
            IOverallGradeStrategy overallGradeStrategy = await _overallGradeStrategyFactory.GetOverallGradeStrategy(academicYear, tlLookup);
            return overallGradeStrategy.GetOverAllGrade(tlPathwayId, pathwayGradeId, speciailsmGradeId, ipStatus);
        }

        private async Task<IList<OverallResult>> ReconcileLearnersData(IEnumerable<TqRegistrationPathway> registrations, List<TlLookup> tlLookup, AssessmentSeries assessmentSeries)
        {
            var results = new List<OverallResult>();

            foreach (TqRegistrationPathway registration in registrations)
            {
                TqPathwayResult pathwayResult = GetHighestPathwayResult(registration);
                OverallSpecialismResultDetail overallSpecialismResult = await GetOverallSpecialismResult(tlLookup, registration.TqRegistrationSpecialisms);
                IndustryPlacementStatus ipStatus = GetIndustryPlacementStatus(registration);

                string overallGrade = await GetOverAllGrade(tlLookup, registration.TqProvider.TqAwardingOrganisation.TlPathwayId, pathwayResult?.TlLookupId, overallSpecialismResult?.TlLookupId, ipStatus, registration.AcademicYear);

                if (string.IsNullOrWhiteSpace(overallGrade))
                    throw new ApplicationException("OverallGrade cannot be null");

                PrsStatus? pathwayResultPrsStatus = HasAnyPathwayResultPrsStatusOutstanding(registration);
                PrsStatus? specialismResultPrsStatus = HasAnySpecialismResultPrsStatusOutstanding(registration.TqRegistrationSpecialisms);
                CalculationStatus calculationStatus = GetCalculationStatus(tlLookup, overallGrade, pathwayResultPrsStatus, specialismResultPrsStatus);
                PrintCertificateType? certificateType = GetPrintCertificateType(tlLookup, overallGrade);
                CertificateStatus? certificateStatus = GetCertificateStatus(calculationStatus);

                var overallResultDetails = new OverallResultDetail
                {
                    TlevelTitle = registration.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                    PathwayName = registration.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    PathwayLarId = registration.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    PathwayResult = pathwayResult?.TlLookup?.Value,
                    SpecialismDetails = overallSpecialismResult.SpecialismDetails,
                    IndustryPlacementStatus = GetIndustryPlacementStatusDisplayName(ipStatus),
                    OverallResult = overallGrade
                };

                var overallResult = new OverallResult
                {
                    TqRegistrationPathwayId = registration.Id,
                    Details = JsonConvert.SerializeObject(overallResultDetails),
                    ResultAwarded = overallGrade,
                    SpecialismResultAwarded = overallSpecialismResult.OverallSpecialismResult,
                    CalculationStatus = calculationStatus,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    PrintAvailableFrom = certificateType != null ? assessmentSeries.PrintAvailableDate : null,
                    IsOptedin = true,
                    CertificateType = certificateType,
                    CertificateStatus = certificateStatus,
                    StartDate = DateTime.UtcNow,
                    CreatedBy = Constants.DefaultPerformedBy
                };

                var existingOverallResult = registration.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                if (existingOverallResult == null)
                    results.Add(overallResult);
                else
                {
                    if (IsOverallResultChangedFromPrevious(overallResult, existingOverallResult))
                    {
                        existingOverallResult.IsOptedin = false;
                        existingOverallResult.EndDate = DateTime.UtcNow;
                        existingOverallResult.ModifiedOn = DateTime.UtcNow;
                        existingOverallResult.ModifiedBy = Constants.DefaultPerformedBy;
                        results.Add(existingOverallResult);

                        results.Add(overallResult);
                    }
                }
            }

            return results;
        }

        private async Task<bool> SaveOverallResultsAsync(IList<OverallResult> overallResults)
            => await _overallResultRepository.UpdateManyAsync(overallResults) > 0;

        private IndustryPlacementStatus GetIndustryPlacementStatus(TqRegistrationPathway pathway)
            => _industryPlacementStatusConverter.Convert(pathway.IndustryPlacements, null);
        private static string GetIndustryPlacementStatusDisplayName(IndustryPlacementStatus ipStatus)
        {
            switch (ipStatus)
            {
                case IndustryPlacementStatus.Completed:
                case IndustryPlacementStatus.CompletedWithSpecialConsideration:
                case IndustryPlacementStatus.NotCompleted:
                case IndustryPlacementStatus.WillNotComplete:
                    return ipStatus.GetDisplayName();
                default:
                    return IndustryPlacementStatus.NotCompleted.GetDisplayName();
            }
        }

        private static PrsStatus? HasAnyPathwayResultPrsStatusOutstanding(TqRegistrationPathway learnerPathway)
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

        private static PrsStatus? HasAnySpecialismResultPrsStatusOutstanding(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null || !specialisms.Any() || specialisms.All(s => !s.TqSpecialismAssessments.Any()))
                return null;

            IEnumerable<TqSpecialismResult> results = specialisms.SelectMany(s => s.TqSpecialismAssessments).SelectMany(sa => sa.TqSpecialismResults);

            if (results.Any(x => x.PrsStatus == PrsStatus.UnderReview))
            {
                return PrsStatus.UnderReview;
            }
            else if (results.Any(x => x.PrsStatus == PrsStatus.BeingAppealed))
            {
                return PrsStatus.BeingAppealed;
            }
            else
            {
                return null;
            }
        }

        private bool IsOverallResultChangedFromPrevious(OverallResult latestOverallResult, OverallResult existingOverallResult)
        {
            var existingResult = new List<OverallResult> { existingOverallResult };
            var latestResult = new List<OverallResult> { latestOverallResult };

            // List Intersect used to rightly refer the Comparer.
            return !existingResult.Intersect(latestResult, _overallResultComparer).Any();
        }
    }
}