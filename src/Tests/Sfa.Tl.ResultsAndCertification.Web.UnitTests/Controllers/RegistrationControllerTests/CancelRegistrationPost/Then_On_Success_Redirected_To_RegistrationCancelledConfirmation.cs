using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class Then_On_Success_Redirected_To_RegistrationCancelledConfirmation : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            Uln = 1234567890;
            ViewModel.CancelRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(true);
            CacheService.SetAsync(CacheKey, new RegistrationCancelledConfirmationViewModel { Uln = Uln });
        }

        [Fact]
        public void Then_Redirected_To_Route_As_Expected()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationCancelledConfirmation);
        }
    }
}
