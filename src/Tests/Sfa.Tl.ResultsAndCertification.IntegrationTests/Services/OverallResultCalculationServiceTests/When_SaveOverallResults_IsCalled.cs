using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_SaveOverallResults_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private bool _actualResult;

        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;
        private List<long> _ulnsAlreadyWithCalculatedResult;
        private List<OverallGradeLookup> _overallGradeLookup;
        private List<OverallResult> _overallResultToSave;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },   // Without result
                { 1111111112, RegistrationPathwayStatus.Active },   // With result
                { 1111111113, RegistrationPathwayStatus.Active },   // With result + Already Calculated OverallResult - Same Outcome
                { 1111111114, RegistrationPathwayStatus.Active },   // With result + Already Calculated OverallResult - Different Outcome
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Build Pathway results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));

                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessments, isLatestActive, isHistoricAssessent));
            }

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            // Seed Ip
            _ulnWithIndustryPlacements = new Dictionary<long, IndustryPlacementStatus>
            {
                { 1111111112, IndustryPlacementStatus.NotCompleted },
                { 1111111113, IndustryPlacementStatus.Completed },
                { 1111111114, IndustryPlacementStatus.CompletedWithSpecialConsideration },
            };
            SeedIndustyPlacementData(_ulnWithIndustryPlacements);
            DbContext.SaveChanges();

            // Seed OverallResultLookup
            _ulnsAlreadyWithCalculatedResult = new List<long> { 1111111113, 1111111114 };
            _overallGradeLookup = new List<OverallGradeLookup>();
            foreach (var uln in _ulnsAlreadyWithCalculatedResult)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
                var coreResultId = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
                var splResultId = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 3, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
            }
            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

            // Seed Overall results
            var sameResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111113).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = sameResultPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed,IsOptedin = true } }, true);

            var differentCalcResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = differentCalcResultPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"C\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Pass\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Pass\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed,IsOptedin = true } }, true);


            DbContext.SaveChanges();

            // Dependencies
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    BatchSize = 10,
                    NoOfAcademicYearsToProcess = 4
                }
            };

            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            var tlLookup = TlLookup.Where(x => x.Category.ToLower().Equals(LookupCategory.OverallResult.ToString().ToLower(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            var assessmentSeries = await OverallResultCalculationService.GetResultCalculationAssessmentAsync(DateTime.Today.AddMonths(4));
            var learnerPathways = await OverallResultCalculationRepository.GetLearnersForOverallGradeCalculation(assessmentSeries);

            _overallResultToSave = new List<OverallResult>
            {
                new OverallResult
                {
                    TqRegistrationPathwayId = 1,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":null,\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":null}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"X - no result\"}",
                    CalculationStatus = CalculationStatus.NoResult,
                    ResultAwarded = "X - no result",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 2,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 4,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Distinction*\"}",
                    CalculationStatus = CalculationStatus.Completed,
                    ResultAwarded = "Distinction*",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 4,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"C\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Pass\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Pass\"}",
                    CalculationStatus = CalculationStatus.Completed,
                    ResultAwarded = "Distinction*",
                    PrintAvailableFrom = null,
                    PublishDate = null,
                    EndDate = DateTime.UtcNow,
                    IsOptedin = true
                }
            };

            _actualResult = await OverallResultCalculationService.SaveOverallResultsAsync(_overallResultToSave);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();
            _actualResult.Should().BeTrue();
        }

        public void SeedIndustyPlacementData(Dictionary<long, IndustryPlacementStatus> ipUlns)
        {
            foreach (var ipUln in ipUlns)
            {
                var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == ipUln.Key).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipUln.Value);
            }
        }
    }
}
