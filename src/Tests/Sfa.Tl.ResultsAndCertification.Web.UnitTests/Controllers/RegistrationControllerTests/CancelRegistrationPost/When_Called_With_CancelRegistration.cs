using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class When_Called_With_CancelRegistration : TestSetup
    {
        public override void Given()
        {
            ViewModel.CancelRegistration = true;
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
