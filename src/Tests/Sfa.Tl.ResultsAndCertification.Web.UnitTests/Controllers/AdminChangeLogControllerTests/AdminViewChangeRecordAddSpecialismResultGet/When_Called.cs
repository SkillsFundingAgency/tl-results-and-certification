﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests.AdminViewChangeRecordAddSpecialismResultGet
{
    public class When_Called : AdminChangeLogControllerTestBase
    {
        private IActionResult Result;

        private AdminViewChangeRecordAddSpecialismResultViewModel Mockresult;
        private AddSpecialismResultRequest details;

        public override void Given()
        {
            details = new()
            {
                SpecialismAssessmentId = 1,
                GradeTo = "Merit"
            };

            Mockresult = CreateViewModel();

            AdminChangeLogLoader.GetAdminViewChangeAddSpecialismResultRecord(ChangeLogId).Returns(Mockresult);
        }

        public override async Task When()
            => Result = await Controller.AdminViewChangeRecordAddSpecialismResultAsync(ChangeLogId);

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminChangeLogLoader.Received(1).GetAdminViewChangeAddSpecialismResultRecord(ChangeLogId);
        }

        [Fact]
        public void Then_Return_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminViewChangeRecordAddSpecialismResultViewModel;

            model.ChangeLogId.Should().Be(Mockresult.ChangeLogId);
            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);

            model.Learner.Should().Be(Mockresult.Learner);
            model.Uln.Should().Be(Mockresult.Uln);
            model.CreatedBy.Should().Be(Mockresult.CreatedBy);

            model.ChangeDetails.Should().Be(Mockresult.ChangeDetails);
            model.SpecialismDetails.SpecialismAssessmentId.Should().Be(Mockresult.SpecialismDetails.SpecialismAssessmentId);
            model.SpecialismDetails.GradeTo.Should().Be(Mockresult.SpecialismDetails.GradeTo);

            model.SummaryLearner.Title.Should().Be(AdminViewChangeRecord.Summary_Learner_Text);
            model.SummaryLearner.Value.Should().Be(Mockresult.SummaryLearner.Value);

            model.SummaryUln.Title.Should().Be(AdminViewChangeRecord.Summary_ULN_Text);
            model.SummaryUln.Value.Should().Be(Mockresult.SummaryUln.Value);

            model.SummaryCreatedBy.Title.Should().Be(AdminViewChangeRecord.Summary_CreatedBy_Text);
            model.SummaryCreatedBy.Value.Should().Be(Mockresult.SummaryCreatedBy.Value);

            model.ChangeRequestedBy.Should().Be(Mockresult.ChangeRequestedBy);
            model.ChangeDateOfRequest.Should().Be(Mockresult.ChangeDateOfRequest);
            model.ReasonForChange.Should().Be(Mockresult.ReasonForChange);
            model.ZendeskTicketID.Should().Be(Mockresult.ZendeskTicketID);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminSearchChangeLog);
        }

        protected AdminViewChangeRecordAddSpecialismResultViewModel CreateViewModel() => new()
        {
            ChangeLogId = ChangeLogId,
            RegistrationPathwayId = 1,
            Learner = "John Wick",
            Uln = 1234567890,
            CreatedBy = "steve vaugh",
            ChangeRequestedBy = "Mark Jacob",
            ReasonForChange = "Change reason",
            ZendeskTicketID = "ZenDesk-1234",
            DateAndTimeOfChange = "1 Jan, 2024",
            ChangeDetails = JsonConvert.SerializeObject(details),
            SpecialismDetails = details,
            ChangeType = Common.Enum.ChangeType.AddSpecialismResult
        };
    }
}