using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
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
    public class When_CalculateOverallResults_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private List<TqSpecialismResult> _specialismResults;
        private List<long> _ulnWithIndustryPlacements;
        private List<OverallResult> _overallResults; 
        private bool _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active },
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

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

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            _specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            _ulnWithIndustryPlacements = new List<long> { 1111111114, 1111111115, 1111111116 };
            SeedIndustyPlacementData(_ulnWithIndustryPlacements);

            // Dependencies
            {
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
                OverallResultLogger = new Logger<GenericRepository<OverallResult>>(new NullLoggerFactory());
                OverallResultRepository = new GenericRepository<OverallResult>(OverallResultLogger, DbContext);
            }
            // Create Service class to test. 
            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository, OverallResultRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(DateTime runDate)
        {
            _actualResult = await OverallResultCalculationService.CalculateOverallResultsAsync(runDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(DateTime runDate, bool expectedResult)
        {
            await WhenAsync(runDate);
            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // TODO: To be extended to in next sprint may need to query Database and assert the results. 
                    new object[] { DateTime.Today.AddMonths(4), true },
                };
            }
        }

        public void SeedIndustyPlacementData(IList<long> ulns)
        {
            foreach (var uln in ulns)
            {
                var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
            }
        }
    }
}
