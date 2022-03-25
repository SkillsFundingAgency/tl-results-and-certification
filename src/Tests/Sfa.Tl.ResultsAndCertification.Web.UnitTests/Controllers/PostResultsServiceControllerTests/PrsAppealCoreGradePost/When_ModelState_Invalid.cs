using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
//using AppealCoreGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.AppealCoreGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private PrsAddAppealViewModel _appealCoreGradeViewModel;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _appealCoreGradeViewModel = new PrsAddAppealViewModel
            {
                //ProfileId = ProfileId,
                //PathwayAssessmentId = AssessmentId,
                //PathwayResultId = ResultId,
                //Uln = 1234567890,
                //LearnerName = "John Smith",
                //DateofBirth = DateTime.Today.AddYears(-20),
                //PathwayName = "Childcare",
                //PathwayCode = "12121212",
                //PathwayAssessmentSeries = "Summer 2021",
                //PathwayGrade = "B",
                //HasPathwayResult = true
            };

            //Loader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_appealCoreGradeViewModel);

            //ViewModel = new PrsAddAppealViewModel { ProfileId = 1, PathwayAssessmentId = AssessmentId, PathwayResultId = ResultId, AppealGrade = null };
            //Controller.ModelState.AddModelError("AppealGrade", AppealCoreGradeContent.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            //Result.Should().BeOfType(typeof(ViewResult));

            //var viewResult = Result as ViewResult;
            //viewResult.Model.Should().BeOfType(typeof(PrsAddAppealViewModel));

            //var model = viewResult.Model as PrsAddAppealViewModel;

            //model.Should().NotBeNull();
            //model.ProfileId.Should().Be(_appealCoreGradeViewModel.ProfileId);
            //model.PathwayAssessmentId.Should().Be(_appealCoreGradeViewModel.PathwayAssessmentId);
            //model.PathwayResultId.Should().Be(_appealCoreGradeViewModel.PathwayResultId);
            //model.Uln.Should().Be(_appealCoreGradeViewModel.Uln);
            //model.LearnerName.Should().Be(_appealCoreGradeViewModel.LearnerName);
            //model.DateofBirth.Should().Be(_appealCoreGradeViewModel.DateofBirth);
            //model.PathwayDisplayName.Should().Be(_appealCoreGradeViewModel.PathwayDisplayName);
            //model.PathwayAssessmentSeries.Should().Be(_appealCoreGradeViewModel.PathwayAssessmentSeries);
            //model.PathwayGrade.Should().Be(_appealCoreGradeViewModel.PathwayGrade);
            //model.HasPathwayResult.Should().Be(_appealCoreGradeViewModel.HasPathwayResult);
            //model.PathwayDisplayName.Should().Be($"{_appealCoreGradeViewModel.PathwayName}<br/>({_appealCoreGradeViewModel.PathwayCode})");
            //model.SuccessBannerMessage.Should().Be(string.Format(AppealCoreGradeContent.Banner_Message, $"{_appealCoreGradeViewModel.PathwayName} ({_appealCoreGradeViewModel.PathwayCode})"));

            //model.BackLink.Should().NotBeNull();
            //model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            //model.BackLink.RouteAttributes.Count.Should().Be(2);
            //model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            //profileIdRouteValue.Should().Be(ProfileId.ToString());
            //model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentIdRouteValue);
            //assessmentIdRouteValue.Should().Be(AssessmentId.ToString());

            //Controller.ViewData.ModelState.Should().ContainSingle();
            //Controller.ViewData.ModelState.ContainsKey(nameof(PrsAddAppealViewModel.AppealGrade)).Should().BeTrue();

            //var modelState = Controller.ViewData.ModelState[nameof(PrsAddAppealViewModel.AppealGrade)];
            //modelState.Errors[0].ErrorMessage.Should().Be(Content.PostResultsService.AppealCoreGrade.Validation_Message);
        }
    }
}
