using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeChangePost
{
    public class When_ModelState_Invalid_For_Core : TestSetup
    {
        private PrsAppealGradeChangeViewModel _appealGradeChangeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _appealGradeChangeViewModel = new PrsAppealGradeChangeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreName = "Childcare",
                CoreLarId = "12121212",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.Reviewed,
                AppealEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_appealGradeChangeViewModel);

            ViewModel = new PrsAppealGradeChangeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = null };
            Controller.ModelState.AddModelError("SelectedGradeCode", Content.PostResultsService.PrsAppealGradeChange.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAppealGradeChangeViewModel));

            var model = viewResult.Model as PrsAppealGradeChangeViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_appealGradeChangeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_appealGradeChangeViewModel.AssessmentId);
            model.Uln.Should().Be(_appealGradeChangeViewModel.Uln);
            model.LearnerName.Should().Be(_appealGradeChangeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealGradeChangeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_appealGradeChangeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_appealGradeChangeViewModel.CoreName);
            model.CoreLarId.Should().Be(_appealGradeChangeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_appealGradeChangeViewModel.CoreName} ({_appealGradeChangeViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_appealGradeChangeViewModel.ExamPeriod);
            model.Grade.Should().Be(_appealGradeChangeViewModel.Grade);
            model.AppealEndDate.Should().Be(_appealGradeChangeViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_appealGradeChangeViewModel.ComponentType);
            model.SelectedGradeCode.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAppealGradeChangeViewModel.SelectedGradeCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAppealGradeChangeViewModel.SelectedGradeCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAppealGradeChange.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddAppealOutcomeKnown);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.ComponentType].Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes[Constants.AppealOutcomeKnownTypeId].Should().Be(((int)AppealOutcomeKnownType.GradeChanged).ToString());
        }
    }
}
