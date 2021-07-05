using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealOutcomePathwayGradePost
{
    public class When_AppealOutcome_Is_UpdateGrade : TestSetup
    {
        private AppealOutcomePathwayGradeViewModel _appealOutcomePathwayGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealOutcomePathwayGradeViewModel = new AppealOutcomePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = ResultId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayName = "Childcare",
                PathwayCode = "12121212",
                PathwayDisplayName = "Childcare<br/>(12121212)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                PathwayPrsStatus = PrsStatus.BeingAppealed
            };

            Loader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealOutcomePathwayGradeViewModel);

            ViewModel = new AppealOutcomePathwayGradeViewModel { ProfileId = 1, PathwayAssessmentId = AssessmentId, PathwayResultId = ResultId, AppealOutcome = AppealOutcomeType.UpdateGrade };
        }

        [Fact]
        public void Then_Redirected_To_PrsAppealUpdatePathwayGrade()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAppealUpdatePathwayGrade);
            route.RouteValues.Count.Should().Be(3);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.PathwayAssessmentId);
            route.RouteValues[Constants.ResultId].Should().Be(ViewModel.PathwayResultId);
        }
    }
}
