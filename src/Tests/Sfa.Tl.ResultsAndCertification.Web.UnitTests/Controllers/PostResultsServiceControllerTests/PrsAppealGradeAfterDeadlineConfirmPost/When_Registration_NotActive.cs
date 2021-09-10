using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public class When_Registration_NotActive : TestSetup
    {
        private AppealGradeAfterDeadlineConfirmViewModel _mockAppealGradeAfterDeadlineRequestViewModel;

        public override void Given()
        {
            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3,
                IsThisGradeBeingAppealed = true
            };

            _mockAppealGradeAfterDeadlineRequestViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ViewModel.ProfileId,
                PathwayAssessmentId = ViewModel.PathwayAssessmentId,
                PathwayResultId = 10,
                Status = RegistrationPathwayStatus.Withdrawn,
                PathwayPrsStatus = PrsStatus.Final
            };

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId).Returns(_mockAppealGradeAfterDeadlineRequestViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
