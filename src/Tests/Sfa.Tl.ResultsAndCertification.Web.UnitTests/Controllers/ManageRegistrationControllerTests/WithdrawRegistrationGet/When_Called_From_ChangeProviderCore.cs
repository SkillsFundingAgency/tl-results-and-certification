using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationGet
{
    public class When_Called_From_ChangeProviderCore : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;

        public override void Given()
        {
            WithdrawBackLinkOption = WithdrawBackLinkOptions.CannotChangeProviderAndCorePage;
            _mockresult = new AssessmentDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, PathwayStatus = _registrationPathwayStatus };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
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

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.CanWithdraw.Should().BeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw);
            backLink.RouteAttributes.Should().BeNull();
        }
    }
}
