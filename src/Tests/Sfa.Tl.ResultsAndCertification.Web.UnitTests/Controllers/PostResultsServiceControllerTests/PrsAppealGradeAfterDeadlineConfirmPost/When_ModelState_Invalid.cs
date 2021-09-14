using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using AppealContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAppealGradeAfterDeadlineConfirm;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 2;
            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
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

            Loader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(ViewModel);
            Controller.ModelState.AddModelError("IsThisGradeBeingAppealed", AppealContent.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AppealGradeAfterDeadlineConfirmViewModel));

            var model = viewResult.Model as AppealGradeAfterDeadlineConfirmViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(ViewModel.PathwayAssessmentId);
            model.AppealEndDate.Should().Be(ViewModel.AppealEndDate);
            model.PathwayPrsStatus.Should().Be(ViewModel.PathwayPrsStatus);
            model.Status.Should().Be(ViewModel.Status);
            model.Uln.Should().Be(ViewModel.Uln);
            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.DateofBirth.Should().Be(ViewModel.DateofBirth);
            model.PathwayDisplayName.Should().Be(ViewModel.PathwayDisplayName);
            model.PathwayGrade.Should().Be(ViewModel.PathwayGrade);
            model.PathwayAssessmentSeries.Should().Be(ViewModel.PathwayAssessmentSeries);
            model.IsThisGradeBeingAppealed.Should().BeNull();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealGradeAfterDeadline);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AppealGradeAfterDeadlineConfirmViewModel.IsThisGradeBeingAppealed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AppealGradeAfterDeadlineConfirmViewModel.IsThisGradeBeingAppealed)];
            modelState.Errors[0].ErrorMessage.Should().Be(AppealContent.Validation_Message);
        }
    }
}
