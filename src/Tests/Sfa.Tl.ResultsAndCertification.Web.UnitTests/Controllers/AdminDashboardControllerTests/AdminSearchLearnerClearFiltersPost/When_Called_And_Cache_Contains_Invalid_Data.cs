using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminSearchLearnerClearFiltersPost
{
    public class When_Called_And_Cache_Contains_Invalid_Data : AdminDashboardControllerTestBase
    {
        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey).Returns(null as AdminSearchLearnerViewModel);
        }

        public override async Task When()
        {
            _result = await Controller.AdminSearchLearnerClearFiltersAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminSearchLearnerViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            _result.ShouldBeRedirectPageNotFound();
        }
    }
}