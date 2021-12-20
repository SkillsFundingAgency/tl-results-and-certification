using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawRegistrationPost
{
    public class When_Called_With_OutstandignPathwayPrsActivities : TestSetup
    {
        private RegistrationAssessmentDetails _mockresult = null;
        private WithdrawRegistrationResponse _mockResponse = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;

        public override void Given()
        {
            ViewModel.CanWithdraw = true;
            ViewModel.ProfileId = ProfileId;
            _mockResponse = new WithdrawRegistrationResponse
            {
                ProfileId = 1,
                Uln = 7654332198,
                IsSuccess = true,
                IsRequestFromProviderAndCorePage = false
            };

            _mockresult = new RegistrationAssessmentDetails { Uln = 1234567890, ProfileId = ProfileId, PathwayStatus = _registrationPathwayStatus, HasAnyOutstandingPathwayPrsActivities = true };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_mockresult);
            RegistrationLoader.WithdrawRegistrationAsync(AoUkprn, ViewModel).Returns(_mockResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
