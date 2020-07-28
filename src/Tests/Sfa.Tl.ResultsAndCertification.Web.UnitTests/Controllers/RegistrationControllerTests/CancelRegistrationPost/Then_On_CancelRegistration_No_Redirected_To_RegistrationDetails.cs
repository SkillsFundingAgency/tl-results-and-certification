using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class Then_On_CancelRegistration_No_Redirected_To_RegistrationDetails : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            ViewModel.CancelRegistration = false;
        }

        [Fact]
        public void Then_On_Set_CancelRegistration_To_False_Redirect_To_RegistrationDetails_Route()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.RegistrationDetails);

            route.RouteValues["profileId"].Should().Be(ViewModel.ProfileId);
        }
    }
}
