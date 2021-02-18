using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ChangeStatus_Delete_IsValid : TestSetup
    {
        private AssessmentDetailsViewModel mockresult;

        public override void Given()
        {
            ViewModel.ChangeStatus = RegistrationChangeStatus.Delete;
            ViewModel.ProfileId = ProfileId;

            mockresult = new AssessmentDetailsViewModel { ProfileId = ProfileId, IsResultExist = false };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_DeleteRegistration()
        {
            var route = (Result as RedirectToRouteResult);
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.DeleteRegistration);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
