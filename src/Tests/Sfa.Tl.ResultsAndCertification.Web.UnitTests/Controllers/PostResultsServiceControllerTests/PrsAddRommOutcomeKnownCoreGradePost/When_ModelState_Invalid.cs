using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownCoreGradePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private PrsAddRommOutcomeKnownCoreGradeViewModel _addRommOutcomeKnownCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addRommOutcomeKnownCoreGradeViewModel = new PrsAddRommOutcomeKnownCoreGradeViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = " Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "TLevel in Childcare",
                CoreDisplayName = "Childcare (12121212)",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = null,
                RommEndDate = DateTime.UtcNow.AddDays(7),
                ComponentType = ComponentType,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeKnownCoreGradeViewModel);

            ViewModel = new PrsAddRommOutcomeKnownCoreGradeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, RommOutcome = null };
            Controller.ModelState.AddModelError("RommOutcome", Content.PostResultsService.PrsAddRommOutcomeKnownCoreGrade.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddRommOutcomeKnownCoreGradeViewModel));

            var model = viewResult.Model as PrsAddRommOutcomeKnownCoreGradeViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.Uln);
            model.LearnerName.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommOutcomeKnownCoreGradeViewModel.ComponentType);
            model.RommOutcome.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddRommOutcomeKnownCoreGradeViewModel.RommOutcome)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddRommOutcomeKnownCoreGradeViewModel.RommOutcome)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddRommOutcomeKnownCoreGrade.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddRomm);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.ComponentType].Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes[Constants.IsBack].Should().Be("true");
        }
    }
}
