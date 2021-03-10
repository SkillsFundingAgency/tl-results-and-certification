using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class CheckAndSubmitViewModel
    {
        public AddLearnerRecordViewModel LearnerRecordModel { get; set; }

        public bool IsCheckAndSubmitPageValid => LearnerRecordModel != null && LearnerRecordModel.Uln != null && LearnerRecordModel.IndustryPlacementQuestion != null;

        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = LearnerRecordModel.Uln.EnterUln, NeedBorderBottomLine = false  };
        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = CheckAndSubmitContent.Title_Name_Text, Value = LearnerRecordModel.LearnerRecord.Name, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = CheckAndSubmitContent.Title_DateofBirth_Text, Value = LearnerRecordModel.LearnerRecord.DateofBirth.ToShortDateString(), NeedBorderBottomLine = false };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = LearnerRecordModel.LearnerRecord.ProviderName, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryEnglishAndMathsStatus => new SummaryItemModel { Id = "englishmathsstatus", Title = CheckAndSubmitContent.Title_EnglishAndMaths_Status_Text, Value = GetEnglishAndMathsStatusText, RenderActionColumn = false, NeedBorderBottomLine = false };
        public SummaryItemModel SummaryWhatsLrsText => new SummaryItemModel { Id = "whatslrstext", Title = "", Value = CheckAndSubmitContent.Whats_Lrs_Text, RenderActionColumn = false, NeedBorderBottomLine = false, IsRawHtml = true };
        public SummaryItemModel SummaryIndustryPlacementStatus => new SummaryItemModel { Id = "industryplacementstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = EnumExtensions.GetDisplayName<IndustryPlacementStatus>(LearnerRecordModel.IndustryPlacementQuestion.IndustryPlacementStatus), ActionText = CheckAndSubmitContent.Change_Action_Link_Text, NeedBorderBottomLine = false };

        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddIndustryPlacementQuestion };

        private string GetEnglishAndMathsStatusText => LearnerRecordModel.LearnerRecord.IsEnglishAndMathsAchieved ? CheckAndSubmitContent.English_And_Maths_Achieved_Lrs_Text : CheckAndSubmitContent.English_And_Maths_Not_Achieved_Lrs_Text;
    }
}