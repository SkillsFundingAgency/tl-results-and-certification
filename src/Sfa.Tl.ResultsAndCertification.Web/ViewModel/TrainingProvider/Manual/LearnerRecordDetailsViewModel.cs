using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordDetailsViewModel
    {
        // Header
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int TlPathwayId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelTitle { get; set; }
        public int AcademicYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus MathsStatus { get; set; }
        public SubjectStatus EnglishStatus { get; set; }

        public int IndustryPlacementId { get; set; }
        public IpStatus IndustryPlacementStatus { get; set; }

        public OverallResultDetail OverallResultDetails { get; set; }
        public DateTime? OverallResultPublishDate { get; set; }

        // PrintCertificate Info
        public int? PrintCertificateId { get; set; }
        public PrintCertificateType? PrintCertificateType { get; set; }
        public AddressViewModel ProviderAddress { get; set; }

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

        public bool DisplayOverallResults => OverallResultDetails != null && OverallResultPublishDate.HasValue && DateTime.UtcNow >= OverallResultPublishDate;
        public NotificationBannerModel SuccessBanner { get; set; }
        public DateTime? LastDocumentRequestedDate { get; set; }
        public string LastDocumentRequestedDateDisplayValue { get { return LastDocumentRequestedDate.HasValue ? LastDocumentRequestedDate.Value.ToFormat() : string.Empty; } }
        public bool IsDocumentRerequestEligible { get; set; }
        public bool IsReprint { get; set; }

        #region Summary Header

        public SummaryItemModel SummaryTLevelStatus => new SummaryItemModel
        {
            Id = "tlevelstatus",
            Title = LearnerRecordDetailsContent.Title_TLevel_Status_Text,
            Value = TLevelStatusValue,
            HasTag = true,
            TagCssClass = TLevelStatusTagCssClass,
            ActionText = TLevelStatusChangeLinkText,
            RouteName = RegistrationPathwayStatus == RegistrationPathwayStatus.Active ? RouteConstants.AddWithdrawnStatus : string.Empty,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
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
            Title = LearnerRecordDetailsContent.Title_Provider_Name_Text,
            Value = ProviderName,
        };

        public SummaryItemModel SummaryProviderUkprn => new SummaryItemModel
        {
            Id = "providerukprn",
            Title = LearnerRecordDetailsContent.Title_Provider_Ukprn_Text,
            Value = ProviderUkprn.ToString(),
        };

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = LearnerRecordDetailsContent.Title_TLevel_Text,
            Value = TlevelTitle
        };

        public SummaryItemModel SummaryStartYear => new SummaryItemModel
        {
            Id = "startyear",
            Title = LearnerRecordDetailsContent.Title_StartYear_Text,
            Value = StartYear
        };

        public SummaryItemModel SummaryAoName => new SummaryItemModel
        {
            Id = "aoname",
            Title = LearnerRecordDetailsContent.Title_AoName_Text,
            Value = AwardingOrganisationName
        };

        #endregion

        # region Summary English & Maths
        public SummaryItemModel SummaryMathsStatus => IsMathsAdded ?
            new SummaryItemModel
            {
                Id = "mathsstatus",
                Title = LearnerRecordDetailsContent.Title_Maths_Text,
                Value = GetSubjectStatus(MathsStatus),
            }
            : new SummaryItemModel
            {
                Id = "mathsstatus",
                Title = LearnerRecordDetailsContent.Title_Maths_Text,
                Value = GetSubjectStatus(MathsStatus),
                ActionText = LearnerRecordDetailsContent.Action_Text_Link_Add,
                RouteName = IsMathsAdded ? string.Empty : RouteConstants.AddMathsStatus,
                RouteAttributes = IsMathsAdded ? null : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } },
                HiddenActionText = LearnerRecordDetailsContent.Hidden_Action_Text_Maths
            };

        public SummaryItemModel SummaryEnglishStatus => IsEnglishAdded ?
            new SummaryItemModel
            {
                Id = "englishstatus",
                Title = LearnerRecordDetailsContent.Title_English_Text,
                Value = GetSubjectStatus(EnglishStatus),
            }
            : new SummaryItemModel
            {
                Id = "englishstatus",
                Title = LearnerRecordDetailsContent.Title_English_Text,
                Value = GetSubjectStatus(EnglishStatus),
                ActionText = LearnerRecordDetailsContent.Action_Text_Link_Add,
                HiddenActionText = LearnerRecordDetailsContent.Hidden_Action_Text_English,
                RouteName = IsEnglishAdded ? string.Empty : RouteConstants.AddEnglishStatus,
                RouteAttributes = IsEnglishAdded ? null : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } },
            };

        #endregion

        // Industry Placement
        public SummaryItemModel SummaryIndustryPlacementStatus =>
            new SummaryItemModel
            {
                Id = "industryplacement",
                Title = LearnerRecordDetailsContent.Title_IP_Status_Text,
                Value = GetIndustryPlacementDisplayText,
                ActionText = IsIndustryPlacementAdded ? LearnerRecordDetailsContent.Action_Text_Link_Change : LearnerRecordDetailsContent.Action_Text_Link_Add,
                RouteName = IsIndustryPlacementAdded ? RouteConstants.ChangeIndustryPlacement : RouteConstants.AddIndustryPlacement,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } },
                HiddenActionText = LearnerRecordDetailsContent.Hidden_Action_Text_Industry_Placement
            };

        // Overall Result

        public SummaryItemModel SummaryCoreResult => new SummaryItemModel
        {
            Id = "coreResult",
            Title = OverallResultDetails?.PathwayName,
            Value = OverallResultDetails?.PathwayResult
        };

        public SummaryItemModel SummarySpecialismResult => new SummaryItemModel
        {
            Id = "specialismResult",
            Title = HasSpecialismInfo ? OverallResultDetails.SpecialismDetails.FirstOrDefault().SpecialismName : null,
            Value = HasSpecialismInfo ? OverallResultDetails.SpecialismDetails.FirstOrDefault().SpecialismResult : null
        };

        public SummaryItemModel SummaryOverallResult => new SummaryItemModel
        {
            Id = "overallResult",
            Title = LearnerRecordDetailsContent.Title_OverallResult_Text,
            Value = OverallResultDetails?.OverallResult
        };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.SearchLearnerRecord
        };

        public InformationBannerModel InformationBanner { get; set; }

        private bool HasSpecialismInfo => DisplayOverallResults && OverallResultDetails.SpecialismDetails != null && OverallResultDetails.SpecialismDetails.Any();

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

        private string TLevelStatusChangeLinkText
        {
            get
            {
                bool shouldShowTLevelStatusChangeLink = IsPendingWithdrawal || RegistrationPathwayStatus == RegistrationPathwayStatus.Active;
                return shouldShowTLevelStatusChangeLink ? LearnerRecordDetailsContent.Action_Text_Link_Change : string.Empty;
            }
        }
    }
}