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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetOverAllGrade_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IList<TlLookup> _tlLookups;
        private IList<OverallGradeLookup> _overallGradeLookups;
        private string _actualResult;

        public override void Given()
        {
            _tlLookups = TlLookupDataProvider.CreateFullTlLookupList(DbContext);

            var seedData = new List<Tuple<int, int, int, int>>
            {
                new Tuple<int, int, int, int>(1, 1, 10, 17),
                new Tuple<int, int, int, int>(1, 1, 11, 18),
                new Tuple<int, int, int, int>(1, 1, 12, 18),
                new Tuple<int, int, int, int>(1, 2, 10, 18),
                new Tuple<int, int, int, int>(1, 2, 11, 18),
                new Tuple<int, int, int, int>(1, 2, 12, 19),
                new Tuple<int, int, int, int>(1, 3, 10, 18),
                new Tuple<int, int, int, int>(1, 3, 11, 19),
                new Tuple<int, int, int, int>(1, 3, 12, 19),
                new Tuple<int, int, int, int>(1, 4, 10, 19),
                new Tuple<int, int, int, int>(1, 4, 11, 19),
                new Tuple<int, int, int, int>(1, 4, 12, 20),
                new Tuple<int, int, int, int>(1, 5, 10, 19),
                new Tuple<int, int, int, int>(1, 5, 11, 20),
                new Tuple<int, int, int, int>(1, 5, 12, 20),
                new Tuple<int, int, int, int>(1, 6, 10, 20),
                new Tuple<int, int, int, int>(1, 6, 11, 20),
                new Tuple<int, int, int, int>(1, 6, 12, 20)
            };

            _overallGradeLookups = new OverallGradeLookupBuilder().BuildList(_tlLookups, seedData);
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

            OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);

            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(int pathwayId, int? pathwayGradeId, int? specialismGradeId, IndustryPlacementStatus ipStatus)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetOverAllGrade(_tlLookups.ToList(), _overallGradeLookups.ToList(), pathwayId, pathwayGradeId, specialismGradeId, ipStatus);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(int tlPathwayId, string pathwayGrade, string specialismGrade, IndustryPlacementStatus ipStatus, string overallGrade)
        {
            var pathwayGradeId = _tlLookups.FirstOrDefault(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(pathwayGrade, StringComparison.InvariantCultureIgnoreCase))?.Id;
            var specialismGradeId = _tlLookups.FirstOrDefault(x => x.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(specialismGrade, StringComparison.InvariantCultureIgnoreCase))?.Id;

            await WhenAsync(tlPathwayId, pathwayGradeId, specialismGradeId, ipStatus);

            _actualResult.Should().Be(overallGrade);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // IndustryPlacement Completed
                    new object[] { 1, "A*", "Distinction", IndustryPlacementStatus.Completed, "Distinction*" },
                    new object[] { 1, "A*", "Merit", IndustryPlacementStatus.Completed, "Distinction" },
                    new object[] { 1, "A*", "Pass", IndustryPlacementStatus.Completed, "Distinction" },
                    new object[] { 1, "A", "Distinction", IndustryPlacementStatus.Completed, "Distinction" },
                    new object[] { 1, "A", "Merit", IndustryPlacementStatus.Completed, "Distinction" },
                    new object[] { 1, "A", "Pass", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "B", "Distinction", IndustryPlacementStatus.Completed, "Distinction" },
                    new object[] { 1, "B", "Merit", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "B", "Pass", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "C", "Distinction", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "C", "Merit", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "C", "Pass", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "D", "Distinction", IndustryPlacementStatus.Completed, "Merit" },
                    new object[] { 1, "D", "Merit", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "D", "Pass", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "E", "Distinction", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "E", "Merit", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "E", "Pass", IndustryPlacementStatus.Completed, "Pass" },
                    new object[] { 1, "Unclassified", "Unclassified", IndustryPlacementStatus.Completed, "Partial achievement" },

                    //// IndustryPlacement Not Completed
                    new object[] { 1, "A*", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "A*", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "A*", "Pass", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "A", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "A", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "A", "Pass", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "B", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "B", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "B", "Pass", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "C", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "C", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "C", "Pass", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "D", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "D", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "D", "Pass", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "E", "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "E", "Merit", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "E", "Pass", IndustryPlacementStatus.NotSpecified, "Partial achievement" },

                    new object[] { 1, "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted, "Unclassified" },
                    new object[] { 1, "A*", null, IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, "Unclassified", null, IndustryPlacementStatus.NotCompleted, "Unclassified" },
                    new object[] { 1, null, "Distinction", IndustryPlacementStatus.NotCompleted, "Partial achievement" },
                    new object[] { 1, null, "Unclassified", IndustryPlacementStatus.NotCompleted, "Unclassified" },
                    new object[] { 1, null, null, IndustryPlacementStatus.NotCompleted, "X - no result" }
                };
            }
        }
    }
}
