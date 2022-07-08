using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetHighestPathwayResult_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private TqPathwayResult _actualResult;

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

            var currentAcademicYear = GetAcademicYear();

            _registrations.ForEach(x =>
            {
                x.TqRegistrationPathways.ToList().ForEach(p => p.AcademicYear = currentAcademicYear - 1);
            });

            var componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114, 1111111115 };
            var componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Summer {currentAcademicYear}");

            componentWithAssessments = new List<long> { 1111111111, 1111111112, 1111111113 };
            componentWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
            SeedAssessmentsAndResults(_registrations, componentWithAssessments, componentWithResults, $"Autumn {currentAcademicYear}");

            SetAssessmentResult(1111111111, $"Summer {currentAcademicYear}", "B", "Merit");
            SetAssessmentResult(1111111112, $"Autumn {currentAcademicYear}", "B", "Pass");

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
            OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);

            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository, OverallResultRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;            
        }

        public async Task WhenAsync(TqRegistrationPathway pathway)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetHighestPathwayResult(pathway);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, bool hasHighestResult, string expectedHighestGrade)
        {
            var pathway = _registrations.SelectMany(r => r.TqRegistrationPathways).FirstOrDefault(r => r.TqRegistrationProfile.UniqueLearnerNumber == uln);

            await WhenAsync(pathway);

            if (hasHighestResult == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            _actualResult.TlLookup.Value.Should().Be(expectedHighestGrade);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, true, "A*" },
                    new object[] { 1111111112, true, "A*" },
                    new object[] { 1111111113, true, "A*" },
                    new object[] { 1111111114, false, null },
                    new object[] { 1111111115, false, null }
                };
            }
        }

        private void SetAssessmentResult(long uln, string seriesName, string coreGrade, string specialismGrade)
        {
            var currentResult = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.TqPathwayAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            currentResult.TlLookup = PathwayComponentGrades.FirstOrDefault(x => x.Value.Equals(coreGrade, StringComparison.InvariantCultureIgnoreCase));

            var currentSpecialismResult = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln &&
            x.TqSpecialismAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            var x = DbContext.TqSpecialismResult.Where(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            var y = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.AssessmentSeries.Name.Equals(seriesName, StringComparison.InvariantCultureIgnoreCase));
            currentSpecialismResult.TlLookup = SpecialismComponentGrades.FirstOrDefault(x => x.Value.Equals(specialismGrade, StringComparison.InvariantCultureIgnoreCase));

            DbContext.SaveChanges();
        }
    }
}
