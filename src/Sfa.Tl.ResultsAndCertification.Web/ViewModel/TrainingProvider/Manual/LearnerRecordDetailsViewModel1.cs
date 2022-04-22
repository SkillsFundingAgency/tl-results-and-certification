using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;
using SubjectStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.SubjectStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordDetailsViewModel1
    {
        // Header
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }

        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelTitle { get; set; }
        public string StartYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus MathsStatus { get; set; }
        public SubjectStatus EnglishStatus { get; set; }
        
        public int IndustryPlacementId { get; set; } // TODO: upcoming story
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; } // TODO: upcoming story

        /// <summary>
        /// True when status is Active or Withdrawn
        /// </summary>
        public bool IsLearnerRegistered { get; set; }
        public bool IsStatusCompleted => IsMathsAdded && IsEnglishAdded && IsIndustryPlacementAdded;
        public bool IsIndustryPlacementAdded => IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified; // TODO: upcoming story
        public bool IsMathsAdded => MathsStatus != SubjectStatus.NotSpecified;
        public bool IsEnglishAdded => EnglishStatus != SubjectStatus.NotSpecified;
        public string StatusTag => CommonHelper.GetLearnerStatusTag(IsStatusCompleted);
        public NotificationBannerModel SuccessBanner { get; set; }

        #region Summary Header
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
                HiddenActionText = LearnerRecordDetailsContent.Hidden_Action_Text_English
            };

        #endregion

        // Industry Placement
        // Todo: Next Story. 

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.SearchLearnerRecord
                };
            }
        }

        private static string GetSubjectStatus(SubjectStatus subjectStatus)
        {
            switch (subjectStatus)
            {
                case SubjectStatus.Achieved:
                    return SubjectStatusContent.Achieved_Display_Text;
                case SubjectStatus.NotAchieved:
                    return SubjectStatusContent.Not_Achieved_Display_Text;
                case SubjectStatus.AchievedByLrs:
                    return SubjectStatusContent.Achieved_Lrs_Display_Text;
                case SubjectStatus.NotAchievedByLrs:
                    return SubjectStatusContent.Not_Achieved_Lrs_Display_Text;
                default:
                    return SubjectStatusContent.Not_Yet_Recevied_Display_Text;
            }
        }
    }
}
