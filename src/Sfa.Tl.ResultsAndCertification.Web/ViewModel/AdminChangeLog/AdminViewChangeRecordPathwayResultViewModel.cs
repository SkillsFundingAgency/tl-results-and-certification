using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordPathwayResultViewModel : AdminViewChangeRecordViewModel
    {
        public ChangePathwayDetails PathwayResultDetails { get; set; }

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
          => CreateSummaryItemModel(AdminViewChangeRecord.Summary_Exam_Period_Id, AdminViewChangeRecord.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminViewChangeRecord.Summary_Grade_Id,
               AdminViewChangeRecord.Summary_Grade_Text,
               PathwayResultDetails.SelectedGradeFrom);

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminViewChangeRecord.Summary_Selected_Grade_Id,
            Title = AdminViewChangeRecord.Summary_Grade_Text,
            Value = PathwayResultDetails.SelectedGradeFrom,
            Value2 = PathwayResultDetails.SelectedGradeTo
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