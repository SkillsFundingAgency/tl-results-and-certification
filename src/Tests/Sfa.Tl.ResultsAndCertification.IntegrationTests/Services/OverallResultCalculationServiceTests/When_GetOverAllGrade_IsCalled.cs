//using FluentAssertions;
//using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
//using Sfa.Tl.ResultsAndCertification.Application.Services;
//using Sfa.Tl.ResultsAndCertification.Application.Strategies;
//using Sfa.Tl.ResultsAndCertification.Common.Enum;
//using Sfa.Tl.ResultsAndCertification.Domain.Models;
//using Sfa.Tl.ResultsAndCertification.Models.Configuration;
//using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
//using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
//{
//    public class When_GetOverAllGrade_IsCalled : OverallResultCalculationServiceBaseTest
//    {
//        private string _actualResult;
//        private const int TlPathwayId = 1;

//        public override void Given()
//        {
//            TlLookup = TlLookupDataProvider.CreateFullTlLookupList(DbContext);

//            var seedData = new List<Tuple<int, int, int, int, int?>>
//            {
//                // 2020
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.DistinctionPlus, 2020),
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Distinction, 2020),
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Distinction, 2020),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Distinction, 2020),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Distinction, 2020),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Distinction, 2020),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass, 2020),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Merit, 2020),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Pass, 2020),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass, 2020),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Pass, 2020),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Pass, 2020),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass, 2020),

//                // 2021 onwards
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.DistinctionPlus),
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.APlus, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.A, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.B, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Distinction),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.C, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.D, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Distinction, OverallResultLookup.Merit),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Merit, OverallResultLookup.Pass),
//                NewSeedData(PathwayComponentGradeLookup.E, SpecialismComponentGradeLookup.Pass, OverallResultLookup.Pass)
//            };

//            OverallGradeLookup = new OverallGradeLookupBuilder().BuildList(TlLookup, seedData);
//            DbContext.AddRange(OverallGradeLookup);

//            DbContext.SaveChanges();

//            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
//            {
//                OverallResultBatchSettings = new OverallResultBatchSettings
//                {
//                    BatchSize = 10,
//                    NoOfAcademicYearsToProcess = 4
//                }
//            };

//            CreateService();
//        }

//        public override Task When()
//        {
//            return Task.CompletedTask;
//        }

//        public async Task WhenAsync(int pathwayId, int? pathwayGradeId, int? specialismGradeId, IndustryPlacementStatus ipStatus, int academicYear)
//        {
//            await Task.CompletedTask;
//            _actualResult = await OverallResultCalculationService.GetOverAllGrade(TlLookup.ToList(), pathwayId, pathwayGradeId, specialismGradeId, ipStatus, academicYear);
//        }

//        [Theory]
//        [MemberData(nameof(Data))]
//        public async Task Then_Expected_Results_Are_Returned(int tlPathwayId, string pathwayGrade, string specialismGrade, string overallGrade, IndustryPlacementStatus ipStatus, int academicYear)
//        {
//            var pathwayGradeId = TlLookup.FirstOrDefault(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(pathwayGrade, StringComparison.InvariantCultureIgnoreCase))?.Id;
//            var specialismGradeId = TlLookup.FirstOrDefault(x => x.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(specialismGrade, StringComparison.InvariantCultureIgnoreCase))?.Id;

//            await WhenAsync(tlPathwayId, pathwayGradeId, specialismGradeId, ipStatus, academicYear);

//            _actualResult.Should().Be(overallGrade);
//        }

//        [Theory]
//        [InlineData(2020, typeof(OverallGradeStrategy2020))]
//        [InlineData(2021, typeof(OverallGradeStrategy2021Onwards))]
//        [InlineData(2022, typeof(OverallGradeStrategy2021Onwards))]
//        [InlineData(2023, typeof(OverallGradeStrategy2021Onwards))]
//        [InlineData(2050, typeof(OverallGradeStrategy2021Onwards))]
//        public async Task Then_Correct_OverallGradeStrategy_Is_Returned(int academicYear, Type expectedStrategy)
//        {
//            IOverallGradeStrategy strategy = await OverallGradeStrategyFactory.GetOverallGradeStrategy(academicYear, TlLookup);
//            strategy.Should().BeOfType(expectedStrategy);
//        }

