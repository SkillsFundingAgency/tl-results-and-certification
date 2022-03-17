using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommPost
{
    public class When_ModelState_Invalid_For_Core : TestSetup
    {
        private PrsAddRommViewModel _addRommCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _addRommCoreGradeViewModel = new PrsAddRommViewModel
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
                ComponentType = ComponentType,
                RommEndDate = DateTime.UtcNow.AddDays(7)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(AoUkprn, _addRommCoreGradeViewModel.ProfileId, _addRommCoreGradeViewModel.AssessmentId, _addRommCoreGradeViewModel.ComponentType)
                  .Returns(_addRommCoreGradeViewModel);

            ViewModel = new PrsAddRommViewModel { ProfileId = 1, AssessmentId = AssessmentId, ComponentType = ComponentType.Core, IsRommRequested = null };
            Controller.ModelState.AddModelError("IsRommRequested", Content.PostResultsService.PrsAddRomm.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(PrsAddRommViewModel));

            var model = viewResult.Model as PrsAddRommViewModel;

            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_addRommCoreGradeViewModel.ProfileId);
            model.AssessmentId.Should().Be(_addRommCoreGradeViewModel.AssessmentId);
            model.Uln.Should().Be(_addRommCoreGradeViewModel.Uln);
            model.LearnerName.Should().Be(_addRommCoreGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_addRommCoreGradeViewModel.DateofBirth);
            model.TlevelTitle.Should().Be(_addRommCoreGradeViewModel.TlevelTitle);
            model.CoreDisplayName.Should().Be(_addRommCoreGradeViewModel.CoreDisplayName);
            model.ExamPeriod.Should().Be(_addRommCoreGradeViewModel.ExamPeriod);
            model.Grade.Should().Be(_addRommCoreGradeViewModel.Grade);
            model.RommEndDate.Should().Be(_addRommCoreGradeViewModel.RommEndDate);
            model.ComponentType.Should().Be(_addRommCoreGradeViewModel.ComponentType);
            model.IsRommRequested.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddRommViewModel.IsRommRequested)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsAddRommViewModel.IsRommRequested)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.PrsAddRomm.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());            
        }
    }
}
