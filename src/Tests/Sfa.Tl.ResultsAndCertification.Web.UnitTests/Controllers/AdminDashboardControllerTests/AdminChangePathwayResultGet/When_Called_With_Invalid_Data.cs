using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangePathwayResultGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminChangePathwayResultViewModel nullModel = null;

            CacheService.GetAsync<AdminChangePathwayResultViewModel>(CacheKey).Returns(nullModel);
            AdminDashboardLoader.GetAdminChangePathwayResultAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangePathwayResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetAdminChangePathwayResultAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}