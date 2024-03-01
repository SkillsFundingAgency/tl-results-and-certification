using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminChangeSpecialismResultViewModel nullModel = null;

            CacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey).Returns(nullModel);
            AdminDashboardLoader.GetAdminChangeSpecialismResultAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetAdminChangeSpecialismResultAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}