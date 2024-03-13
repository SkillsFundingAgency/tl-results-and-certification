using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminOpenPathwayRommViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int PathwayAssessmentId { get; set; }

        public string PathwayName { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminOpenRomm.Summary_Learner_Id, AdminOpenRomm.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminOpenRomm.Summary_ULN_Id, AdminOpenRomm.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminOpenRomm.Summary_Provider_Id, AdminOpenRomm.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminOpenRomm.Summary_TLevel_Id, AdminOpenRomm.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminOpenRomm.Summary_StartYear_Id, AdminOpenRomm.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminOpenRomm.Summary_Exam_Period_Id, AdminOpenRomm.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminOpenRomm.Summary_Grade_Id,
               AdminOpenRomm.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminOpenRomm.No_Grade_Entered : Grade);

        #endregion

        [Required(ErrorMessageResourceType = typeof(AdminOpenRomm), ErrorMessageResourceName = "Validation_Message")]
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