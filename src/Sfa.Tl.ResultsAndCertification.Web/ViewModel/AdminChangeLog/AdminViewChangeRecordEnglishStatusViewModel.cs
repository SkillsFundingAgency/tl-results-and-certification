using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordEnglishStatusViewModel : AdminViewChangeRecordViewModel
    {
        public ReviewChangeEnglishStatusRequest ChangeEnglishStatusRequest { get; set; }

        public SummaryItemModel SummaryEnglishStatus => new()
        {
            Id = AdminViewChangeRecord.Summary_English_Status_Id,
            Title = AdminViewChangeRecord.Title_English_Status,
            Value = ChangeEnglishStatusRequest.EnglishStatusFrom.ToDisplayText(),
            Value2 = ChangeEnglishStatusRequest.EnglishStatusTo.ToDisplayText()
        };
    };
}