using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using BatchItemStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.PrintingBatchItemStatus;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminLearnerRecordViewModel
    {
        // Header
        public int RegistrationPathwayId { get; set; }
        public int TlPathwayId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus MathsStatus { get; set; }
        public SubjectStatus EnglishStatus { get; set; }
        public string OverallResult { get; set; }

        public bool IsCertificateRerequestEligible { get; set; }

        public DateTime? LastPrintCertificateRequestedDate { get; set; }

        public int IndustryPlacementId { get; set; }

        public IpStatus IndustryPlacementStatus { get; set; }

        public int? BatchId { get; set; }

        public DateTime? PrintRequestSubmittedOn { get; set; }

        public BatchItemStatus? PrintingBatchItemStatus { get; set; }

        public DateTime? PrintingBatchItemStatusChangedOn { get; set; }

        public string TrackingId { get; set; }

        public PrintCertificateType? PrintCertificateType { get; set; }

        public string StartYear => string.Format(LearnerRecordDetailsContent.Start_Year_Value, AcademicYear, AcademicYear + 1);

        /// <summary>
        /// True when status is Active or Withdrawn
        /// </summary>
        public bool IsLearnerRegistered { get; set; }
        public bool IsStatusCompleted => IsMathsAdded && IsEnglishAdded && (IsIndustryPlacementAdded && IndustryPlacementStatus != IpStatus.NotCompleted);
        public bool IsMathsAdded => MathsStatus != SubjectStatus.NotSpecified;
        public bool IsEnglishAdded => EnglishStatus != SubjectStatus.NotSpecified;
        public bool IsIndustryPlacementAdded => IndustryPlacementStatus != IpStatus.NotSpecified;
        public bool IsIndustryPlacementStillToBeCompleted => IndustryPlacementStatus == IpStatus.NotSpecified || IndustryPlacementStatus == IpStatus.NotCompleted;
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public bool IsPendingWithdrawal { get; set; }
        public NotificationBannerModel SuccessBanner { get; set; }

        #region Summary Header

        public SummaryItemModel SummaryTLevelStatus => new SummaryItemModel
        {
            Id = "tlevelstatus",
            Title = LearnerRecordDetailsContent.Title_TLevel_Status_Text,
            Value = TLevelStatusValue,
            HasTag = true,
            TagCssClass = TLevelStatusTagCssClass
        };

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = LearnerRecordDetailsContent.Uln_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = LearnerRecordDetailsContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryProviderName => new SummaryItemModel
        {
            Id = "providername",
            Title = LearnerRecordDetailsContent.Title_Provider_Ukprn_Name_Text,
            Value = string.Concat(ProviderName, " ", "(", ProviderUkprn.ToString(), ")"),
        };

        public SummaryItemModel SummaryProviderUkprn => new SummaryItemModel
        {
            Id = "providerukprn",
            Title = LearnerRecordDetailsContent.Title_Provider_Ukprn_Name_Text,
            Value = ProviderUkprn.ToString(),
        };

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = LearnerRecordDetailsContent.Title_TLevel_Text,
            Value = TlevelName
        };

        public SummaryItemModel SummaryStartYear => new SummaryItemModel
        {
            Id = "startyear",
            Title = LearnerRecordDetailsContent.Title_StartYear_Text,
            Value = StartYear,
            ActionText = LearnerRecordDetailsContent.Action_Text_Link_Change,
            RouteName = RouteConstants.ChangeStartYearClear,
            RouteAttributes = new Dictionary<string, string> { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryAoName => new SummaryItemModel
        {
            Id = "aoname",
            Title = LearnerRecordDetailsContent.Title_AoName_Text,
            Value = AwardingOrganisationName
        };

        public SummaryItemModel SummaryOverallResult => new SummaryItemModel
        {
            Id = "result",
            Title = LearnerRecordDetailsContent.Title_Result,
            Value = OverallResult ?? LearnerRecordDetailsContent.Label_Overall_Result_Not_Calculated
        };

        public SummaryItemModel SummaryPrintCertificateType => new()
        {
            Id = "printcertificatetype",
            Title = LearnerRecordDetailsContent.Title_CertificateType_Text,
            Value = PrintCertificateType.HasValue ? PrintCertificateType.Value.GetDisplayName() : string.Empty
        };

        public SummaryItemModel SummaryBatchId => new()
        {
            Id = "batchid",
            Title = LearnerRecordDetailsContent.Title_Batch_Id,
            Value = BatchId.HasValue ? BatchId.Value.ToString() : string.Empty
        };

        public SummaryItemModel SummaryPrintRequestSubmittedOn => new()
        {
            Id = "printrequestsubmittedon",
            Title = LearnerRecordDetailsContent.Title_Date_Of_Submission,
            Value = PrintRequestSubmittedOn.HasValue ? PrintRequestSubmittedOn.Value.ToDobFormat() : string.Empty
        };

        public SummaryItemModel SummaryPrintingBatchItemStatus => new()
        {
            Id = "printingbatchitemstatus",
            Title = LearnerRecordDetailsContent.Title_Batch_Status,
            Value = PrintingBatchItemStatus.HasValue && PrintingBatchItemStatus.Value != BatchItemStatus.NotSpecified ? PrintingBatchItemStatus?.GetDisplayName() : string.Empty
        };

        public SummaryItemModel SummaryPrintingBatchItemStatusChangedOn => new()
        {
            Id = "printingbatchitemstatuschangedon",
            Title = LearnerRecordDetailsContent.Title_Batch_Status_Changed_On,
            Value = PrintingBatchItemStatusChangedOn.HasValue ? PrintingBatchItemStatusChangedOn.Value.ToDobFormat() : string.Empty
        };

        public SummaryItemModel SummaryTrackingId => new()
        {
            Id = "trackingid",
            Title = LearnerRecordDetailsContent.Title_Tracking_Id,
            Value = !string.IsNullOrEmpty(TrackingId) ? TrackingId : string.Empty
        };

        #endregion

        # region Summary English & Maths

        public SummaryItemModel SummaryMathsStatus =>
            new SummaryItemModel
            {
                Id = "mathsstatus",
                Title = LearnerRecordDetailsContent.Title_Maths_Text,
                Value = GetSubjectStatus(MathsStatus),
            };

        public SummaryItemModel SummaryEnglishStatus =>
            new SummaryItemModel
            {
                Id = "englishstatus",
                Title = LearnerRecordDetailsContent.Title_English_Text,
                Value = GetSubjectStatus(EnglishStatus),
            };

        #endregion

        // Industry Placement
        public SummaryItemModel SummaryIndustryPlacementStatus =>
            new()
            {
                Id = "industryplacement",
                Title = LearnerRecordDetailsContent.Title_IP_Status_Text,
                Value = GetIndustryPlacementDisplayText,
                ActionText = LearnerRecordDetailsContent.Action_Text_Link_Change,
                RouteName = RouteConstants.AdminChangeIndustryPlacementClear,
                RouteAttributes = new Dictionary<string, string> { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() } },
                HiddenActionText = LearnerRecordDetailsContent.Hidden_Action_Text_Industry_Placement
            };

        public InformationBannerModel InformationBanner { get; set; }

        public AdminAssessmentDetailsViewModel AssessmentDetails { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminSearchLearnersRecords
        };

        private static string GetSubjectStatus(SubjectStatus subjectStatus)
        {
            return subjectStatus switch
            {
                SubjectStatus.Achieved => SubjectStatusContent.Achieved_Display_Text,
                SubjectStatus.NotAchieved => SubjectStatusContent.Not_Achieved_Display_Text,
                SubjectStatus.AchievedByLrs => SubjectStatusContent.Achieved_Lrs_Display_Text,
                SubjectStatus.NotAchievedByLrs => SubjectStatusContent.Not_Achieved_Lrs_Display_Text,
                _ => SubjectStatusContent.Not_Yet_Recevied_Display_Text,
            };
        }

        private string GetIndustryPlacementDisplayText => IndustryPlacementStatus switch
        {
            IpStatus.Completed => IndustryPlacementStatusContent.Completed_Display_Text,
            IpStatus.CompletedWithSpecialConsideration => IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text,
            IpStatus.NotCompleted => IndustryPlacementStatusContent.Still_To_Be_Completed_Display_Text,
            IpStatus.WillNotComplete => IndustryPlacementStatusContent.Placement_Will_Not_Be_Completed,
            _ => IndustryPlacementStatusContent.Not_Yet_Received_Display_Text,
        };

        private string TLevelStatusTagCssClass
            => RegistrationPathwayStatus == RegistrationPathwayStatus.Active && !IsPendingWithdrawal ? "govuk-tag--green" : "govuk-tag--blue";

        private string TLevelStatusValue
            => IsPendingWithdrawal ? LearnerRecordDetailsContent.TLevel_Status_Pending_Withdrawal_Text : RegistrationPathwayStatus.ToString();
    }
}
