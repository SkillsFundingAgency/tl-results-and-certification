using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationPost
{
    public class When_Called_With_No_DeleteRegistration : TestSetup
    {
        public override void Given()
        {
            ViewModel.DeleteRegistration = false;
        }

        [Fact]
        public void Then_Redirected_To_RegistrationDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
