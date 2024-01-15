using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursPost
{
    public class When_CompletedWithSpecialConsideration : TestSetup
    {
        private readonly AdminChangeIpViewModel _cachedViewModel = new()
        {
            AdminIpCompletion = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            }
        };

        public override void Given()
        {
            ViewModel = new AdminIpSpecialConsiderationHoursViewModel
            {
                RegistrationPathwayId = 1,
                Hours = "15"
            };

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cachedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminChangeIpViewModel>(p => p == _cachedViewModel && p.HoursViewModel == ViewModel));
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons);
        }
    }
}
