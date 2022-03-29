using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomeKnownPost
{
    public class When_ModelState_Invalid_For_Specialism : TestSetup
    {
        private PrsAddAppealOutcomeKnownViewModel _addAppealOutcomeKnownViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addAppealOutcomeKnownViewModel = new PrsAddAppealOutcomeKnownViewModel
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
                ComponentType = ComponentType,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealOutcomeKnownViewModel);

            ViewModel = new PrsAddAppealOutcomeKnownViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, AppealOutcome = null };
            Controller.ModelState.AddModelError("AppealOutcome", Content.PostResultsService.PrsAddAppealOutcomeKnown.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddAppealOutcomeKnownViewModel));

            var model = viewResult.Model as PrsAddAppealOutcomeKnownViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addAppealOutcomeKnownViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealOutcomeKnownViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealOutcomeKnownViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealOutcomeKnownViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealOutcomeKnownViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealOutcomeKnownViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addAppealOutcomeKnownViewModel.CoreName);
            model.CoreLarId.Should().Be(_addAppealOutcomeKnownViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addAppealOutcomeKnownViewModel.CoreName} ({_addAppealOutcomeKnownViewModel.CoreLarId})");
            model.SpecialismLarId.Should().Be(_addAppealOutcomeKnownViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addAppealOutcomeKnownViewModel.SpecialismName} ({_addAppealOutcomeKnownViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addAppealOutcomeKnownViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealOutcomeKnownViewModel.Grade);
            model.AppealEndDate.Should().Be(_addAppealOutcomeKnownViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_addAppealOutcomeKnownViewModel.ComponentType);
            model.AppealOutcome.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddAppealOutcomeKnownViewModel.AppealOutcome)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddAppealOutcomeKnownViewModel.AppealOutcome)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddAppealOutcomeKnown.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAddAppeal);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes[Constants.AssessmentId].Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes[Constants.ComponentType].Should().Be(((int)ComponentType).ToString());
            model.BackLink.RouteAttributes[Constants.IsBack].Should().Be("true");
        }
    }
}
