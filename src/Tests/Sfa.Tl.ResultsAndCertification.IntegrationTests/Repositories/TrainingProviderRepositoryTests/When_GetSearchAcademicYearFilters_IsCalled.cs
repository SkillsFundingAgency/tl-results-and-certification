using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public class When_GetSearchAcademicYearFilters_IsCalled : TrainingProviderRepositoryBaseTest
    {
        private IList<FilterLookupData> _actualResult;

        public override void Given()
        {
            SeedAcademicYears();
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TraningProviderRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(DateTime searchDate)
        {
            _actualResult = await TrainingProviderRepository.GetSearchAcademicYearFiltersAsync(searchDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(DateTime searchDate, IList<FilterLookupData> expectedResult)
        {
            await WhenAsync(searchDate);
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(expectedResult.Count);
            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { "05/02/1900".ToDateTime(), new List<FilterLookupData>() },

                    new object[] { "05/02/2021".ToDateTime(),
                        new List<FilterLookupData>
                        {
                            new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                        }
                    },

                    new object[] { "05/02/2022".ToDateTime(),
                        new List<FilterLookupData>
                        {
                            new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                            new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
                        }
                    },

                    new object[] { "05/02/2023".ToDateTime(),
                        new List<FilterLookupData>
                        {
                            new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                            new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
                            new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false }
                        }
                    },

                    new object[] { "05/02/2024".ToDateTime(),
                        new List<FilterLookupData>
                        {
                            new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                            new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
                            new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false },
                            new FilterLookupData { Id = 2023, Name = "2023 to 2024", IsSelected = false }
                        }
                    },
                };
            }
        }

        public void SeedAcademicYears()
        {
            AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}
