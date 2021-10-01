using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.CommonRepositoryTests
{
    public class When_GetCurrentAcademicYearAsync_IsCalled : CommonRepositoryBaseTest
    {
        private IEnumerable<Models.Contracts.Common.AcademicYear> _result;
        private AcademicYear _expectedAcademicYear;

        public override void Given()
        {
            SeedAcademicYears();
            _expectedAcademicYear = AcademicYears.FirstOrDefault(x => DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate);
            CommonRepository = new CommonRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_result != null)
                return;

            _result = await CommonRepository.GetCurrentAcademicYearsAsync();
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            _result.Should().NotBeNullOrEmpty();
            _result.Count().Should().Be(1);

            var actualResult = _result.FirstOrDefault();

            actualResult.Id.Should().Be(_expectedAcademicYear.Id);
            actualResult.Name.Should().Be(_expectedAcademicYear.Name);
            actualResult.Year.Should().Be(_expectedAcademicYear.Year);
        }
    }
}
