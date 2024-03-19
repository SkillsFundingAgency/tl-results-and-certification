using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangePathwayResultReviewChangesGet
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            AdminChangePathwayResultViewModel nullModel = null;

            CacheService.GetAsync<AdminChangePathwayResultViewModel>(CacheKey).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangePathwayResultViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().CreateAdminChangePathwayResultReviewChanges(Arg.Any<AdminChangePathwayResultViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}