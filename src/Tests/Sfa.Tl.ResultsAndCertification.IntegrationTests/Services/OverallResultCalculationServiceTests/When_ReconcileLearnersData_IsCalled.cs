using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_ReconcileLearnersData_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IList<OverallResult> _actualResult;

        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;
        private List<TqSpecialismResult> _specialismResults;
        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;
        private List<long> _ulnsWithCalculatedResult;
        private List<long> _ulnWithNoChangeInCalculatedResult;
        private List<OverallResult> _expectedResult;
        private IList<OverallGradeLookup> _overallGradeLookups;
        private OverallGradeLookup _overallGradeLookup;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },   // Without result
                { 1111111112, RegistrationPathwayStatus.Active },   // With result
                //{ 1111111113, RegistrationPathwayStatus.Active },   // Same result
                //{ 1111111114, RegistrationPathwayStatus.Active }, // With different result
                //{ 1111111115, RegistrationPathwayStatus.Active },
                //{ 1111111116, RegistrationPathwayStatus.Active }
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

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            _specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            //Seed Ip
            //_ulnWithIndustryPlacements = new Dictionary<long, IndustryPlacementStatus>
            //{
            //    { 1111111113, IndustryPlacementStatus.Completed },
            //    //{ 1111111114, IndustryPlacementStatus.CompletedWithSpecialConsideration },
            //    //{ 1111111115, IndustryPlacementStatus.NotCompleted },
            //    //{ 1111111116, IndustryPlacementStatus.NotSpecified }
            //};
            //SeedIndustyPlacementData(_ulnWithIndustryPlacements);
            //DbContext.SaveChanges();

            //var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111113).TqRegistrationPathways.FirstOrDefault().Id;
            //var coreResultId = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
            //var splResultId = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
            //_overallGradeLookup = new OverallGradeLookup { TlPathwayId = 3, TlLookupCoreGradeId =  coreResultId, TlLookupSpecialismGradeId =  splResultId, TlLookupOverallGradeId = 17 };
            //OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, new List<OverallGradeLookup> { _overallGradeLookup });

            ////_ulnWithNoChangeInCalculatedResult = new List<long> { 1111111113, 1111111114 };
            ////var overallResult = new OverallResult {  }

            //// Seed Overall results
            //_ulnsWithCalculatedResult = new List<long> { 1111111113 };
            //SeedOverallResultData(_registrations, _ulnsWithCalculatedResult);


            DbContext.SaveChanges();


            // Depenencies
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    BatchSize = 10,
                    NoOfAcademicYearsToProcess = 4
                }
            };
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            OverallGradeLookupLogger = new Logger<GenericRepository<OverallGradeLookup>>(new NullLoggerFactory());
            OverallGradeLookupRepository = new GenericRepository<OverallGradeLookup>(OverallGradeLookupLogger, DbContext);
            AssessmentSeriesLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesLogger, DbContext);
            OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);
            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            await Task.CompletedTask;
            var learnerPathways = await OverallResultCalculationRepository.GetLearnersForOverallGradeCalculation(2020, 2020);
            var tlLookup = TlLookup.Where(x => x.Category.Equals("OverallResult", StringComparison.InvariantCultureIgnoreCase)).ToList();
            var assessmentSeries = await OverallResultCalculationService.GetResultCalculationAssessmentAsync(DateTime.Today.AddMonths(4));
            
            _expectedResult = new List<OverallResult>
            {
                new OverallResult
                {
                    TqRegistrationPathwayId = 1,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":null,\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":null}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"X - no result\"}",
                    CalculationStatus = CalculationStatus.NoResult,
                    ResultAwarded = "X - no result",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 2,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 3,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null
                }
            };

            _actualResult = OverallResultCalculationService.ReconcileLearnersData(learnerPathways, tlLookup, new List<OverallGradeLookup> { _overallGradeLookup }, assessmentSeries.ResultPublishDate);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();
            _actualResult.Count.Should().Be(2);

            var assertUlns = new List<long> {
                1111111111, 1111111112,
                //1111111113
            };

            foreach (var assertUln in assertUlns)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == assertUln).TqRegistrationPathways.FirstOrDefault().Id;
                var actualOverallResult = _actualResult.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId);
                var expectedOverallResult = _expectedResult.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId);

                // Assert OverallResults 
                actualOverallResult.TqRegistrationPathwayId.Should().Be(expectedOverallResult.TqRegistrationPathwayId);
                actualOverallResult.Details.Should().Be(expectedOverallResult.Details);
                actualOverallResult.ResultAwarded.Should().Be(expectedOverallResult.ResultAwarded);
                actualOverallResult.CalculationStatus.Should().Be(expectedOverallResult.CalculationStatus);
                actualOverallResult.PrintAvailableFrom.Should().Be(expectedOverallResult.PrintAvailableFrom);
                actualOverallResult.PublishDate.Should().Be(expectedOverallResult.PublishDate);
                actualOverallResult.EndDate.Should().Be(expectedOverallResult.EndDate);
            }
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
