using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class Then_On_CancelRegistration_Set_True_Redirected_To_RegistrationCancelledConfirmation : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            ViewModel.CancelRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(true);
        }

        [Fact]
        public void Then_Redirected_To_Route_As_Expected()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationCancelledConfirmation);
        }
    }
}
