using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_CompletedWithSpecialConsideration_And_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            ViewModel = CreateViewModel(IndustryPlacementStatus.CompletedWithSpecialConsideration);
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(null as AdminChangeIpViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminChangeIpViewModel>(p => p.AdminIpCompletion == ViewModel));
        }

        [Fact]
        public void Then_Redirected_To_AdminIndustryPlacementSpecialConsiderationHours()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.AdminIndustryPlacementSpecialConsiderationHours);
        }
    }
}