using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AcademicYear = Sfa.Tl.ResultsAndCertification.Domain.Models.AcademicYear;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AdminDashboardRepositoryTests
{
    public class When_GetAllowedChangeAcademicYears_IsCalled : BaseTest<AcademicYear>
    {
        private readonly AdminDashboardRepository _repository;
        private IList<int> _actualResult;

        public When_GetAllowedChangeAcademicYears_IsCalled()
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

        public async Task WhenAsync(DateTime today, int learnerAcademicYear, int pathwayStartYear)
        {
            _actualResult = await _repository.GetAllowedChangeAcademicYearsAsync(() => today, learnerAcademicYear, pathwayStartYear);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(DateTime today, int learnerAcademicYear, int pathwayStartYear, int[] expectedResult)
        {
            await WhenAsync(today, learnerAcademicYear, pathwayStartYear);

            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { "01/01/2024".ToDateTime(), 2023, 2023, Array.Empty<int>() },
                    new object[] { "01/01/2024".ToDateTime(), 2023, 2022, new int[] { 2022 } },
                    new object[] { "01/01/2024".ToDateTime(), 2023, 2021, new int[] { 2021, 2022 } },
                    new object[] { "01/01/2024".ToDateTime(), 2022, 2021, new int[] { 2021, 2023 } },
                    new object[] { "01/01/2024".ToDateTime(), 2022, 2020, new int[] { 2020, 2021, 2023 } },
                    new object[] { "01/01/2025".ToDateTime(), 2021, 2020, new int[] { 2020, 2022 } },
                    new object[] { "01/01/2025".ToDateTime(), 2022, 2020, new int[] { 2020, 2021, 2023 } },
                    new object[] { "01/01/2026".ToDateTime(), 2024, 2024, new int[] { 2025 } },
                    new object[] { "01/01/2026".ToDateTime(), 2024, 2023, new int[] { 2023, 2025 } },
                    new object[] { "01/01/2026".ToDateTime(), 2024, 2020, new int[] { 2022, 2023, 2025 } }
                };
            }
        }

        private void SeedAcademicYears()
        {
            AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}