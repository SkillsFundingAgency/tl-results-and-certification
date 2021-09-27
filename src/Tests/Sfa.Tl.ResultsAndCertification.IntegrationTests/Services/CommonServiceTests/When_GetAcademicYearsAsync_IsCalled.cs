using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CommonServiceTests
{
    public class When_GetAcademicYearsAsync_IsCalled : CommonServiceBaseTest
    {
        private IEnumerable<Models.Contracts.Common.AcademicYear> _result;
        private IEnumerable<Models.Contracts.Common.AcademicYear> _expectedResult;

        public override void Given()
        {
            SeedAcademicYears();
            _expectedResult = AcademicYears.Select(a => new Models.Contracts.Common.AcademicYear { Id = a.Id, Name = a.Name, Year = a.Year });
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

            _result = await CommonRepository.GetAcademicYearsAsync();
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            _result.Should().NotBeNullOrEmpty();
            _result.Count().Should().Be(_expectedResult.Count());
            _result.Should().BeEquivalentTo(_expectedResult);
        }
    }
}
