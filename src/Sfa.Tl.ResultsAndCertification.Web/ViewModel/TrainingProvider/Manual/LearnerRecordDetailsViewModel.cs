using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;
using LearnerRecordDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string PathwayName { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool IsSendLearner { get; set; }
        public int IndustryPlacementId { get; set; }
        public IpStatus? IndustryPlacementStatus { get; set; }

        public SummaryItemModel SummaryEnglishAndMathsStatus => new SummaryItemModel
        {
            Id = "englishmathsstatus",
            Title = LearnerRecordDetailsContent.Title_EnglishAndMaths_Status_Text,
            Value = GetEnglishAndMathsStatusText,
            ActionText = GetEnglishAndMathsActionText,
            RouteName = GetEnglishAndMathsRouteName,            
            RouteAttributes = GetEnglishAndMathsRouteAttributes,
            NeedBorderBottomLine = false,
            RenderHiddenActionText = true,
            HiddenActionText = LearnerRecordDetailsContent.English_And_Maths_Action_Hidden_Text
        };

        public SummaryItemModel SummaryWhatsLrsText => new SummaryItemModel
        {
            Id = "whatslrstext",
            Title = string.Empty,
            Value = LearnerRecordDetailsContent.Whats_Lrs_Text,
            RenderActionColumn = false,
            NeedBorderBottomLine = false,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryIndustryPlacementStatus => new SummaryItemModel
        {
            Id = "industryplacementstatus",
            Title = LearnerRecordDetailsContent.Title_IP_Status_Text,
            Value = GetIndustryPlacementDisplayText,
            ActionText = LearnerRecordDetailsContent.Update_Action_Link_Text,
            RouteName = "",
            NeedBorderBottomLine = false,
            RenderHiddenActionText = true,
            HiddenActionText = LearnerRecordDetailsContent.Industry_Placement_Action_Hidden_Text
        };

        private string GetEnglishAndMathsStatusText => HasLrsEnglishAndMaths
            ? (IsEnglishAndMathsAchieved ? LearnerRecordDetailsContent.English_And_Maths_Achieved_Lrs_Text : LearnerRecordDetailsContent.English_And_Maths_Not_Achieved_Lrs_Text)
            : string.Empty;

        private string GetEnglishAndMathsActionText => HasLrsEnglishAndMaths ? LearnerRecordDetailsContent.Query_Action_Link_Text : string.Empty;

        private string GetEnglishAndMathsRouteName => HasLrsEnglishAndMaths ? string.Empty : string.Empty;
        private Dictionary<string, string> GetEnglishAndMathsRouteAttributes => HasLrsEnglishAndMaths ? null : new Dictionary<string, string>();

        private string GetIndustryPlacementDisplayText
        {
            get
            {
                return IndustryPlacementStatus switch
                {
                    IpStatus.Completed => IndustryPlacementStatusContent.Completed_Display_Text,
                    IpStatus.CompletedWithSpecialConsideration => IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text,
                    IpStatus.NotCompleted => IndustryPlacementStatusContent.NotCompleted_Display_Text,
                    _ => string.Empty,
                };
            }
        }

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Manage_Learner_Records, RouteName = RouteConstants.ManageLearnerRecordsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.SearchLearnerRecord },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learner_TLevel_Record }
                    }
                };
            }
        }
    }
}
