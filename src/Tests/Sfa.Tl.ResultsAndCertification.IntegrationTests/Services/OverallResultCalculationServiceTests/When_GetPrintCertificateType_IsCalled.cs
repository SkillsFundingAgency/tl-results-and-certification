using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetPrintCertificateType_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IList<TlLookup> _tlLookups;
        private PrintCertificateType? _actualResult;

        public override void Given()
        {
            _tlLookups = TlLookupDataProvider.CreateFullTlLookupList(DbContext);
            DbContext.SaveChanges();

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

        public async Task WhenAsync(string overallGrade)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetPrintCertificateType(_tlLookups.ToList(), overallGrade);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(string overallGrade, PrintCertificateType? expectedResult)
        {
            await WhenAsync(overallGrade);

            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { null, null },
                    new object[] { "xyz", null },
                    new object[] { "Unclassified", null },
                    new object[] { "X - no result", null },
                    new object[] { "Partial achievement", PrintCertificateType.StatementOfAchievement},
                    new object[] { "Distinction*", PrintCertificateType.Certificate },
                    new object[] { "Distinction", PrintCertificateType.Certificate },
                    new object[] { "Merit", PrintCertificateType.Certificate },
                    new object[] { "Pass", PrintCertificateType.Certificate }
                };
            }
        }
    }
}
