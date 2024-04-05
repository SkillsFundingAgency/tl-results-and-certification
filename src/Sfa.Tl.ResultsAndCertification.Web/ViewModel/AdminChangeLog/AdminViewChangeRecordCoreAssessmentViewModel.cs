using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordCoreAssessmentViewModel : AdminViewChangeRecordViewModel
    {
        public AddCoreAssessmentDetails CoreAssessmentDetails { get; set; }

        public SummaryItemModel SummaryAssessment => new()
        {
            Id = AdminViewChangeRecord.Summary_Assessment_Id,
            Title = $"{AdminViewChangeRecord.Core_Component}: {PathwayName}",
            Value = $"{AdminViewChangeRecord.Text_No_Assement_Message} {CoreAssessmentDetails.CoreAssessmentTo.ToLower()}",
            Value2 = CoreAssessmentDetails.CoreAssessmentTo,
            TitleCss = "govuk-summary-list__value",
        };
    };
}