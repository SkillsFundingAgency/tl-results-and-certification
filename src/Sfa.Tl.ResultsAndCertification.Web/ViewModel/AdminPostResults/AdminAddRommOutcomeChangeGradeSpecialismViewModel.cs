using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminAddRommOutcomeChangeGradeSpecialismViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int SpecialismAssessmentId { get; set; }

        public int SpecialismResultId { get; set; }

        public string SpecialismName { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public bool IsRemoveResult { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_Learner_Id, AdminAddRommOutcomeChangeGradeCore.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_ULN_Id, AdminAddRommOutcomeChangeGradeCore.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_Provider_Id, AdminAddRommOutcomeChangeGradeCore.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_TLevel_Id, AdminAddRommOutcomeChangeGradeCore.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_StartYear_Id, AdminAddRommOutcomeChangeGradeCore.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public string GradeCode { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddRommOutcomeChangeGradeCore.Summary_Exam_Period_Id, AdminAddRommOutcomeChangeGradeCore.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddRommOutcomeChangeGradeCore.Summary_Grade_Id,
               AdminAddRommOutcomeChangeGradeCore.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminAddRommOutcomeChangeGradeCore.No_Grade_Entered : Grade);

        #endregion

        [Required(ErrorMessageResourceType = typeof(AdminAddRommOutcomeChangeGradeCore), ErrorMessageResourceName = "Validation_Message")]
        public int? SelectedGradeId { get; set; }

        public string SelectedGradeValue
            => Grades?.FirstOrDefault(g => g.Id == SelectedGradeId)?.Value;

        public List<LookupViewModel> Grades { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminAddSpecialismRommOutcome,
            RouteAttributes = new Dictionary<string, string> 
            { 
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, SpecialismAssessmentId.ToString() } 
            }
        };

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
            => new()
            {
                Id = id,
                Title = title,
                Value = value
            };
    }
}
