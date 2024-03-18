using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordStartYearViewModel : AdminViewChangeRecordViewModel
    {
        public ChangeStartYearDetails ChangeStartYearDetails { get; set; }

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = AdminViewChangeRecord.Summary_Academic_Year_Id,
            Title = AdminViewChangeRecord.Title_StartYear,
            Value = $"{ChangeStartYearDetails.StartYearFrom} to {ChangeStartYearDetails.StartYearFrom + 1}",
            Value2 = $"{ChangeStartYearDetails.StartYearTo} to {ChangeStartYearDetails.StartYearTo + 1}"
        };
    };
}