using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordRemoveCoreAssessmentViewModel : AdminViewChangeRecordViewModel
    {
        public DetailsChangeAssessmentRemove DetailsChangeAssessment { get; set; }

        public SummaryItemModel SummaryAssessmentYear => new()
        {
            Id = "assessmentyear",
            Title = string.Format(AdminViewChangeRecord.Label_Core_Component, DetailsChangeAssessment.PathwayName),
            Value = DetailsChangeAssessment.From,
            Value2 = string.Format(AdminViewChangeRecord.Label_No_Assessment_Entry_Recorded, DetailsChangeAssessment.From),
            TitleCss = "govuk-summary-list__value"
        };
    };
}