using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommGradeChangePost
{
    public class When_ModelState_Invalid_For_Core : TestSetup
    {
        private PrsRommGradeChangeViewModel _rommGradeChangeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _rommGradeChangeViewModel = new PrsRommGradeChangeViewModel
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
                PrsStatus = null,
                RommEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_rommGradeChangeViewModel);

            ViewModel = new PrsRommGradeChangeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = null };
            Controller.ModelState.AddModelError("SelectedGradeCode", Content.PostResultsService.PrsRommGradeChange.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsRommGradeChangeViewModel));

            var model = viewResult.Model as PrsRommGradeChangeViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_rommGradeChangeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_rommGradeChangeViewModel.AssessmentId);
            model.Uln.Should().Be(_rommGradeChangeViewModel.Uln);
            model.LearnerName.Should().Be(_rommGradeChangeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_rommGradeChangeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_rommGradeChangeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_rommGradeChangeViewModel.CoreName);
            model.CoreLarId.Should().Be(_rommGradeChangeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_rommGradeChangeViewModel.CoreName} ({_rommGradeChangeViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_rommGradeChangeViewModel.ExamPeriod);
            model.Grade.Should().Be(_rommGradeChangeViewModel.Grade);
            model.RommEndDate.Should().Be(_rommGradeChangeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_rommGradeChangeViewModel.ComponentType);
            model.SelectedGradeCode.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsRommGradeChangeViewModel.SelectedGradeCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsRommGradeChangeViewModel.SelectedGradeCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsRommGradeChange.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRommOutcomeKnown);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.ComponentType].Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes[Constants.RommOutcomeKnownTypeId].Should().Be(((int)RommOutcomeKnownType.GradeChanged).ToString());
        }
    }
}
