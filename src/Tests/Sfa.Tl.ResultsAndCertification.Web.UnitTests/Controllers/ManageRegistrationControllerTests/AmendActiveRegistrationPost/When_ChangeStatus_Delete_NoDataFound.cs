using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ChangeStatus_Delete_NoDataFound : TestSetup
    {
        private AssessmentDetailsViewModel mockresult;

        public override void Given()
        {
            ViewModel.ChangeStatus = RegistrationChangeStatus.Delete;
            ViewModel.ProfileId = ProfileId;

            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
