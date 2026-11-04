using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordUlnViewModel : AdminViewChangeRecordViewModel
    {
        public ChangeUlnDetails ChangeUlnDetails { get; set; }

        public SummaryItemModel SummaryUlnChange => new()
        {
            Id = "ulnchange",
            Title = "ULN",
            Value = ChangeUlnDetails.UlnFrom.ToString(),
            Value2 = ChangeUlnDetails.UlnTo.ToString()
        };
    };
}