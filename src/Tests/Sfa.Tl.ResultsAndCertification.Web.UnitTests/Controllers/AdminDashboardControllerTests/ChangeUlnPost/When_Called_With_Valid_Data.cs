using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnPost
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(3).Returns(Entry());
            AdminDashboardLoader.IsLearnerUlnAvailableAsync(9999999991).Returns(true);
            Model = Entry();
            Model.Uln = 1111111111;
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            result.RouteName.Should().Be(RouteConstants.AdminReviewChangeUln);
            await CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AdminChangeUlnViewModel>(m => m.UlnTo == "9999999991" && m.Uln == 9999999990), Arg.Any<CacheExpiryTime>());
        }
    }
}
