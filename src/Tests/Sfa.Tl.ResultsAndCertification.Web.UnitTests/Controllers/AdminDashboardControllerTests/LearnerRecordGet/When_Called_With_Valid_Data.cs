using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Threading.Tasks;
using Xunit;
using IndustryPlacement = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.LearnerRecordGet
{
    public class When_Called_With_Valid_Data : AdminDashboardControllerTestBase
    {
        private const int RegistrationPathwayId = 10;
        private IActionResult Result;

        private AdminLearnerRecordViewModel _loaderResult;

        public override void Given()
        {
            _loaderResult = new AdminLearnerRecordViewModel
            {
                RegistrationPathwayId = 15,
                Uln = 1235469874,
                LearnerName = "John Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barnsley College",
                ProviderUkprn = 58794528,
                TlevelName = "Tlevel in Test Pathway Name",
                AcademicYear = 2021,
                AwardingOrganisationName = "NCFE",
                IsLearnerRegistered = true,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                PrintCertificateType = PrintCertificateType.StatementOfAchievement,
                BatchId = 1000,
                PrintRequestSubmittedOn = new DateTime(2024, 1, 1),
                PrintingBatchItemStatus = PrintingBatchItemStatus.AwaitingProcessing,
                PrintingBatchItemStatusChangedOn = new DateTime(2024, 1, 2),
                TrackingId = "1Z6F88X56839656159"
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(RegistrationPathwayId).Returns(_loaderResult);
        }

        public async override Task When()
        {
            Result = await Controller.AdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = Result.ShouldBeViewResult<AdminLearnerRecordViewModel>();

            model.RegistrationPathwayId.Should().Be(_loaderResult.RegistrationPathwayId);
            model.Uln.Should().Be(_loaderResult.Uln);
            model.LearnerName.Should().Be(_loaderResult.LearnerName);
            model.DateofBirth.Should().Be(_loaderResult.DateofBirth);
            model.ProviderName.Should().Be(_loaderResult.ProviderName);
            model.ProviderUkprn.Should().Be(_loaderResult.ProviderUkprn);
            model.TlevelName.Should().Be(_loaderResult.TlevelName);
            model.StartYear.Should().Be("2021 to 2022");
            model.AwardingOrganisationName.Should().Be(_loaderResult.AwardingOrganisationName);
            model.IsLearnerRegistered.Should().Be(_loaderResult.IsLearnerRegistered);

            model.IndustryPlacementId.Should().Be(_loaderResult.IndustryPlacementId);
            model.IndustryPlacementStatus.Should().Be(_loaderResult.IndustryPlacementStatus);

            model.IsIndustryPlacementAdded.Should().BeTrue();
            model.IsStatusCompleted.Should().BeTrue();

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(LearnerRecordDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_loaderResult.DateofBirth.ToDobFormat());

            // ProviderName
            model.SummaryProviderName.Value.Should().Be($"{_loaderResult.ProviderName} ({_loaderResult.ProviderUkprn})");

            // ProviderUkprn
            model.SummaryProviderUkprn.Value.Should().Be(_loaderResult.ProviderUkprn.ToString());

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(LearnerRecordDetailsContent.Title_TLevel_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_loaderResult.TlevelName);

            // Start Year
            model.SummaryStartYear.Title.Should().Be(LearnerRecordDetailsContent.Title_StartYear_Text);
            model.SummaryStartYear.Value.Should().Be(_loaderResult.StartYear);

            // AO Name
            model.SummaryAoName.Title.Should().Be(LearnerRecordDetailsContent.Title_AoName_Text);
            model.SummaryAoName.Value.Should().Be(_loaderResult.AwardingOrganisationName);

            // Summary Industry Placement
            model.SummaryIndustryPlacementStatus.Should().NotBeNull();
            model.SummaryIndustryPlacementStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_IP_Status_Text);
            model.SummaryIndustryPlacementStatus.Value.Should().Be(IndustryPlacement.Completed_Display_Text);
            model.SummaryIndustryPlacementStatus.HiddenActionText.Should().Be(LearnerRecordDetailsContent.Hidden_Action_Text_Industry_Placement);
            model.SummaryIndustryPlacementStatus.ActionText.Should().Be(LearnerRecordDetailsContent.Action_Text_Link_Change);

            // Summary Certificate Type
            model.SummaryPrintCertificateType.Should().NotBeNull();
            model.SummaryPrintCertificateType.Title.Should().Be(LearnerRecordDetailsContent.Title_CertificateType_Text);
            model.SummaryPrintCertificateType.Value.Should().Be(_loaderResult.PrintCertificateType.Value.GetDisplayName());

            // Summary Batch Id
            model.SummaryBatchId.Should().NotBeNull();
            model.SummaryBatchId.Title.Should().Be(LearnerRecordDetailsContent.Title_Batch_Id);
            model.SummaryBatchId.Value.Should().Be(_loaderResult.BatchId.ToString());

            // Summary Print Request Submitted On
            model.SummaryPrintRequestSubmittedOn.Should().NotBeNull();
            model.SummaryPrintRequestSubmittedOn.Title.Should().Be(LearnerRecordDetailsContent.Title_Date_Of_Submission);
            model.SummaryPrintRequestSubmittedOn.Value.Should().Be(_loaderResult.PrintRequestSubmittedOn.Value.ToDobFormat());

            // Summary Printing Batch Item Status
            model.SummaryPrintingBatchItemStatus.Should().NotBeNull();
            model.SummaryPrintingBatchItemStatus.Title.Should().Be(LearnerRecordDetailsContent.Title_Batch_Status);
            model.SummaryPrintingBatchItemStatus.Value.Should().Be(_loaderResult.PrintingBatchItemStatus?.GetDisplayName());

            // Summary Printing Batch Item Status Changed On
            model.SummaryPrintingBatchItemStatusChangedOn.Should().NotBeNull();
            model.SummaryPrintingBatchItemStatusChangedOn.Title.Should().Be(LearnerRecordDetailsContent.Title_Batch_Status_Changed_On);
            model.SummaryPrintingBatchItemStatusChangedOn.Value.Should().Be(_loaderResult.PrintingBatchItemStatusChangedOn.Value.ToDobFormat());

            // Summary Batch Id
            model.SummaryTrackingId.Should().NotBeNull();
            model.SummaryTrackingId.Title.Should().Be(LearnerRecordDetailsContent.Title_Tracking_Id);
            model.SummaryTrackingId.Value.Should().Be(_loaderResult.TrackingId);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminSearchLearnersRecords);
        }
    }
}