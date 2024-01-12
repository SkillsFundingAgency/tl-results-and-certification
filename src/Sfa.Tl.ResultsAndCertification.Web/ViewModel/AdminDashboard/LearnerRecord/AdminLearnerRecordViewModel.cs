using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord
{
    public class AdminLearnerRecordViewModel
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
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus MathsStatus { get; set; }
        public SubjectStatus EnglishStatus { get; set; }

        public int IndustryPlacementId { get; set; }
        public IpStatus IndustryPlacementStatus { get; set; }


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
            RouteName = RouteConstants.ChangeStartYear,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryAoName => new SummaryItemModel
        {
            Id = "aoname",
            Title = LearnerRecordDetailsContent.Title_AoName_Text,
            Value = AwardingOrganisationName
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


        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminSearchLearnersRecords
        };

        public InformationBannerModel InformationBanner { get; set; }
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


        private string TLevelStatusChangeRouteName
        {
            get
            {
                string routeName = string.Empty;

                if (RegistrationPathwayStatus == RegistrationPathwayStatus.Active)
                {
                    routeName = IsPendingWithdrawal ? RouteConstants.ChangeBackToActiveStatus : RouteConstants.AddWithdrawnStatus;
                }

                return routeName;
            }
        }
    }
}
