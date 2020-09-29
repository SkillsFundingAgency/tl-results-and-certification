using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationPost
{
    public class When_Called_With_DeleteRegistration : TestSetup
    {
        public override void Given()
        {
            ViewModel.DeleteRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(true);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationCancelledConfirmation()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationCancelledConfirmation);
        }
    }
}
