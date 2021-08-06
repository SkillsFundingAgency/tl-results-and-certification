using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using GradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsGradeChangeRequest;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestGet
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private PrsGradeChangeRequestViewModel _mockGradeChangeRequestViewModel;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;
            ResultId = 1;

            _mockGradeChangeRequestViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                ResultId = ResultId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                Status = RegistrationPathwayStatus.Active,

                ProviderName = "Barsely College",
                ProviderUkprn = 9876543210,

                TlevelTitle = "Tlevel in Childcare",

                PathwayAssessmentSeries = "Summer 2021",
                PathwayGrade = "B",
                PathwayPrsStatus = PrsStatus.Final
            };

            Loader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_mockGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsGradeChangeRequestViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_mockGradeChangeRequestViewModel.ProfileId);
            model.AssessmentId.Should().Be(_mockGradeChangeRequestViewModel.AssessmentId);
            model.ResultId.Should().Be(_mockGradeChangeRequestViewModel.ResultId);
            model.Uln.Should().Be(_mockGradeChangeRequestViewModel.Uln);
            model.Firstname.Should().Be(_mockGradeChangeRequestViewModel.Firstname);
            model.Lastname.Should().Be(_mockGradeChangeRequestViewModel.Lastname);
            model.DateofBirth.Should().Be(_mockGradeChangeRequestViewModel.DateofBirth);
            model.Status.Should().Be(_mockGradeChangeRequestViewModel.Status);
            model.CanRequestFinalGradeChange.Should().BeTrue();
            model.ChangeRequestData.Should().BeNull();
            model.IsResultJourney.Should().BeFalse();

            model.ProviderName.Should().Be(_mockGradeChangeRequestViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(_mockGradeChangeRequestViewModel.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockGradeChangeRequestViewModel.TlevelTitle);

            model.PathwayAssessmentSeries.Should().Be(_mockGradeChangeRequestViewModel.PathwayAssessmentSeries);
            model.PathwayGrade.Should().Be(_mockGradeChangeRequestViewModel.PathwayGrade);
            model.PathwayPrsStatus.Should().Be(_mockGradeChangeRequestViewModel.PathwayPrsStatus);

            // Uln
            model.SummaryUln.Title.Should().Be(GradeChangeContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockGradeChangeRequestViewModel.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(GradeChangeContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockGradeChangeRequestViewModel.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(GradeChangeContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockGradeChangeRequestViewModel.DateofBirth.ToDobFormat());

            // Pathway name
            model.SummaryCore.Title.Should().Be(GradeChangeContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(_mockGradeChangeRequestViewModel.PathwayDisplayName);
            model.SummaryCore.IsRawHtml.Should().BeTrue();

            // Exam period
            model.SummaryCoreExamPeriod.Title.Should().Be(GradeChangeContent.Title_ExamPeriod_Text);
            model.SummaryCoreExamPeriod.Value.Should().Be(_mockGradeChangeRequestViewModel.PathwayAssessmentSeries);

            // Pathway grade
            model.SummaryCoreGrade.Title.Should().Be(GradeChangeContent.Title_Grade_Text);
            model.SummaryCoreGrade.Value.Should().Be(_mockGradeChangeRequestViewModel.PathwayGrade);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(2);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(_mockGradeChangeRequestViewModel.ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.AssessmentId, out string assessmentId);
            assessmentId.Should().Be(_mockGradeChangeRequestViewModel.AssessmentId.ToString());
        }
    }
}
