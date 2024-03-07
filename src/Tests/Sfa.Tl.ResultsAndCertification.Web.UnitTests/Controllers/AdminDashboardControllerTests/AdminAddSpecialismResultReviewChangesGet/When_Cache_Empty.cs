using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddSpecialismResultReviewChangesGet
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            AdminAddSpecialismResultViewModel nullModel = null;

            CacheService.GetAsync<AdminAddSpecialismResultViewModel>(CacheKey).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().CreateAdminAddSpecialismResultReviewChanges(Arg.Any<AdminAddSpecialismResultViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}