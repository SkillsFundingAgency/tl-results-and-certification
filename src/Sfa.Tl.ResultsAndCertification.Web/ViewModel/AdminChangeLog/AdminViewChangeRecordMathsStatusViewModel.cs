using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordMathsStatusViewModel : AdminViewChangeRecordViewModel
    {
        public ReviewChangeMathsStatusRequest ChangeMathsStatusRequest { get; set; }

        public SummaryItemModel SummaryMathsStatus => new()
        {
            Id = AdminViewChangeRecord.Summary_Maths_Status_Id,
            Title = AdminViewChangeRecord.Title_Maths_Status,
            Value = ChangeMathsStatusRequest.MathsStatusFrom.ToDisplayText(),
            Value2 = ChangeMathsStatusRequest.MathsStatusTo.ToDisplayText()
        };
    };
}