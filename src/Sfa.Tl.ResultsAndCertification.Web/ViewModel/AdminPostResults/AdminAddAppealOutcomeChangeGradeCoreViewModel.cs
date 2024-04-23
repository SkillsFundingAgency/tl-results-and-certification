using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminAddAppealOutcomeChangeGradeCoreViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int PathwayAssessmentId { get; set; }

        public int PathwayResultId { get; set; }

        public string PathwayName { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public bool IsRemoveResult { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_Learner_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_ULN_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_Provider_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_TLevel_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_StartYear_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public string GradeCode { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddAppealOutcomeChangeGradeCore.Summary_Exam_Period_Id, AdminAddAppealOutcomeChangeGradeCore.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddAppealOutcomeChangeGradeCore.Summary_Grade_Id,
               AdminAddAppealOutcomeChangeGradeCore.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminAddAppealOutcomeChangeGradeCore.No_Grade_Entered : Grade);

        #endregion

        [Required(ErrorMessageResourceType = typeof(AdminAddAppealOutcomeChangeGradeCore), ErrorMessageResourceName = "Validation_Message")]
        public int? SelectedGradeId { get; set; }

        public string SelectedGradeValue
            => Grades?.FirstOrDefault(g => g.Id == SelectedGradeId)?.Value;

        public List<LookupViewModel> Grades { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminAddCoreAppealOutcome,
            RouteAttributes = new Dictionary<string, string>
            {
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, PathwayAssessmentId.ToString() }
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
