using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultReviewChangesGet
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            AdminChangeSpecialismResultViewModel nullModel = null;

            CacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().CreateAdminChangeSpecialismResultReviewChanges(Arg.Any<AdminChangeSpecialismResultViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}