using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownPost
{
    public class When_ModelState_Invalid_For_Specialism : TestSetup
    {
        private PrsAddRommOutcomeKnownViewModel _addRommOutcomeKnownViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addRommOutcomeKnownViewModel = new PrsAddRommOutcomeKnownViewModel
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
                ComponentType = ComponentType,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_addRommOutcomeKnownViewModel);

            ViewModel = new PrsAddRommOutcomeKnownViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, RommOutcome = null };
            Controller.ModelState.AddModelError("RommOutcome", Content.PostResultsService.PrsAddRommOutcomeKnown.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddRommOutcomeKnownViewModel));

            var model = viewResult.Model as PrsAddRommOutcomeKnownViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addRommOutcomeKnownViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommOutcomeKnownViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommOutcomeKnownViewModel.Uln);
            model.LearnerName.Should().Be(_addRommOutcomeKnownViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommOutcomeKnownViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommOutcomeKnownViewModel.TlevelTitle);
            model.SpecialismName.Should().Be(_addRommOutcomeKnownViewModel.SpecialismName);
            model.SpecialismLarId.Should().Be(_addRommOutcomeKnownViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addRommOutcomeKnownViewModel.SpecialismName} ({_addRommOutcomeKnownViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addRommOutcomeKnownViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommOutcomeKnownViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommOutcomeKnownViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommOutcomeKnownViewModel.ComponentType);
            model.RommOutcome.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddRommOutcomeKnownViewModel.RommOutcome)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddRommOutcomeKnownViewModel.RommOutcome)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddRommOutcomeKnown.Validation_Message);

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