//        [Theory]
//        [InlineData(2018)]
//        [InlineData(2019)]
//        [InlineData(1998)]
//        public async Task If_Year_Less_Than_2020_Then_ArgumentException_Is_Thrown(int academicYear)
//        {
//            Func<Task<IOverallGradeStrategy>> getOverallGradeStrategy = () => OverallGradeStrategyFactory.GetOverallGradeStrategy(academicYear, TlLookup);

//            await getOverallGradeStrategy.Should()
//                .ThrowExactlyAsync<ArgumentException>()
//                .WithParameterName(nameof(academicYear));
//        }

//        [Fact]
//        public async Task If_TlLookup_Collection_Null_Then_ArgumentNullException_Is_Thrown()
//        {
//            Func<Task<IOverallGradeStrategy>> getOverallGradeStrategy = () => OverallGradeStrategyFactory.GetOverallGradeStrategy(2020, null);

//            await getOverallGradeStrategy.Should()
//                .ThrowExactlyAsync<ArgumentNullException>()
//                .WithParameterName("tlLookup");
//        }

//        [Fact]
//        public async Task If_TlLookup_Collection_Empty_Then_ArgumentException_Is_Thrown()
//        {
//            Func<Task<IOverallGradeStrategy>> getOverallGradeStrategy = () => OverallGradeStrategyFactory.GetOverallGradeStrategy(2020, Enumerable.Empty<TlLookup>());

//            await getOverallGradeStrategy.Should()
//                .ThrowExactlyAsync<ArgumentException>()
//                .WithParameterName("tlLookup");
//        }

//        public static IEnumerable<object[]> Data
//        {
//            get
//            {
//                return Data_2020.Concat(Data_2021);
//            }
//        }

//        private static IEnumerable<object[]> Data_2020
//        {
//            get
//            {
//                return new[]
//                {
//                    // IndustryPlacement Completed
//                    new object[] { TlPathwayId, "A*", "Distinction", "Distinction*", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A", "Distinction", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A", "Merit", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A", "Pass", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "B", "Distinction", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "B", "Merit", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "B", "Pass", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "C", "Distinction", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "C", "Merit", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "C", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "D", "Distinction", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "D", "Merit", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "D", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "E", "Distinction", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "E", "Merit", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "E", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },

//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },

//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, null, "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, null, null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },

//                    new object[] { TlPathwayId, "X - no result", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, null, "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2020 },

//                    // IndustryPlacement Not Completed
//                    new object[] { TlPathwayId, "A*", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "B", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "B", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "B", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "C", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "C", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "C", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "D", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "D", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "D", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "E", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "E", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "E", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2020 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.NotSpecified , 2020 },

//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2020 },

//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", null, "Unclassified", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, null, "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, null, null, "X - no result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.NotCompleted , 2020 },

//                    new object[] { TlPathwayId, "X - no result", null, "X - no result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, null, "X - no result", "X - no result", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "Unclassified", IndustryPlacementStatus.NotCompleted , 2020 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted , 2020 }
//                };
//            }
//        }

//        private static IEnumerable<object[]> Data_2021
//        {
//            get
//            {
//                return new[]
//                {
//                    #region Industry Placement Completed

//                    new object[] { TlPathwayId, "A*", "Distinction", "Distinction*", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "A", "Distinction", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", "Merit", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", "Pass", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "B", "Distinction", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", "Merit", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", "Pass", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "C", "Distinction", "Distinction", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", "Merit", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", "Pass", "Pass", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "D", "Distinction", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", "Merit", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", "Pass", "Pass", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "E", "Distinction", "Merit", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", "Merit", "Pass", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", "Pass", "Pass", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "Unclassified", "Distinction", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Merit", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Pass", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, "Merit", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, "Pass", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "X - no result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    new object[] { TlPathwayId, "X - no result", "Distinction", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Merit", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Pass", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", null, "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "X - no result", "Partial achievement", IndustryPlacementStatus.Completed , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.Completed , 2021 },

//                    #endregion

//                    #region IndustryPlacement CompletedWithSpecialConsideration

