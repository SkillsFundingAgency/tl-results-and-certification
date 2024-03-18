using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordAddSpecialismResultViewModel : AdminViewChangeRecordViewModel
    {
        public AddSpecialismResultRequest SpecialismDetails { get; set; }

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
          => CreateSummaryItemModel(AdminViewChangeRecord.Summary_Exam_Period_Id, AdminViewChangeRecord.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminViewChangeRecord.Summary_Grade_Id,
               AdminViewChangeRecord.Summary_Grade_Text,
               AdminViewChangeRecord.No_Grade_Entered);

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminViewChangeRecord.Summary_Selected_Grade_Id,
            Title = AdminViewChangeRecord.Summary_Grade_Text,
            Value = AdminViewChangeRecord.No_Grade_Entered,
            Value2 = SpecialismDetails.GradeTo
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