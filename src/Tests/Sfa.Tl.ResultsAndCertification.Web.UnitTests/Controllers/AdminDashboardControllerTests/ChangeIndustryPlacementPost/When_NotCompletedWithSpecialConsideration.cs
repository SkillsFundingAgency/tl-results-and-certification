using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public abstract class When_NotCompletedWithSpecialConsideration : TestSetup
    {
        private readonly IndustryPlacementStatus _industryPlacementStatus;

        public When_NotCompletedWithSpecialConsideration(IndustryPlacementStatus industryPlacementStatus)
        {
            _industryPlacementStatus = industryPlacementStatus;
        }

        public override void Given()
        {
            ViewModel = CreateViewModel(_industryPlacementStatus);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminChangeIpViewModel>(p => p.AdminIpCompletion == ViewModel));
        }

        [Fact]
        public void Then_Redirected_To_AdminLearnerRecord()
        {
            Result.ShouldBeRedirectToActionResult(RouteConstants.AdminLearnerRecord, (Constants.PathwayId, ViewModel.RegistrationPathwayId));
        }
    }
}