using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealOutcomePathwayGradeGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AppealOutcomePathwayGradeViewModel _appealOutcomePathwayGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealOutcomePathwayGradeViewModel = new AppealOutcomePathwayGradeViewModel
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
                PathwayPrsStatus = PrsStatus.BeingAppealed
            };

            Loader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealOutcomePathwayGradeViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AppealOutcomePathwayGradeViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_appealOutcomePathwayGradeViewModel.ProfileId);
            model.PathwayAssessmentId.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayAssessmentId);
            model.PathwayResultId.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayResultId);
            model.Uln.Should().Be(_appealOutcomePathwayGradeViewModel.Uln);
            model.LearnerName.Should().Be(_appealOutcomePathwayGradeViewModel.LearnerName);
            model.DateofBirth.Should().Be(_appealOutcomePathwayGradeViewModel.DateofBirth);
            model.PathwayName.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayName);
            model.PathwayCode.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayCode);
            model.PathwayDisplayName.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_appealOutcomePathwayGradeViewModel.PathwayPrsStatus);

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
