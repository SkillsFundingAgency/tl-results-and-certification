using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests.AdminViewChangeRecordSpecialismAssessmentGet
{
    public class When_Called : AdminChangeLogControllerTestBase
    {
        private IActionResult Result;

        private AdminViewChangeRecordSpecialismAssessmentViewModel Mockresult;
        private AddSpecialismDetails details;

        public override void Given()
        {
            details = new()
            {
                AssessmentSeriesId = 1,
                SpecialismAssessmentFrom = "No assessment entry recorded for summer 2023",
                SpecialismAssessmentTo = "Summer 2023",
            };

            Mockresult = CreateViewModel();

            AdminChangeLogLoader.GetAdminViewChangeSpecialismAssessmentRecord(ChangeLogId).Returns(Mockresult);
        }

        public override async Task When()
            => Result = await Controller.AdminViewChangeRecordSpecialismAssessmentAsync(ChangeLogId);

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminChangeLogLoader.Received(1).GetAdminViewChangeSpecialismAssessmentRecord(ChangeLogId);
        }

        [Fact]
        public void Then_Return_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminViewChangeRecordSpecialismAssessmentViewModel;

            model.ChangeLogId.Should().Be(Mockresult.ChangeLogId);
            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);

            model.Learner.Should().Be(Mockresult.Learner);
            model.Uln.Should().Be(Mockresult.Uln);
            model.CreatedBy.Should().Be(Mockresult.CreatedBy);

            model.ChangeDetails.Should().Be(Mockresult.ChangeDetails);
            model.SpecialismDetails.AssessmentSeriesId.Should().Be(Mockresult.SpecialismDetails.AssessmentSeriesId);
            model.SpecialismDetails.SpecialismAssessmentFrom.Should().Be(Mockresult.SpecialismDetails.SpecialismAssessmentFrom);
            model.SpecialismDetails.SpecialismAssessmentTo.Should().Be(Mockresult.SpecialismDetails.SpecialismAssessmentTo);

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
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminSearchChangeLogClear);
        }

        protected AdminViewChangeRecordSpecialismAssessmentViewModel CreateViewModel() => new()
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
            ChangeType = Common.Enum.ChangeType.AddSpecialismAssessment
        };
    }
}