//                    new object[] { TlPathwayId, "A*", "Distinction", "Distinction*", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "A", "Distinction", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", "Merit", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", "Pass", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "B", "Distinction", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", "Merit", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", "Pass", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "C", "Distinction", "Distinction", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", "Merit", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "D", "Distinction", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", "Merit", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "E", "Distinction", "Merit", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", "Merit", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", "Pass", "Pass", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "Unclassified", "Distinction", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Merit", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Pass", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, "Merit", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, "Pass", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "X - no result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    new object[] { TlPathwayId, "X - no result", "Distinction", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Merit", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Pass", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", null, "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "X - no result", "Partial achievement", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.CompletedWithSpecialConsideration , 2021 },

//                    #endregion

//                    #region IndustryPlacement NotCompleted

//                    new object[] { TlPathwayId, "A*", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A*", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A*", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "A", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "A", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "A", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "B", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "B", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "B", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "B", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "B", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "B", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "C", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "C", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "C", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "C", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "C", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "C", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "D", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "D", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "D", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "D", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "D", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "D", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "E", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "E", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted , 2021 },
//                    new object[] { TlPathwayId, "E", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "E", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "E", null, "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "E", "X - no result", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "Unclassified", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Unclassified", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", null, "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, "Unclassified", "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, null, "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, "X - no result", "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "X - no result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    new object[] { TlPathwayId, "X - no result", "Distinction", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Merit", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Pass", "Partial achievement", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", null, "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "X - no result", "X - no result", IndustryPlacementStatus.NotCompleted, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotCompleted, 2021 },

//                    #endregion

//                    #region IndustryPlacement WillNotComplete

//                    new object[] { TlPathwayId, "A*", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "A", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "A", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "A", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "B", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "B", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "B", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "B", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "B", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "B", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "C", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "C", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "C", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "C", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "C", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "C", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "D", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "D", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "D", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "D", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "D", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "D", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "E", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "E", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete , 2021 },
//                    new object[] { TlPathwayId, "E", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "E", "Unclassified", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "E", null, "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "E", "X - no result", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "Unclassified", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Unclassified", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", null, "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, "Unclassified", "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, null, "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, "X - no result", "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "X - no result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    new object[] { TlPathwayId, "X - no result", "Distinction", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Merit", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Pass", "Partial achievement", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", null, "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "X - no result", "X - no result", IndustryPlacementStatus.WillNotComplete, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.WillNotComplete, 2021 },

//                    #endregion

//                    #region IndustryPlacement NotSpecified

//                    new object[] { TlPathwayId, "A*", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "A*", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A*", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A*", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A*", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A*", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A*", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "A", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "A", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "A", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "A", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "B", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "B", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "B", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "B", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "B", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "B", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "B", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "C", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "C", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "C", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "C", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "C", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "C", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "C", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "D", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "D", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "D", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "D", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "D", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "D", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "D", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "E", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "E", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified , 2021 },
//                    new object[] { TlPathwayId, "E", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "E", "Unclassified", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "E", null, "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "E", "X - no result", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "E", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "Unclassified", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Unclassified", "Unclassified", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", null, "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "X - no result", "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Unclassified", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, null, "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, "Unclassified", "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, null, "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, "X - no result", "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, null, "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "Q - pending result", "Distinction", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Merit", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Pass", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Unclassified", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", null, "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "X - no result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "Q - pending result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 },

//                    new object[] { TlPathwayId, "X - no result", "Distinction", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Merit", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Pass", "Partial achievement", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Unclassified", "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", null, "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "X - no result", "X - no result", IndustryPlacementStatus.NotSpecified, 2021 },
//                    new object[] { TlPathwayId, "X - no result", "Q - pending result", "Q - pending result", IndustryPlacementStatus.NotSpecified, 2021 }

//                    #endregion
//                };
//            }
//        }

//        private Tuple<int, int, int, int, int?> NewSeedData(PathwayComponentGradeLookup pathwayComponentGrade, SpecialismComponentGradeLookup specialismComponentGrade, OverallResultLookup overallResult, int? academicYear = null)
//        {
//            return new Tuple<int, int, int, int, int?>(TlPathwayId, (int)pathwayComponentGrade, (int)specialismComponentGrade, (int)overallResult, academicYear);
//        }
//    }
//}
