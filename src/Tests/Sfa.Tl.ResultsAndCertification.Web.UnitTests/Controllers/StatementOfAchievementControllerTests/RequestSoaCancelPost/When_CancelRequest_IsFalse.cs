using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelPost
{
    public class When_CancelRequest_IsFalse : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RequestSoaCancelViewModel { ProfileId = 1, CancelRequest = false };
        }

        [Fact]
        public void Then_Redirected_To_RequestSoaCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RequestSoaCheckAndSubmit);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
