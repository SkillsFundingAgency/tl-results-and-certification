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
    public class When_ViewModel_IsValid_For_Specialism : TestSetup
    {
        private PrsGradeChangeRequestViewModel _mockGradeChangeRequestViewModel;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;
            ComponentType = (int)Common.Enum.ComponentType.Specialism;
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
                SpecialismName = "Heating",
                SpecialismLarId = "Z1234567",
                TlevelTitle = "Tlevel in Childcare",

                ExamPeriod = "Summer 2021",
                Grade = "B",
                PrsStatus = null,
                RommEndDate = DateTime.Now.AddDays(-1),
                AppealEndDate = DateTime.Now.AddDays(10)
            };

            Loader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, (ComponentType)ComponentType).Returns(_mockGradeChangeRequestViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ProfileId, AssessmentId, (ComponentType)ComponentType);
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
            model.SpecialismName.Should().Be(_mockGradeChangeRequestViewModel.SpecialismName);
            model.SpecialismLarId.Should().Be(_mockGradeChangeRequestViewModel.SpecialismLarId);
            model.SpecialismDisplayName.Should().Be($"{_mockGradeChangeRequestViewModel.SpecialismName} ({_mockGradeChangeRequestViewModel.SpecialismLarId})");
            model.Status.Should().Be(_mockGradeChangeRequestViewModel.Status);
            model.PrsStatus.Should().Be(_mockGradeChangeRequestViewModel.PrsStatus);
            model.AppealEndDate.Should().Be(_mockGradeChangeRequestViewModel.AppealEndDate);
            model.CanRequestFinalGradeChange.Should().BeTrue();
            model.ChangeRequestData.Should().BeNull();
            model.IsResultJourney.Should().BeFalse();

            model.ProviderName.Should().Be(_mockGradeChangeRequestViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(_mockGradeChangeRequestViewModel.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockGradeChangeRequestViewModel.TlevelTitle);

            model.ExamPeriod.Should().Be(_mockGradeChangeRequestViewModel.ExamPeriod);
            model.Grade.Should().Be(_mockGradeChangeRequestViewModel.Grade);
            model.PrsStatus.Should().Be(_mockGradeChangeRequestViewModel.PrsStatus);

            // Uln
            model.SummaryUln.Title.Should().Be(GradeChangeContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_mockGradeChangeRequestViewModel.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(GradeChangeContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_mockGradeChangeRequestViewModel.LearnerName);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(GradeChangeContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_mockGradeChangeRequestViewModel.DateofBirth.ToDobFormat());

            // T Level title
            model.SummaryTlevelTitle.Title.Should().Be(GradeChangeContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_mockGradeChangeRequestViewModel.TlevelTitle);

            // Pathway name
            model.SummaryCore.Title.Should().Be(GradeChangeContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(_mockGradeChangeRequestViewModel.CoreDisplayName);

            // Exam period
            model.SummaryExamPeriod.Title.Should().Be(GradeChangeContent.Title_ExamPeriod_Text);
            model.SummaryExamPeriod.Value.Should().Be(_mockGradeChangeRequestViewModel.ExamPeriod);

            // Pathway grade
            model.SummaryGrade.Title.Should().Be(GradeChangeContent.Title_Grade_Text);
            model.SummaryGrade.Value.Should().Be(_mockGradeChangeRequestViewModel.Grade);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(_mockGradeChangeRequestViewModel.ProfileId.ToString());
        }
    }
}
