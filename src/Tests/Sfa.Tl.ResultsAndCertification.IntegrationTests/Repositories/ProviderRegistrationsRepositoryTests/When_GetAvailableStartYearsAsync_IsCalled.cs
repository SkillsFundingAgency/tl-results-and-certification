using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AcademicYear = Sfa.Tl.ResultsAndCertification.Domain.Models.AcademicYear;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.ProviderRegistrationsRepositoryTests
{
    public class When_GetAvailableStartYearsAsync_IsCalled : BaseTest<AcademicYear>
    {
        private readonly ProviderRegistrationsRepository _repository;

        private IList<int> _actualResult;

        public When_GetAvailableStartYearsAsync_IsCalled()
        {
            _repository = new ProviderRegistrationsRepository(DbContext);
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
            _actualResult = await _repository.GetAvailableStartYearsAsync(() => searchDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(DateTime searchDate, IList<int> expectedResult)
        {
            await WhenAsync(searchDate);

            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(expectedResult.Count);
            _actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static readonly Dictionary<DateTime, List<int>> _data = new()
        {
            ["05/02/1900".ToDateTime()] = new List<int>(),
            ["05/02/2021".ToDateTime()] = new List<int> { 2020 },
            ["05/02/2022".ToDateTime()] = new List<int> { 2021, 2020 },
            ["05/02/2023".ToDateTime()] = new List<int> { 2022, 2021, 2020 },
            ["05/02/2024".ToDateTime()] = new List<int> { 2023, 2022, 2021, 2020 },
            ["10/09/2024".ToDateTime()] = new List<int> { 2024, 2023, 2022, 2021, 2020 },
            ["11/09/2025".ToDateTime()] = new List<int> { 2025, 2024, 2023, 2022, 2021 }
        };

        public static IEnumerable<object[]> Data
            => _data.Select(p => new object[] { p.Key, p.Value });

        private void SeedAcademicYears()
        {
            AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}