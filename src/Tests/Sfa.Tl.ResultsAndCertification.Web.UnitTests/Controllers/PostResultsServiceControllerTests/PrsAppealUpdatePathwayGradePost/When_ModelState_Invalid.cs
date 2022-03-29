using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AppealUpdatePathwayGradeViewModel _appealOutcomePathwayGradeViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _appealOutcomePathwayGradeViewModel = new AppealUpdatePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = ResultId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayName = "Childcare",
                PathwayCode = "12121212",
                PathwayDisplayName = "Childcare<br/>(12121212)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Grades = _grades
            };

            Loader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealOutcomePathwayGradeViewModel);

            ViewModel = new AppealUpdatePathwayGradeViewModel { ProfileId = 1, PathwayAssessmentId = AssessmentId, PathwayResultId = ResultId, SelectedGradeCode = null };
            Controller.ModelState.AddModelError("SelectedGradeCode", Content.PostResultsService.AppealUpdatePathwayGrade.Validation_Message); ;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AppealUpdatePathwayGradeViewModel));

            var model = viewResult.Model as AppealUpdatePathwayGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealOutcomePathwayGradeViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayAssessmentId);
            model.PathwayResultId.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayResultId);
            model.Uln.Should().Be(_appealOutcomePathwayGradeViewModel.Uln);
            model.LearnerName.Should().Be(_appealOutcomePathwayGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealOutcomePathwayGradeViewModel.DateofBirth);
            model.PathwayName.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayName);
            model.PathwayCode.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayCode);
            model.PathwayDisplayName.Should().Be($"{_appealOutcomePathwayGradeViewModel.PathwayName}<br/>({_appealOutcomePathwayGradeViewModel.PathwayCode})");
            model.PathwayAssessmentSeries.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayPrsStatus);
            model.SelectedGradeCode.Should().Be(_appealOutcomePathwayGradeViewModel.SelectedGradeCode);
            model.Grades.Should().BeEquivalentTo(_appealOutcomePathwayGradeViewModel.Grades);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsAppealOutcomePathwayGrade);
            model.BackLink.RouteAttributes.Count.Should().Be(4);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ResultId, out string resultIdRouteValue);
            resultIdRouteValue.Should().Be(ResultId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AppealOutcomeTypeId, out string outcomeTypeIdRouteValue);
            outcomeTypeIdRouteValue.Should().Be(((int)AppealOutcomeType.GradeChanged).ToString());

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AppealUpdatePathwayGradeViewModel.SelectedGradeCode)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AppealUpdatePathwayGradeViewModel.SelectedGradeCode)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.AppealUpdatePathwayGrade.Validation_Message);
        }
    }
}
