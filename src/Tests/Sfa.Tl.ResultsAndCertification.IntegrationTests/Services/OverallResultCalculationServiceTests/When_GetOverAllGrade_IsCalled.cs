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
        public async Task Then_Expected_Results_Are_Returned(int tlPathwayId, string pathwayGrade, string specialismGrade, string overallGrade, IndustryPlacementStatus ipStatus)
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
                    new object[] { 1, "A*", "Distinction", "Distinction*", IndustryPlacementStatus.Completed },
                    new object[] { 1, "A*", "Merit", "Distinction", IndustryPlacementStatus.Completed },
                    new object[] { 1, "A*", "Pass", "Distinction", IndustryPlacementStatus.Completed },
                    new object[] { 1, "A", "Distinction", "Distinction", IndustryPlacementStatus.Completed },
                    new object[] { 1, "A", "Merit", "Distinction", IndustryPlacementStatus.Completed },
                    new object[] { 1, "A", "Pass", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "B", "Distinction", "Distinction", IndustryPlacementStatus.Completed },
                    new object[] { 1, "B", "Merit", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "B", "Pass", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "C", "Distinction", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "C", "Merit", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "C", "Pass", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "D", "Distinction", "Merit", IndustryPlacementStatus.Completed },
                    new object[] { 1, "D", "Merit", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "D", "Pass", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "E", "Distinction", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "E", "Merit", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "E", "Pass", "Pass", IndustryPlacementStatus.Completed },
                    new object[] { 1, "Unclassified", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed },

                    // IndustryPlacement Not Completed
                    new object[] { 1, "A*", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A*", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A*", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "B", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "B", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "B", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "C", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "C", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "C", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "D", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "D", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "D", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "E", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "E", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "E", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified },

                    new object[] { 1, "Unclassified", "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "A*", null, "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, "Unclassified", null, "Unclassified", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, null, "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, null, "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted },
                    new object[] { 1, null, null, "X - no result", IndustryPlacementStatus.NotCompleted }
                };
            }
        }
    }
}
