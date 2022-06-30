using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
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
    public class When_GetResultCalculationAssessment_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private AssessmentSeries _actualAssessmentSeries;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

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

        public async Task WhenAsync(DateTime runDate)
        {
            _actualAssessmentSeries = await OverallResultCalculationService.GetResultCalculationAssessmentAsync(runDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(DateTime runDate, string expectedSeries)
        {
            await WhenAsync(runDate);
            if (string.IsNullOrEmpty(expectedSeries))
            {
                _actualAssessmentSeries.Should().BeNull();
                return;
            }

            var expectedAssessmentSeries = DbContext.AssessmentSeries.FirstOrDefault(x => x.Name.Equals(expectedSeries, StringComparison.InvariantCultureIgnoreCase));
            _actualAssessmentSeries.Should().BeEquivalentTo(expectedAssessmentSeries);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { DateTime.Today, null },
                    new object[] { DateTime.Today.AddMonths(4), "Summer 2021" },
                    new object[] { DateTime.Today.AddMonths(7), "Autumn 2021" },
                    new object[] { DateTime.Today.AddMonths(10), "Summer 2022" }
                };
            }
        }
    }
}
