using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomePost
{
    public class When_ModelState_Invalid_For_Core : TestSetup
    {
        private PrsAddAppealOutcomeViewModel _addAppealOutcomeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addAppealOutcomeViewModel = new PrsAddAppealOutcomeViewModel
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
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addAppealOutcomeViewModel);

            ViewModel = new PrsAddAppealOutcomeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, AppealOutcome = null };
            Controller.ModelState.AddModelError("AppealOutcome", Content.PostResultsService.PrsAddAppealOutcome.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddAppealOutcomeViewModel));

            var model = viewResult.Model as PrsAddAppealOutcomeViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addAppealOutcomeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealOutcomeViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealOutcomeViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealOutcomeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealOutcomeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealOutcomeViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addAppealOutcomeViewModel.CoreName);
            model.CoreLarId.Should().Be(_addAppealOutcomeViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addAppealOutcomeViewModel.CoreName} ({_addAppealOutcomeViewModel.CoreLarId})");
            model.ExamPeriod.Should().Be(_addAppealOutcomeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealOutcomeViewModel.Grade);
            model.ComponentType.Should().Be(_addAppealOutcomeViewModel.ComponentType);
            model.AppealOutcome.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddAppealOutcomeViewModel.AppealOutcome)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddAppealOutcomeViewModel.AppealOutcome)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddAppealOutcome.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
        }
    }
}
