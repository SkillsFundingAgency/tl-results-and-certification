using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_Withdraw_Success : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;
        private WithdrawRegistrationResponse _mockResponse = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;

        public override void Given()
        {
            _mockresult = new AssessmentDetailsViewModel { Uln = 1234567890, ProfileId = ProfileId, PathwayStatus = _registrationPathwayStatus };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_mockresult);


            ViewModel.CanWithdraw = true;
            ViewModel.ProfileId = ProfileId;
            _mockResponse = new WithdrawRegistrationResponse
            {
                ProfileId = 1,
                Uln = 7654332198,
                IsSuccess = true,
                IsRequestFromProviderAndCorePage = false
            };
            RegistrationLoader.WithdrawRegistrationAsync(AoUkprn, ViewModel).Returns(_mockResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
            RegistrationLoader.Received(1).WithdrawRegistrationAsync(AoUkprn, ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_WithdrawRegistrationConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.WithdrawRegistrationConfirmation);
        }
    }
}
