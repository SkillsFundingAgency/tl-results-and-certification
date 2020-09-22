using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendWithdrawRegistrationGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;
        private RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdraw;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, Status = _registrationPathwayStatus };
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AmendWithdrawRegistrationViewModel));

            var model = viewResult.Model as AmendWithdrawRegistrationViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.ChangeStatus.Should().BeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
