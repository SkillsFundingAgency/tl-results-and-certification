using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public class When_IsThisGradeBeingAppealed_IsFalse : TestSetup
    {
        private AppealGradeAfterDeadlineConfirmViewModel _mockAppealGradeAfterDeadlineRequestViewModel;

        public override void Given()
        {
            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3,
                IsThisGradeBeingAppealed = false
            };

            _mockAppealGradeAfterDeadlineRequestViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ViewModel.ProfileId,
                PathwayAssessmentId = ViewModel.PathwayAssessmentId,
                PathwayResultId = 10,
                Status = RegistrationPathwayStatus.Active
            };

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId).Returns(_mockAppealGradeAfterDeadlineRequestViewModel);
        }
       
        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.PathwayAssessmentId);
        }
    }
}
