using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistration
{
    public class Then_Redirected_To_AddRegistrationUln_Route : When_AddRegistration_Action_Is_Called
    {
        public override void Given()
        {
        }

        [Fact]
        public void Then_Redirected_To_Route_AddRegistrationUln()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationUln);
        }
    }
}
