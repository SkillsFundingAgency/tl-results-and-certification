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
    public class When_GetCalculationStatus_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IList<TlLookup> _tlLookups;
        private CalculationStatus _actualResult;

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

        public async Task WhenAsync(string overallGrade, PrsStatus? pathwayResultPrsStatus, PrsStatus? specialismResultPrsStatus)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetCalculationStatus(_tlLookups.ToList(), overallGrade, pathwayResultPrsStatus, specialismResultPrsStatus);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(string overallGrade, PrsStatus? pathwayResultPrsStatus, PrsStatus? specialismResultPrsStatus, CalculationStatus expectedResult)
        {
            await WhenAsync(overallGrade, pathwayResultPrsStatus, specialismResultPrsStatus);

            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { "Unclassified", null, null, CalculationStatus.Unclassified},
                    new object[] { "X - no result", null, null, CalculationStatus.NoResult},
                    new object[] { "Partial achievement", null, null, CalculationStatus.PartiallyCompleted},
                    new object[] { "Partial achievement", PrsStatus.UnderReview, null, CalculationStatus.PartiallyCompletedRommRaised},
                    new object[] { "Partial achievement", null, PrsStatus.UnderReview, CalculationStatus.PartiallyCompletedRommRaised},
                    new object[] { "Partial achievement", PrsStatus.UnderReview, PrsStatus.UnderReview, CalculationStatus.PartiallyCompletedRommRaised},
                    new object[] { "Partial achievement", PrsStatus.BeingAppealed, null, CalculationStatus.PartiallyCompletedAppealRaised},
                    new object[] { "Partial achievement", null, PrsStatus.BeingAppealed, CalculationStatus.PartiallyCompletedAppealRaised},
                    new object[] { "Partial achievement", PrsStatus.BeingAppealed, PrsStatus.BeingAppealed, CalculationStatus.PartiallyCompletedAppealRaised},

                    new object[] { "Distinction*", null, null, CalculationStatus.Completed},
                    new object[] { "Distinction*", PrsStatus.Reviewed, PrsStatus.Reviewed, CalculationStatus.Completed},
                    new object[] { "Distinction*", PrsStatus.Final, PrsStatus.Final, CalculationStatus.Completed},
                    new object[] { "Distinction*", PrsStatus.UnderReview, null, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction*", null, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction*", PrsStatus.UnderReview, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction*", PrsStatus.BeingAppealed, null, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Distinction*", null, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Distinction*", PrsStatus.BeingAppealed, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},

                    new object[] { "Distinction", null, null, CalculationStatus.Completed},
                    new object[] { "Distinction", PrsStatus.Reviewed, PrsStatus.Reviewed, CalculationStatus.Completed},
                    new object[] { "Distinction", PrsStatus.Final, PrsStatus.Final, CalculationStatus.Completed},
                    new object[] { "Distinction", PrsStatus.UnderReview, null, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction", null, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction", PrsStatus.UnderReview, PrsStatus.Reviewed, CalculationStatus.CompletedRommRaised},
                    new object[] { "Distinction", PrsStatus.BeingAppealed, null, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Distinction", null, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Distinction", PrsStatus.BeingAppealed, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},

                    new object[] { "Merit", null, null, CalculationStatus.Completed},
                    new object[] { "Merit", PrsStatus.Reviewed, PrsStatus.Reviewed, CalculationStatus.Completed},
                    new object[] { "Merit", PrsStatus.Final, PrsStatus.Final, CalculationStatus.Completed},
                    new object[] { "Merit", PrsStatus.UnderReview, null, CalculationStatus.CompletedRommRaised},
                    new object[] { "Merit", null, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Merit", PrsStatus.UnderReview, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Merit", PrsStatus.BeingAppealed, null, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Merit", null, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Merit", PrsStatus.BeingAppealed, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},

                    new object[] { "Pass", null, null, CalculationStatus.Completed},
                    new object[] { "Pass", PrsStatus.Reviewed, PrsStatus.Reviewed, CalculationStatus.Completed},
                    new object[] { "Pass", PrsStatus.Final, PrsStatus.Final, CalculationStatus.Completed},
                    new object[] { "Pass", PrsStatus.UnderReview, null, CalculationStatus.CompletedRommRaised},
                    new object[] { "Pass", null, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Pass", PrsStatus.Reviewed, PrsStatus.UnderReview, CalculationStatus.CompletedRommRaised},
                    new object[] { "Pass", PrsStatus.BeingAppealed, null, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Pass", null, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised},
                    new object[] { "Pass", PrsStatus.BeingAppealed, PrsStatus.BeingAppealed, CalculationStatus.CompletedAppealRaised}
                };
            }
        }
    }
}
