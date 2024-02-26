using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result
{
    public class AdminAddSpecialismResultViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int SpecialismAssessmentId { get; set; }

        public string SpecialismName { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Learner_Id, RemoveAssessmentEntryCore.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_ULN_Id, RemoveAssessmentEntryCore.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Provider_Id, RemoveAssessmentEntryCore.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_TLevel_Id, RemoveAssessmentEntryCore.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_StartYear_Id, RemoveAssessmentEntryCore.Summary_StartYear_Text, StartYear);

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
             => new()
             {
                 Id = id,
                 Title = title,
                 Value = value
             };

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddSpecialismResult.Summary_Exam_Period_Id, AdminAddSpecialismResult.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddSpecialismResult.Summary_Grade_Id,
               AdminAddSpecialismResult.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminAddPathwayResult.No_Grade_Entered : Grade);

        #endregion

        [Required(ErrorMessageResourceType = typeof(AdminAddSpecialismResult), ErrorMessageResourceName = "Validation_Message")]
        public string SelectedGradeCode { get; set; }

        public List<LookupViewModel> Grades { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };
    }
}