using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCoreGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        protected AdminReviewRemoveCoreAssessmentEntryViewModel Mockresult = null;

        public override void Given()
        {
            PathwayId = 10;

            AdminRemovePathwayAssessmentEntryViewModel RemovePathwayAssessmentEntryViewModel = new()
            {
                Learner = "John Smith",
                Provider = "Barnsley College (10000536)",
                Uln = 1080808080,
                RegistrationPathwayId = 10,
                PathwayAssessmentId = 1,
                Tlevel = "Healthcare Science",
                ExamPeriod = "Autumn 2023",
                StartYear = "2023 to 2024",
                PathwayName = "Healthcare Science(6037083X)",
                DoYouWantToRemoveThisAssessmentEntry = true,
                CanAssessmentEntryBeRemoved = true,
                Grade = null
            };

            Mockresult = new AdminReviewRemoveCoreAssessmentEntryViewModel
            {
                ContactName = "Contact Name",
                Day = "01",
                Month = "01",
                Year = "1970",
                ChangeReason = "Reason to request change",
                ZendeskId = "1234567890",
                PathwayAssessmentViewModel = RemovePathwayAssessmentEntryViewModel
            };

            CacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey).Returns(RemovePathwayAssessmentEntryViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewRemoveCoreAssessmentEntryViewModel;

            model.PathwayAssessmentViewModel.RegistrationPathwayId.Should().Be(Mockresult.PathwayAssessmentViewModel.RegistrationPathwayId);
            model.PathwayAssessmentViewModel.Uln.Should().Be(Mockresult.PathwayAssessmentViewModel.Uln);
            model.PathwayAssessmentViewModel.Provider.Should().Be(Mockresult.PathwayAssessmentViewModel.Provider);
            model.PathwayAssessmentViewModel.Tlevel.Should().Be(Mockresult.PathwayAssessmentViewModel.Tlevel);

            // Learner
            model.PathwayAssessmentViewModel.SummaryLearner.Title.Should().Be(RemoveAssessmentEntryCore.Summary_Learner_Text);
            model.PathwayAssessmentViewModel.SummaryLearner.Value.Should().Be(Mockresult.PathwayAssessmentViewModel.Learner);

            //Uln
            model.PathwayAssessmentViewModel.SummaryUln.Title.Should().Be(RemoveAssessmentEntryCore.Summary_ULN_Text);
            model.PathwayAssessmentViewModel.SummaryUln.Value.Should().Be(Mockresult.PathwayAssessmentViewModel.Uln.ToString());

            // Provider
            model.PathwayAssessmentViewModel.SummaryProvider.Title.Should().Be(RemoveAssessmentEntryCore.Summary_Provider_Text);
            model.PathwayAssessmentViewModel.SummaryProvider.Value.Should().Be(Mockresult.PathwayAssessmentViewModel.Provider);

            // TLevelTitle
            model.PathwayAssessmentViewModel.SummaryTlevel.Title.Should().Be(RemoveAssessmentEntryCore.Summary_TLevel_Text);
            model.PathwayAssessmentViewModel.SummaryTlevel.Value.Should().Be(Mockresult.PathwayAssessmentViewModel.Tlevel);

            // StartYear
            model.PathwayAssessmentViewModel.SummaryStartYear.Title.Should().Be(RemoveAssessmentEntryCore.Summary_StartYear_Text);
            model.PathwayAssessmentViewModel.SummaryStartYear.Value.Should().Be(Mockresult.PathwayAssessmentViewModel.StartYear);


            // ContactName
            model.SummaryContactName.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Contact_Name_Text);

            // Day
            model.SummaryDay.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Day_Text);

            // Month
            model.SummaryMonth.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Month_Text);

            // Year
            model.SummaryYear.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Year_Text);

            // ChangeReason
            model.SummaryChangeReason.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Reason_For_Change_Text);

            // ZendeskId
            model.SummaryZendeskTicketId.Title.Should().Be(AdminReviewRemoveAssessmentEntry.Title_Zendesk_Ticket_Id);

            // AssessmentYear
            model.SummaryAssessmentYear.Title.Should().Be(string.Format(AdminReviewRemoveAssessmentEntry.Label_Core_Component, model.PathwayAssessmentViewModel.PathwayName));
            model.SummaryAssessmentYear.Value.Should().Be(model.PathwayAssessmentViewModel.ExamPeriod);
            model.SummaryAssessmentYear.Value2.Should().Be(string.Format(AdminReviewRemoveAssessmentEntry.Label_No_Assessment_Entry_Recorded, model.PathwayAssessmentViewModel.ExamPeriod));
            model.SummaryAssessmentYear.RouteName.Should().Be(RouteConstants.RemoveAssessmentEntryCore);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RemoveAssessmentEntryCore);
        }
    }
}
