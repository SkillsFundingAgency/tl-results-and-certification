using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
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
            Value = GetSubjectStatusDisplayText(ChangeMathsStatusRequest.MathsStatusFrom),
            Value2 = GetSubjectStatusDisplayText(ChangeMathsStatusRequest.MathsStatusTo)
        };

        private static string GetSubjectStatusDisplayText(SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => AdminChangeMathsStatus.Status_Achieved_Text,
            SubjectStatus.NotAchieved => AdminChangeMathsStatus.Status_NotAchieved_Text,
            SubjectStatus.AchievedByLrs => AdminChangeMathsStatus.Status_AchievedByLrs_Text,
            SubjectStatus.NotAchievedByLrs => AdminChangeMathsStatus.Status_NotAchievedByLrs_Text,
            _ => AdminChangeMathsStatus.Status_Not_Yet_Received_Text
        };
    };
}