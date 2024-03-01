using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey).Returns(null as AdminChangeSpecialismResultViewModel);
            AdminDashboardLoader.GetAdminChangeSpecialismResultAsync(RegistrationPathwayId, AssessmentId).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            AdminDashboardLoader.Received(1).GetAdminChangeSpecialismResultAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminChangeSpecialismResultViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}