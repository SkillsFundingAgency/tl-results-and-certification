using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminAddCoreRommOutcomeViewModel
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

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_Learner_Id, AdminAddCoreRommOutcome.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_ULN_Id, AdminAddCoreRommOutcome.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_Provider_Id, AdminAddCoreRommOutcome.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_TLevel_Id, AdminAddCoreRommOutcome.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_StartYear_Id, AdminAddCoreRommOutcome.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddCoreRommOutcome.Summary_Exam_Period_Id, AdminAddCoreRommOutcome.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddCoreRommOutcome.Summary_Grade_Id,
               AdminAddCoreRommOutcome.Summary_Grade_Text,
               string.IsNullOrWhiteSpace(Grade) ? AdminAddCoreRommOutcome.No_Grade_Entered : Grade);

        #endregion

        public bool IsValid
            => string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminAddCoreRommOutcome), ErrorMessageResourceName = "Validation_Message")]
        public bool? WhatIsRommOutcome { get; set; }

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