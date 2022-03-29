using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealPost
{
    public class When_ModelState_Invalid_For_Specialism : TestSetup
    {
        private PrsAddAppealViewModel _addAppealViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _addAppealViewModel = new PrsAddAppealViewModel
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
                SpecialismName = "Heating",
                SpecialismLarId = "Z0001234",
                ExamPeriod = "Summer 2021",
                Grade = "A",
                PrsStatus = PrsStatus.Reviewed,
                ComponentType = ComponentType,
                AppealEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, _addAppealViewModel.ProfileId, _addAppealViewModel.AssessmentId, _addAppealViewModel.ComponentType)
                  .Returns(_addAppealViewModel);

            ViewModel = new PrsAddAppealViewModel { ProfileId = 1, AssessmentId = AssessmentId, ComponentType = ComponentType, IsAppealRequested = null };
            Controller.ModelState.AddModelError("IsAppealRequested", Content.PostResultsService.PrsAddRomm.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddAppealViewModel));

            var model = viewResult.Model as PrsAddAppealViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addAppealViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addAppealViewModel.AssessmentId);
            model.Uln.Should().Be(_addAppealViewModel.Uln);
            model.LearnerName.Should().Be(_addAppealViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addAppealViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addAppealViewModel.TlevelTitle);
            model.CoreName.Should().Be(_addAppealViewModel.CoreName);
            model.CoreLarId.Should().Be(_addAppealViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_addAppealViewModel.CoreName} ({_addAppealViewModel.CoreLarId})");
            model.SpecialismLarId.Should().Be(_addAppealViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_addAppealViewModel.SpecialismName} ({_addAppealViewModel.SpecialismLarId})");
            model.ExamPeriod.Should().Be(_addAppealViewModel.ExamPeriod);
            model.Grade.Should().Be(_addAppealViewModel.Grade);
            model.AppealEndDate.Should().Be(_addAppealViewModel.AppealEndDate);
            model.ComponentType.Should().Be(_addAppealViewModel.ComponentType);
            model.IsAppealRequested.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddAppealViewModel.IsAppealRequested)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddAppealViewModel.IsAppealRequested)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddRomm.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
