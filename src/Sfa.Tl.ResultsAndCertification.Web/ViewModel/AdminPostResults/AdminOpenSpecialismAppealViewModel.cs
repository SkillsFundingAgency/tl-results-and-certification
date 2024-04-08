using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminOpenSpecialismAppealViewModel
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

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_Learner_Id, AdminOpenSpecialismAppeal.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_ULN_Id, AdminOpenSpecialismAppeal.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_Provider_Id, AdminOpenSpecialismAppeal.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_TLevel_Id, AdminOpenSpecialismAppeal.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_StartYear_Id, AdminOpenSpecialismAppeal.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminOpenSpecialismAppeal.Summary_Exam_Period_Id, AdminOpenSpecialismAppeal.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminOpenSpecialismAppeal.Summary_Grade_Id,
               AdminOpenSpecialismAppeal.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminOpenSpecialismAppeal.No_Grade_Entered : Grade);

        #endregion

        public bool IsValid
            => string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminOpenSpecialismAppeal), ErrorMessageResourceName = "Validation_Message")]
        public bool? DoYouWantToOpenAppeal { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
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