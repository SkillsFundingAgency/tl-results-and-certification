using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelPost
{
    public class When_CancelRequest_IsTrue : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RequestSoaCancelViewModel { ProfileId = 1, CancelRequest = true };
        }

        [Fact]
        public void Then_Redirected_To_Home()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.Home);
            route.RouteValues.Should().BeNull();
        }
    }
}
