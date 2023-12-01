using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AcademicYear = Sfa.Tl.ResultsAndCertification.Domain.Models.AcademicYear;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminDashboardRepositoryTests
{
    public class When_GetAcademicYearFiltersAsync_IsCalled : BaseTest<AcademicYear>
    {
        private readonly AdminDashboardRepository _repository;

        private IList<FilterLookupData> _actualResult;

        public When_GetAcademicYearFiltersAsync_IsCalled()
        {
            _repository = new AdminDashboardRepository(DbContext);
        }

        public override void Given()
        {
            SeedAcademicYears();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(DateTime searchDate)
        {
            _actualResult = await _repository.GetAcademicYearFiltersAsync(searchDate);
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

        private static readonly Dictionary<DateTime, List<FilterLookupData>> _data = new()
        {
            ["05/02/1900".ToDateTime()] = new List<FilterLookupData>(),
            ["05/02/2021".ToDateTime()] = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false }
            },
            ["05/02/2022".ToDateTime()] = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }
            },
            ["05/02/2023".ToDateTime()] = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
                new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
                new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false }
            },
            ["05/02/2024".ToDateTime()] = new List<FilterLookupData>
            {
               new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
               new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
               new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false },
               new FilterLookupData { Id = 2023, Name = "2023 to 2024", IsSelected = false }
            },
            ["10/9/2024".ToDateTime()] = new List<FilterLookupData>
            {
               new FilterLookupData { Id = 2020, Name = "2020 to 2021", IsSelected = false },
               new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
               new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false },
               new FilterLookupData { Id = 2023, Name = "2023 to 2024", IsSelected = false },
               new FilterLookupData { Id = 2024, Name = "2024 to 2025", IsSelected = false }
            },
            ["11/9/2025".ToDateTime()] = new List<FilterLookupData>
            {
               new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false },
               new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false },
               new FilterLookupData { Id = 2023, Name = "2023 to 2024", IsSelected = false },
               new FilterLookupData { Id = 2024, Name = "2024 to 2025", IsSelected = false },
               new FilterLookupData { Id = 2025, Name = "2025 to 2026", IsSelected = false }
            }
        };

        public static IEnumerable<object[]> Data
        {
            get
            {
                return _data.Select(p => new object[] { p.Key, p.Value });
            }
        }

        private void SeedAcademicYears()
        {
            AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}