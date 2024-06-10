using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.SearchRegistrationServiceTests
{
    public class When_SearchRegistrationDetailsAsync_IsCalled : SearchRegistrationServiceBaseTest
    {
        private readonly SearchRegistrationRequest _request = new() { AoUkprn = 10009696, SearchKey = "Johnson" };

        private PagedResponse<SearchRegistrationDetail> _expectedResult;
        private PagedResponse<SearchRegistrationDetail> _actualResult;

        public override void Given()
        {
            _expectedResult = new PagedResponse<SearchRegistrationDetail>
            {
                TotalRecords = 150,
                Records = new List<SearchRegistrationDetail>
                {
                    new()
                    {
                        Uln = 1234567890,
                        Firstname = "Jessica",
                        Lastname = "Johnson",
                        ProviderName = "Barnsley College",
                        ProviderUkprn = 10000536,
                        AcademicYear = 2021
                    }
                },
                PagerInfo = new Pager(1, 1, 10)
            };

            SearchRegistrationRepository.SearchRegistrationDetailsAsync(Arg.Is(_request)).Returns(_expectedResult);
        }

        public override async Task When()
        {
            _actualResult = await RegistrationService.SearchRegistrationDetailsAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}