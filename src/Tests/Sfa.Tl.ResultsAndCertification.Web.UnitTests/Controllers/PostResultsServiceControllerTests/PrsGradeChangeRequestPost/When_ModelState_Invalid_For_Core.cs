﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

using GradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsGradeChangeRequest;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestPost
{
    public class When_ModelState_Invalid_For_Core : TestSetup
    {
        private PrsGradeChangeRequestViewModel _mockGradeChangeRequestViewModel;

        public override void Given()
        {
            ViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ComponentType = ComponentType.Core,
                ChangeRequestData = string.Empty
            };

            _mockGradeChangeRequestViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = ViewModel.ProfileId,
                AssessmentId = ViewModel.AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                Status = RegistrationPathwayStatus.Active,

                ProviderName = "Barsely College",
                ProviderUkprn = 9876543210,
                CoreName = "Education",
                CoreLarId = "1234567",
                TlevelTitle = "Tlevel in Childcare",

                ExamPeriod = "Summer 2021",
                Grade = "B",
                PrsStatus = PrsStatus.Final
            };

            Loader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockGradeChangeRequestViewModel);
            Controller.ModelState.AddModelError("ChangeRequestData", GradeChangeContent.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ValidationError() 
        {
            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(PrsGradeChangeRequestViewModel.ChangeRequestData)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(PrsGradeChangeRequestViewModel.ChangeRequestData)];
            modelState.Errors[0].ErrorMessage.Should().Be(GradeChangeContent.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsGradeChangeRequestViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_mockGradeChangeRequestViewModel.ProfileId);
            model.Uln.Should().Be(_mockGradeChangeRequestViewModel.Uln);
            model.Firstname.Should().Be(_mockGradeChangeRequestViewModel.Firstname);
            model.Lastname.Should().Be(_mockGradeChangeRequestViewModel.Lastname);
            model.DateofBirth.Should().Be(_mockGradeChangeRequestViewModel.DateofBirth);
            model.CoreName.Should().Be(_mockGradeChangeRequestViewModel.CoreName);
            model.CoreLarId.Should().Be(_mockGradeChangeRequestViewModel.CoreLarId);
            model.CoreDisplayName.Should().Be($"{_mockGradeChangeRequestViewModel.CoreName} ({_mockGradeChangeRequestViewModel.CoreLarId})");
            model.Status.Should().Be(_mockGradeChangeRequestViewModel.Status);
            model.CanRequestFinalGradeChange.Should().BeTrue();
            model.ChangeRequestData.Should().BeNull();

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
