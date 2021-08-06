using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.AmendActiveRegistrationPost
{
    public class When_ChangeStatus_Withdraw_HasOutstandingPrsActivities : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult;

        public override void Given()
        {
            ViewModel.ChangeStatus = RegistrationChangeStatus.Withdrawn;
            ViewModel.ProfileId = ProfileId;

            _mockresult = new AssessmentDetailsViewModel { ProfileId = ProfileId, HasAnyOutstandingPathwayPrsActivities = true };
            RegistrationLoader.GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<RegistrationCannotBeWithdrawnViewModel>
                (x => x.ProfileId == ViewModel.ProfileId), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationCannotBeWithdrawn()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RegistrationCannotBeWithdrawn);
        }
    }
}
