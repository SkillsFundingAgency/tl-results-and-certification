using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            ViewModel.DeleteRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(false);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.Error);
        }
    }
}
