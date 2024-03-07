using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddPathwayResultReviewChangesGet
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            AdminAddPathwayResultViewModel nullModel = null;

            CacheService.GetAsync<AdminAddPathwayResultViewModel>(CacheKey).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddPathwayResultViewModel>(CacheKey);
            AdminDashboardLoader.DidNotReceive().CreateAdminAddPathwayResultReviewChanges(Arg.Any<AdminAddPathwayResultViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}