using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.CheckAndSubmit;
using EnglishAndMathsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsStatus;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class CheckAndSubmitViewModel
    {
        private Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
        private bool IsValidLearnerRecord => LearnerRecordModel?.LearnerRecord != null && LearnerRecordModel.LearnerRecord.IsLearnerRegistered == true
            && LearnerRecordModel.LearnerRecord.IsLearnerRecordAdded == false;
        private bool IsValidEnglishAndMaths => true;
        private bool IsValidUln => LearnerRecordModel?.Uln != null;
        private bool IsValidIndustryPlacement => false;
        private string GetEnglishAndMathsActionText => HasLrsEnglishAndMaths ? string.Empty : CheckAndSubmitContent.Change_Action_Link_Text;
        private string GetEnglishAndMathsStatusText => HasLrsEnglishAndMaths ? GetLrsEnglishAndMathsStatusDisplayText : GetEnglishAndMathsStatusDisplayText;
        private string GetEnglishAndMathsRouteName => "sdf";
        private Dictionary<string, string> GetEnglishAndMathsRouteAttributes => HasLrsEnglishAndMaths ? null : ChangeLinkRouteAttributes;
        private string GetLrsEnglishAndMathsStatusDisplayText
        {
            get
            {
                if (!HasLrsEnglishAndMaths)
                    return null;

                if (LearnerRecordModel?.LearnerRecord?.IsEnglishAndMathsAchieved == true && LearnerRecordModel?.LearnerRecord?.IsSendLearner == true)
                {
                    return CheckAndSubmitContent.English_And_Maths_Achieved_With_Send_Lrs_Text;
                }
                else
                {
                    return LearnerRecordModel?.LearnerRecord?.IsEnglishAndMathsAchieved == true && LearnerRecordModel?.LearnerRecord?.IsSendLearner == null
                        ? CheckAndSubmitContent.English_And_Maths_Achieved_Lrs_Text
                        : CheckAndSubmitContent.English_And_Maths_Not_Achieved_Lrs_Text;
                }
            }
        }
        private string GetEnglishAndMathsStatusDisplayText
        {
            get
            {
                return "asdf";
            }
        }
        private string GetIndustryPlacementDisplayText
        {
            get
            {
                return (IndustryPlacementStatus.Completed) switch
                {
                    IndustryPlacementStatus.Completed => IndustryPlacementStatusContent.Completed_Display_Text,
                    IndustryPlacementStatus.CompletedWithSpecialConsideration => IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text,
                    IndustryPlacementStatus.NotCompleted => IndustryPlacementStatusContent.NotCompleted_Display_Text,
                    _ => string.Empty,
                };
            }
        }

        public AddLearnerRecordViewModel LearnerRecordModel { get; set; }

        public bool IsCheckAndSubmitPageValid => IsValidLearnerRecord && IsValidEnglishAndMaths && IsValidUln && IsValidIndustryPlacement;

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = CheckAndSubmitContent.Title_Uln_Text,
            Value = LearnerRecordModel.Uln.EnterUln,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = CheckAndSubmitContent.Title_Name_Text,
            Value = LearnerRecordModel.LearnerRecord.Name,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = CheckAndSubmitContent.Title_DateofBirth_Text,
            Value = LearnerRecordModel.LearnerRecord.DateofBirth.ToShortDateString(),
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryProvider => new SummaryItemModel
        {
            Id = "provider",
            Title = CheckAndSubmitContent.Title_Provider_Text,
            Value = LearnerRecordModel.LearnerRecord.ProviderName,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = CheckAndSubmitContent.Title_Core_Text,
            Value = LearnerRecordModel.LearnerRecord.PathwayName,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryEnglishAndMathsStatus => new SummaryItemModel
        {
            Id = "englishmathsstatus",
            Title = CheckAndSubmitContent.Title_EnglishAndMaths_Status_Text,
            Value = GetEnglishAndMathsStatusText,
            ActionText = GetEnglishAndMathsActionText,
            RouteName = GetEnglishAndMathsRouteName,
            RenderActionColumn = !HasLrsEnglishAndMaths,
            RouteAttributes = GetEnglishAndMathsRouteAttributes,
            NeedBorderBottomLine = false,
            RenderHiddenActionText = !HasLrsEnglishAndMaths,
            HiddenActionText = HasLrsEnglishAndMaths ? string.Empty : CheckAndSubmitContent.English_And_Maths_Action_Hidden_Text
        };

        public SummaryItemModel SummaryWhatsLrsText => new SummaryItemModel
        {
            Id = "whatslrstext",
            Title = string.Empty,
            Value = CheckAndSubmitContent.Whats_Lrs_Text,
            RenderActionColumn = false,
            NeedBorderBottomLine = false,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryIndustryPlacementStatus => new SummaryItemModel
        {
            Id = "industryplacementstatus",
            Title = CheckAndSubmitContent.Title_IP_Status_Text,
            Value = GetIndustryPlacementDisplayText,
            ActionText = CheckAndSubmitContent.Change_Action_Link_Text,
            RouteAttributes = ChangeLinkRouteAttributes,
            NeedBorderBottomLine = false,
            RenderHiddenActionText = true,
            HiddenActionText = CheckAndSubmitContent.Industry_Placement_Action_Hidden_Text
        };

        public BackLinkModel BackLink => new BackLinkModel { RouteName = "" };


        public bool HasLrsEnglishAndMaths => LearnerRecordModel?.LearnerRecord != null && LearnerRecordModel.LearnerRecord.HasLrsEnglishAndMaths;

        public AddLearnerRecordViewModel ResetChangeMode()
        {

            return LearnerRecordModel;
        }
    }
}