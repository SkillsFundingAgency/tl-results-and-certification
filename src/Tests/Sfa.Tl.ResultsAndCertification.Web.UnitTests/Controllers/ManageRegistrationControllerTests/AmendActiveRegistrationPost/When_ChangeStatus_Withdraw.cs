using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ChangeStatus_Withdraw : TestSetup
    {
        public override void Given()
        {
            ViewModel.ChangeStatus = RegistrationChangeStatus.Withdraw;
        }

        [Fact]
        public void Then_Redirected_To_WithdrawRegistration()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.WithdrawRegistration);
            route.RouteValues["withdrawBackLinkOptionId"].Should().Be((int)WithdrawBackLinkOptions.AmendActiveRegistrationPage);
        }
    }
}
