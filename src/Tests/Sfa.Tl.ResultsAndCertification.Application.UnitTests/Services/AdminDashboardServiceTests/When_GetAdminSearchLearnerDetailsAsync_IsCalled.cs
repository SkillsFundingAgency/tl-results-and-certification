using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetAdminSearchLearnerDetailsAsync_IsCalled : AdminDashboardServiceBaseTest
    {
        private PagedResponse<AdminSearchLearnerDetail> _expectedResult;
        private PagedResponse<AdminSearchLearnerDetail> _actualResult;

        public override void Given()
        {
            _expectedResult = new PagedResponse<AdminSearchLearnerDetail>
            {
                TotalRecords = 150,
                Records = new List<AdminSearchLearnerDetail>
                           {
                               new AdminSearchLearnerDetail
                               {
                                   Uln = 1234567890,
                                   Firstname = "Jessica",
                                   Lastname = "Johnson",
                                   Provider = "Barnsley College",
                                   ProviderUkprn = 10000536,
                                   AwardingOrganisation = EnumAwardingOrganisation.Pearson.ToString(),
                                   AcademicYear = 2021
                               }
                           },
                PagerInfo = new Pager(1, 1, 10)
            };

            AdminDashboardRepository.SearchLearnerDetailsAsync(Arg.Any<AdminSearchLearnerRequest>()).Returns(_expectedResult);
        }

        public override async Task When()
        {
            var request = new AdminSearchLearnerRequest { SearchKey = "Johnson" };
            _actualResult = await AdminDashboardService.GetAdminSearchLearnerDetailsAsync(request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}