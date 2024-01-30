using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsGet
{
    public abstract class When_IndustryPlacementStatus_Invalid : TestSetup
    {
        private readonly IndustryPlacementStatus _industryPlacementStatus;

        public When_IndustryPlacementStatus_Invalid(IndustryPlacementStatus industryPlacementStatus)
        {
            _industryPlacementStatus = industryPlacementStatus;
        }

        public override void Given()
        {
            var cachedViewModel = new AdminChangeIpViewModel
            {
                AdminIpCompletion = new AdminIpCompletionViewModel
                {
                    IndustryPlacementStatus = _industryPlacementStatus
                }
            };

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(cachedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}
