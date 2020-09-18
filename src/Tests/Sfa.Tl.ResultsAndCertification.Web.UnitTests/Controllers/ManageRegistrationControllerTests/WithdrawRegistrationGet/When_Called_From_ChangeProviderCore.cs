using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationGet
{
    public class When_Called_From_ChangeProviderCore : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;

        public override void Given()
        {
            WithdrawBackLinkOption = WithdrawBackLinkOptions.CannotChangeProviderAndCorePage;
            mockresult = new RegistrationDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, Status = RegistrationPathwayStatus.Active };
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
            viewResult.Model.Should().BeOfType(typeof(WithdrawRegistrationViewModel));

            var model = viewResult.Model as WithdrawRegistrationViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.CanWithdraw.Should().BeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw);
            backLink.RouteAttributes.Should().BeNull();
        }
    }
}
