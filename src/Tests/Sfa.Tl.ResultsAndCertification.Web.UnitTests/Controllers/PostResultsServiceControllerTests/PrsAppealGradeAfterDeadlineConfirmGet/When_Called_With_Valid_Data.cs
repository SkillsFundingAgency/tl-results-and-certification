using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AppealGradeAfterDeadlineConfirmViewModel _appealGradeAfterDeadlineViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;

            _appealGradeAfterDeadlineViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayDisplayName = "Healthcare (12345678)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                AppealEndDate = DateTime.UtcNow.AddDays(-3),
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
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealGradeAfterDeadlineConfirmViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealGradeAfterDeadlineViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayAssessmentId);
            model.AppealEndDate.Should().Be(_appealGradeAfterDeadlineViewModel.AppealEndDate);
            model.PathwayPrsStatus.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayPrsStatus);
            model.Status.Should().Be(_appealGradeAfterDeadlineViewModel.Status);
            model.Uln.Should().Be(_appealGradeAfterDeadlineViewModel.Uln);
            model.LearnerName.Should().Be(_appealGradeAfterDeadlineViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealGradeAfterDeadlineViewModel.DateofBirth);
            model.PathwayDisplayName.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayDisplayName);
            model.PathwayGrade.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayGrade);
            model.PathwayAssessmentSeries.Should().Be(_appealGradeAfterDeadlineViewModel.PathwayAssessmentSeries);
            model.IsThisGradeBeingAppealed.Should().BeNull();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealGradeAfterDeadline);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
        }
    }
}
