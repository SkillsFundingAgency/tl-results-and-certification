using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordPathwayRommOutcomeViewModel : AdminViewChangeRecordViewModel
    {
        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public PathwayRommOutcomeDetails RommOutcomeDetails { get; set; }

        public SummaryItemModel SummaryExamPeriod
          => CreateSummaryItemModel(AdminViewChangeRecord.Summary_Exam_Period_Id, AdminViewChangeRecord.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminViewChangeRecord.Summary_Grade_Id,
               AdminViewChangeRecord.Summary_Grade_Text,
               RommOutcomeDetails.From);

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminReviewChangesRommOutcomeCore.Summary_Selected_Grade_Id,
            Title = AdminReviewChangesRommOutcomeCore.Summary_Grade_Text,
            Value = RommOutcomeDetails.From,
            Value2 = RommOutcomeDetails.To
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