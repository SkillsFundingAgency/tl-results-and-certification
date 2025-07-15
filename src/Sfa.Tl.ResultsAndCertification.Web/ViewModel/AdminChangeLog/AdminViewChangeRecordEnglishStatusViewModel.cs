using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
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
            Value = GetSubjectStatusDisplayText(ChangeEnglishStatusRequest.EnglishStatusFrom),
            Value2 = GetSubjectStatusDisplayText(ChangeEnglishStatusRequest.EnglishStatusTo)
        };

        private static string GetSubjectStatusDisplayText(SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => AdminChangeEnglishStatus.Status_Achieved_Text,
            SubjectStatus.NotAchieved => AdminChangeEnglishStatus.Status_NotAchieved_Text,
            SubjectStatus.AchievedByLrs => AdminChangeEnglishStatus.Status_AchievedByLrs_Text,
            SubjectStatus.NotAchievedByLrs => AdminChangeEnglishStatus.Status_NotAchievedByLrs_Text,
            _ => AdminChangeEnglishStatus.Status_Not_Yet_Received_Text
        };
    };
}