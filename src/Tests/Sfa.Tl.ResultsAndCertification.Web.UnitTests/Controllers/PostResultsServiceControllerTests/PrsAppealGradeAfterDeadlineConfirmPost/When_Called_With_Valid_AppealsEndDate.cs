using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public class When_Called_With_Valid_AppealsEndDate : TestSetup
    {
        private AppealGradeAfterDeadlineConfirmViewModel _appealGradeAfterDeadlineViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;

            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = 3
            };

            _appealGradeAfterDeadlineViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                AppealEndDate = DateTime.UtcNow.AddDays(3),
                PathwayPrsStatus = null,
                Status = RegistrationPathwayStatus.Active
            };

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealGradeAfterDeadlineViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
