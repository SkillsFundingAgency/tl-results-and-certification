using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationGet
{
    public class When_Called_With_ChangeStatus_Withdraw : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;

        public override void Given()
        {
            ChangeStatusId = Common.Enum.RegistrationChangeStatus.Withdraw;
            mockresult = new RegistrationDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, Status = Common.Enum.RegistrationPathwayStatus.Active };
            RegistrationLoader.GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AmendActiveRegistrationViewModel));

            var model = viewResult.Model as AmendActiveRegistrationViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.ChangeStatus.Should().Be(RegistrationChangeStatus.Withdraw);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
