using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordSpecialismAssessmentViewModel : AdminViewChangeRecordViewModel
    {
        public AddSpecialismDetails SpecialismDetails { get; set; }

        public SummaryItemModel SummaryAssessment => new()
        {
            Id = AdminViewChangeRecord.Summary_Assessment_Id,
            Title = $"{AdminViewChangeRecord.Occupational_Specialism}: {SpecialismName}",
            Value = $"{AdminViewChangeRecord.Text_No_Assement_Message} {SpecialismDetails.SpecialismAssessmentTo.ToLower()}",
            Value2 = SpecialismDetails.SpecialismAssessmentTo,
            TitleCss = "govuk-summary-list__value"
        };
    };
}