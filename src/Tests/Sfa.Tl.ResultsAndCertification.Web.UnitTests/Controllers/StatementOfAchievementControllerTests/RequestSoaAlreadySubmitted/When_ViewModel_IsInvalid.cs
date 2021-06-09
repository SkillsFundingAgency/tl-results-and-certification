using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaAlreadySubmitted
{
    public class When_ViewModel_IsInvalid : TestSetup
    {
        private RequestSoaSubmittedAlreadyViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            _mockLearnerDetails = new RequestSoaSubmittedAlreadyViewModel
            {
                PathwayStatus = RegistrationPathwayStatus.Active
            };
            StatementOfAchievementLoader.GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
