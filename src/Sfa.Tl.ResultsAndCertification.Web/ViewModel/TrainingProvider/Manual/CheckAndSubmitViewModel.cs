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
        public AddLearnerRecordViewModel LearnerRecordModel { get; set; }

        public bool IsCheckAndSubmitPageValid => IsValidLearnerRecord && IsValidEnglishAndMaths && IsValidUln && IsValidIndustryPlacement;

        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = LearnerRecordModel.Uln.EnterUln, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = CheckAndSubmitContent.Title_Name_Text, Value = LearnerRecordModel.LearnerRecord.Name, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = CheckAndSubmitContent.Title_DateofBirth_Text, Value = LearnerRecordModel.LearnerRecord.DateofBirth.ToShortDateString(), NeedBorderBottomLine = false };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = LearnerRecordModel.LearnerRecord.ProviderName, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryEnglishAndMathsStatus => new SummaryItemModel { Id = "englishmathsstatus", Title = CheckAndSubmitContent.Title_EnglishAndMaths_Status_Text, Value = GetEnglishAndMathsStatusText, ActionText = GetEnglishAndMathsActionText, RouteName = GetEnglishAndMathsRouteName, RenderActionColumn = !HasLrsEnglishAndMaths, RouteAttributes = GetEnglishAndMathsRouteAttributes, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryWhatsLrsText => new SummaryItemModel { Id = "whatslrstext", Title = "", Value = CheckAndSubmitContent.Whats_Lrs_Text, RenderActionColumn = false, NeedBorderBottomLine = false, IsRawHtml = true };
        public SummaryItemModel SummaryIndustryPlacementStatus => new SummaryItemModel { Id = "industryplacementstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = GetIndustryPlacementDisplayText, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteName = RouteConstants.AddIndustryPlacementQuestion, RouteAttributes = ChangeLinkRouteAttributes, NeedBorderBottomLine = false };

        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddIndustryPlacementQuestion };

        private Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
        public bool HasLrsEnglishAndMaths => LearnerRecordModel?.LearnerRecord != null && LearnerRecordModel.LearnerRecord.HasLrsEnglishAndMaths;
        private bool IsValidLearnerRecord => LearnerRecordModel?.LearnerRecord != null && LearnerRecordModel.LearnerRecord.IsLearnerRegistered == true && LearnerRecordModel.LearnerRecord.IsLearnerRecordAdded == false;
        private bool IsValidEnglishAndMaths => LearnerRecordModel?.LearnerRecord != null && ((LearnerRecordModel.LearnerRecord.HasLrsEnglishAndMaths == true && LearnerRecordModel.EnglishAndMathsQuestion == null) || (LearnerRecordModel.LearnerRecord.HasLrsEnglishAndMaths == false && LearnerRecordModel.EnglishAndMathsQuestion != null));
        private bool IsValidUln => LearnerRecordModel?.Uln != null;
        private bool IsValidIndustryPlacement => LearnerRecordModel?.IndustryPlacementQuestion != null;        

        private string GetEnglishAndMathsActionText => HasLrsEnglishAndMaths ? string.Empty : CheckAndSubmitContent.Change_Action_Link_Text;
        private string GetEnglishAndMathsStatusText => HasLrsEnglishAndMaths ? LearnerRecordModel.LearnerRecord.IsEnglishAndMathsAchieved ? CheckAndSubmitContent.English_And_Maths_Achieved_Lrs_Text : CheckAndSubmitContent.English_And_Maths_Not_Achieved_Lrs_Text : GetEnglishAndMathsStatusDisplayText;

        private string GetEnglishAndMathsRouteName => HasLrsEnglishAndMaths ? string.Empty : RouteConstants.AddEnglishAndMathsQuestion;
        private Dictionary<string, string> GetEnglishAndMathsRouteAttributes => HasLrsEnglishAndMaths ? null : ChangeLinkRouteAttributes;
        private string GetEnglishAndMathsStatusDisplayText
        {
            get
            {
                return (LearnerRecordModel?.EnglishAndMathsQuestion?.EnglishAndMathsStatus) switch
                {
                    EnglishAndMathsStatus.Achieved => EnglishAndMathsStatusContent.Achieved_Display_Text,
                    EnglishAndMathsStatus.AchievedWithSend => EnglishAndMathsStatusContent.Achieved_With_Send_Display_Text,
                    EnglishAndMathsStatus.NotAchieved => EnglishAndMathsStatusContent.Not_Achieved_Display_Text,
                    _ => string.Empty,
                };
            }
        }

        private string GetIndustryPlacementDisplayText
        {
            get
            {
                return (LearnerRecordModel?.IndustryPlacementQuestion?.IndustryPlacementStatus) switch
                {
                    IndustryPlacementStatus.Completed => IndustryPlacementStatusContent.Completed_Display_Text,
                    IndustryPlacementStatus.CompletedWithSpecialConsideration => IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text,
                    IndustryPlacementStatus.NotCompleted => IndustryPlacementStatusContent.NotCompleted_Display_Text,
                    _ => string.Empty,
                };
            }
        }
    }
}