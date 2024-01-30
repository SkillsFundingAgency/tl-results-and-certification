using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public class When_Cache_Not_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            AssertViewResult();
        }
    }
}