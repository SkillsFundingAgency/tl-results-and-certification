using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_No_Redirect_To_ChangeCore : TestSetup
    {
        public override void Given()
        {
            ViewModel.CanWithdraw = false;
            ViewModel.ProfileId = ProfileId;
            ViewModel.WithdrawBackLinkOptionId = (int)WithdrawBackLinkOptions.ChangeCorePage;
        }

        [Fact]
        public void Then_Redirected_To_ChangeRegistrationCore()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationCore);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId.ToString());
        }
    }
}
