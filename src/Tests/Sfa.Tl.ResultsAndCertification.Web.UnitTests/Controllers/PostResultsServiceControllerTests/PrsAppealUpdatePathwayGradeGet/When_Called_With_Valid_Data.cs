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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AppealUpdatePathwayGradeViewModel _appealUpdatePathwayGradeViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _appealUpdatePathwayGradeViewModel = new AppealUpdatePathwayGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = ResultId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayCode = "12121212",
                PathwayName = "Childcare",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Grades = _grades
            };

            Loader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealUpdatePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealUpdatePathwayGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealUpdatePathwayGradeViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayAssessmentId);
            model.PathwayResultId.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayResultId);
            model.Uln.Should().Be(_appealUpdatePathwayGradeViewModel.Uln);
            model.LearnerName.Should().Be(_appealUpdatePathwayGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealUpdatePathwayGradeViewModel.DateofBirth);
            model.PathwayName.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayName);
            model.PathwayCode.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayCode);
            model.PathwayDisplayName.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_appealUpdatePathwayGradeViewModel.PathwayPrsStatus);
            model.SelectedGradeCode.Should().Be(_appealUpdatePathwayGradeViewModel.SelectedGradeCode);
            model.Grades.Should().BeEquivalentTo(_appealUpdatePathwayGradeViewModel.Grades);

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
            outcomeTypeIdRouteValue.Should().Be(((int)AppealOutcomeType.UpdateGrade).ToString());
        }
    }
}
