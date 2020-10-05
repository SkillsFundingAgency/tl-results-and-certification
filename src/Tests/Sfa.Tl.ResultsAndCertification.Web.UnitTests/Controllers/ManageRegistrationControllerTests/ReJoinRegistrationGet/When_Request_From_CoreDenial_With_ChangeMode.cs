using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.RejoinRegistrationGet
{
    public class When_Request_From_CoreDenial_With_ChangeMode : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            IsFromCoreDenialPage = true;
            IsChangeMode = true;
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
            viewResult.Model.Should().BeOfType(typeof(RejoinRegistrationViewModel));

            var model = viewResult.Model as RejoinRegistrationViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.CanRejoin.Should().BeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ReregisterCannotSelectSameCore);
            backLink.RouteAttributes.Count.Should().Be(2);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
            backLink.RouteAttributes.TryGetValue(Constants.IsChangeMode, out string routeValueChange);
            routeValueChange.Should().Be(IsChangeMode.ToString().ToLowerInvariant());
        }
    }
}
