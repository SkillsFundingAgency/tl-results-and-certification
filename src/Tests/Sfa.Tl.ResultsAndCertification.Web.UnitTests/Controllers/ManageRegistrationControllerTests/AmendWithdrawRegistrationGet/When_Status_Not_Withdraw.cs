using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendWithdrawRegistrationGet
{
    public class When_Status_Not_Withdraw : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdraw;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, Status = RegistrationPathwayStatus.Active };
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
