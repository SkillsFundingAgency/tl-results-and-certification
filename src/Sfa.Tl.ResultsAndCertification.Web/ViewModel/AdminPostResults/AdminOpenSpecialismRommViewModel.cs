using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminOpenSpecialismRommViewModel
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
            => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_Learner_Id, AdminOpenSpecialismRomm.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_ULN_Id, AdminOpenSpecialismRomm.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_Provider_Id, AdminOpenSpecialismRomm.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_TLevel_Id, AdminOpenSpecialismRomm.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_StartYear_Id, AdminOpenSpecialismRomm.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminOpenSpecialismRomm.Summary_Exam_Period_Id, AdminOpenSpecialismRomm.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminOpenSpecialismRomm.Summary_Grade_Id,
               AdminOpenSpecialismRomm.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminOpenSpecialismRomm.No_Grade_Entered : Grade);

        #endregion

        [Required(ErrorMessageResourceType = typeof(AdminOpenSpecialismRomm), ErrorMessageResourceName = "Validation_Message")]
        public bool? DoYouWantToOpenRomm { get; set; }

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