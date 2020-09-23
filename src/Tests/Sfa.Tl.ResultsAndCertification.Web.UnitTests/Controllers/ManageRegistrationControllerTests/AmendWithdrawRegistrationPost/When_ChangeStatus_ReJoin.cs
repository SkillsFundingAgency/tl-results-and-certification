using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendWithdrawRegistrationPost
{
    public class When_ChangeStatus_ReJoin : TestSetup
    {
        public override void Given()
        {
            ViewModel.ProfileId = ProfileId;
            ViewModel.ChangeStatus = RegistrationChangeStatus.Rejoin;
        }

        [Fact]
        public void Then_Redirected_To_WithdrawRegistration()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReJoinRegistration);
            route.RouteValues[Constants.ProfileId].Should().Be(ProfileId);
        }
    }
}
