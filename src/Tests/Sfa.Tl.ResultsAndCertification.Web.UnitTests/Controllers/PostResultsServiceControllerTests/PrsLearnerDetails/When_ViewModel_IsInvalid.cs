using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_ViewModel_IsInvalid : TestSetup
    {
        private PrsLearnerDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;

            _mockLearnerDetails = new PrsLearnerDetailsViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                Status = RegistrationPathwayStatus.Withdrawn
            };

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
