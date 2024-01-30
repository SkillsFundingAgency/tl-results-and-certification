using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(null as AdminChangeIpViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminChangeIpViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}
