using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ChangeStatus_Withdraw_IsValid : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult;
        public override void Given()
        {
            ViewModel.ChangeStatus = RegistrationChangeStatus.Withdrawn;
            ViewModel.ProfileId = ProfileId;

            _mockresult = new AssessmentDetailsViewModel { ProfileId = ProfileId, HasAnyOutstandingPathwayPrsActivities = false };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Redirected_To_WithdrawRegistration()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.WithdrawRegistration);
            route.RouteValues["withdrawBackLinkOptionId"].Should().Be((int)WithdrawBackLinkOptions.AmendActiveRegistrationPage);
        }
    }
}
