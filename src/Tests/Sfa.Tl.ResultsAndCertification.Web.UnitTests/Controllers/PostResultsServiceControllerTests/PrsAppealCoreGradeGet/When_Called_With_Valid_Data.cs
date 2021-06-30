using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AppealCoreGradeViewModel _appealCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealCoreGradeViewModel = new AppealCoreGradeViewModel
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayResultId = ResultId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                PathwayDisplayName = "Childcare<br/>(12121212)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                HasPathwayResult = true
            };

            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealCoreGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealCoreGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealCoreGradeViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealCoreGradeViewModel.PathwayAssessmentId);
            model.PathwayResultId.Should().Be(_appealCoreGradeViewModel.PathwayResultId);
            model.Uln.Should().Be(_appealCoreGradeViewModel.Uln);
            model.LearnerName.Should().Be(_appealCoreGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealCoreGradeViewModel.DateofBirth);
            model.PathwayDisplayName.Should().Be(_appealCoreGradeViewModel.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_appealCoreGradeViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_appealCoreGradeViewModel.PathwayGrade);
            model.HasPathwayResult.Should().Be(_appealCoreGradeViewModel.HasPathwayResult);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            assessmentIdRouteValue.Should().Be(AssessmentId.ToString());
        }
    }
}