using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminChangeLogControllerTests.AdminViewChangeRecordPathwayRommOutcomeGet
{
    public class When_Called : AdminChangeLogControllerTestBase
    {
        private IActionResult Result;

        private AdminViewChangeRecordPathwayRommOutcomeViewModel Mockresult;
        private PathwayRommOutcomeDetails details;

        public override void Given()
        {
            details = new()
            {
                PathwayResultId = 3104334,
                From = "B",
                To = "A*",
                PathwayAssessmentId = "24325162",
                RegistrationPathwayId = "29548539"
            };

            Mockresult = CreateViewModel();

            AdminChangeLogLoader.GetAdminViewChangePathwayRommOutcomeRecord(ChangeLogId).Returns(Mockresult);
        }

        public override async Task When()
            => Result = await Controller.AdminViewChangeRecordPathwayRommOutcomeAsync(ChangeLogId);

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminChangeLogLoader.Received(1).GetAdminViewChangePathwayRommOutcomeRecord(ChangeLogId);
        }

        [Fact]
        public void Then_Return_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminViewChangeRecordPathwayRommOutcomeViewModel;

            model.ChangeLogId.Should().Be(Mockresult.ChangeLogId);
            model.RegistrationPathwayId.Should().Be(Mockresult.RegistrationPathwayId);

            model.Learner.Should().Be(Mockresult.Learner);
            model.Uln.Should().Be(Mockresult.Uln);
            model.CreatedBy.Should().Be(Mockresult.CreatedBy);

            model.ChangeDetails.Should().Be(Mockresult.ChangeDetails);
            model.RommOutcomeDetails.PathwayResultId.Should().Be(Mockresult.RommOutcomeDetails.PathwayResultId);
            model.RommOutcomeDetails.From.Should().Be(Mockresult.RommOutcomeDetails.From);
            model.RommOutcomeDetails.To.Should().Be(Mockresult.RommOutcomeDetails.To);
            model.RommOutcomeDetails.PathwayAssessmentId.Should().Be(Mockresult.RommOutcomeDetails.PathwayAssessmentId);
            model.RommOutcomeDetails.RegistrationPathwayId.Should().Be(Mockresult.RommOutcomeDetails.RegistrationPathwayId);

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

        protected AdminViewChangeRecordPathwayRommOutcomeViewModel CreateViewModel() => new()
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
            RommOutcomeDetails = details,
            ChangeType = Common.Enum.ChangeType.PathwayRommOutcome
        };
    }
}