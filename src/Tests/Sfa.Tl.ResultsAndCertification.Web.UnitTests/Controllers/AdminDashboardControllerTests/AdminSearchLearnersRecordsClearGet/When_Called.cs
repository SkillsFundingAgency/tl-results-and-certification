using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnersRecordsClearGet
{
    public class When_Called : AdminDashboardControllerTestBase
    {
        private IActionResult _result;

        public override void Given()
        {
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnersRecordsClearAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminSearchLearnerViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_AdminSearchLearnersRecords()
        {
            _result.Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
        }
    }
}