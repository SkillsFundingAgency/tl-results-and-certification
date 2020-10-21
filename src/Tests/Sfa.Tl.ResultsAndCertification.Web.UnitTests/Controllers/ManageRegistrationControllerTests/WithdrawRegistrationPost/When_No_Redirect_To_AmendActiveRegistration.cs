using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_No_Redirect_To_AmendActiveRegistration : TestSetup
    {
        public override void Given()
        {
            ViewModel.CanWithdraw = false;
            ViewModel.ProfileId = ProfileId;
            ViewModel.WithdrawBackLinkOptionId = (int)WithdrawBackLinkOptions.AmendActiveRegistrationPage;
        }

        [Fact]
        public void Then_Redirected_To_AmendActiveRegistration()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.AmendActiveRegistration);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId.ToString());
            route.RouteValues[Constants.ChangeStatusId].Should().Be(((int)RegistrationChangeStatus.Withdrawn).ToString());
        }
    }
}
