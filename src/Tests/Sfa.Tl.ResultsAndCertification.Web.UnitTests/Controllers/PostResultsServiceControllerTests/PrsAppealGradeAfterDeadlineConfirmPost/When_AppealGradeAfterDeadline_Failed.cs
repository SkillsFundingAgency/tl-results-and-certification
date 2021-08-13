using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public class When_AppealGradeAfterDeadline_Failed : TestSetup
    {
        private readonly bool _gradeChangeRequestResponse = false;
        private AppealGradeAfterDeadlineConfirmViewModel _mockAppealGradeAfterDeadlineRequestViewModel;

        public override void Given()
        {
            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3
            };

            _mockAppealGradeAfterDeadlineRequestViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ViewModel.ProfileId,
                PathwayAssessmentId = ViewModel.PathwayAssessmentId,
                PathwayResultId = 10,
                Status = RegistrationPathwayStatus.Active
            };

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId).Returns(_mockAppealGradeAfterDeadlineRequestViewModel);
            Loader.AppealGradeAfterDeadlineRequestAsync(ViewModel).Returns(_gradeChangeRequestResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);
            Loader.Received(1).AppealGradeAfterDeadlineRequestAsync(ViewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<PrsAppealGradeAfterDeadlineRequestConfirmationViewModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
