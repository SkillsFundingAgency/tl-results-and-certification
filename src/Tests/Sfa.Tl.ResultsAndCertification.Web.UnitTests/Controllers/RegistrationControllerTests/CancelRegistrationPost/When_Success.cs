using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            Uln = 1234567890;
            ViewModel.CancelRegistration = true;
            RegistrationLoader.DeleteRegistrationAsync(AoUkprn, ProfileId).Returns(true);
            CacheService.SetAsync(CacheKey, new RegistrationCancelledConfirmationViewModel { Uln = Uln });
        }

        [Fact]
        public void Then_Redirected_To_RegistrationCancelledConfirmation()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.RegistrationCancelledConfirmation);
        }
    }
}
