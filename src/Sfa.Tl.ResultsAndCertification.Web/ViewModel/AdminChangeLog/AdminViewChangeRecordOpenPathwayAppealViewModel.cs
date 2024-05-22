using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordOpenPathwayAppealViewModel : AdminViewChangeRecordViewModel
    {
        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
          => CreateSummaryItemModel(AdminViewChangeRecord.Summary_Exam_Period_Id, AdminViewChangeRecord.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminViewChangeRecord.Summary_Grade_Id,
               AdminViewChangeRecord.Summary_Grade_Text,
               Grade);

        public AdminReviewSummaryItemModel SummaryAppeal => new()
        {
            Id = AdminReviewCoreAppealChanges.Summary_Appeal_Id,
            Title = AdminReviewCoreAppealChanges.Change_Summary_Change,
            Value = AdminReviewCoreAppealChanges.Change_Summary_From,
            Value2 = AdminReviewCoreAppealChanges.Change_Summary_To
        };

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
            => new()
            {
                Id = id,
                Title = title,
                Value = value
            };
    };
}