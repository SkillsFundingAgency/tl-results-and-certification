using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class Then_On_Failed_CancelRegistration_Redirected_To_Error : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            ViewModel.CancelRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(false);
        }

        [Fact]
        public void Then_Redirected_To_Route_As_Expected()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.Error);
        }
    }
}
