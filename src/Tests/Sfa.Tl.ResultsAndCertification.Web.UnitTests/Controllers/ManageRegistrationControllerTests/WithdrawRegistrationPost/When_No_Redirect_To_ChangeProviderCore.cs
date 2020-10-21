using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_No_Redirect_To_ChangeProviderCore : TestSetup
    {
        public override void Given()
        {
            ViewModel.CanWithdraw = false;
            ViewModel.ProfileId = ProfileId;
            ViewModel.WithdrawBackLinkOptionId = (int)WithdrawBackLinkOptions.CannotChangeProviderAndCorePage;
        }

        [Fact]
        public void Then_Redirected_To_ProviderCoreNeedToWithdraw()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw);
            route.RouteValues.Should().BeNull();
        }
    }
}
