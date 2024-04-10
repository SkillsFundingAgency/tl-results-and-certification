using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminAppealCoreReviewChangesViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int PathwayAssessmentId { get; set; }

        public int PathwayResultId { get; set; }

        public string PathwayName { get; set; }

        public string Grade { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_Learner_Id, AdminReviewCoreAppealChanges.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_ULN_Id, AdminReviewCoreAppealChanges.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_Provider_Id, AdminReviewCoreAppealChanges.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_TLevel_Id, AdminReviewCoreAppealChanges.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_StartYear_Id, AdminReviewCoreAppealChanges.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_Exam_Period_Id, AdminReviewCoreAppealChanges.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
          => CreateSummaryItemModel(AdminReviewCoreAppealChanges.Summary_Grade_Id, AdminReviewCoreAppealChanges.Summary_Grade_Text, Grade);

        #endregion

        #region Change summary

        public AdminReviewSummaryItemModel SummaryRomm
            => new()
            {
                Id = AdminReviewCoreAppealChanges.Summary_Romm_Id,
                Title = AdminReviewCoreAppealChanges.Change_Summary_Change,
                Value = AdminReviewCoreAppealChanges.Change_Summary_From,
                Value2 = AdminReviewCoreAppealChanges.Change_Summary_To,
                ActionText = AdminReviewCoreAppealChanges.Link_Change_Text,
                RouteName = RouteConstants.AdminOpenPathwayRomm,
                RouteAttributes = new Dictionary<string, string>()
                {
                    { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                    { Constants.AssessmentId, PathwayAssessmentId.ToString() }
                }
            };

        #endregion

        #region Who has asked for this change?

        [Required(ErrorMessageResourceType = typeof(AdminReviewCoreAppealChanges), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminReviewCoreAppealChanges))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewCoreAppealChanges), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        #endregion

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminOpenPathwayRomm,
            RouteAttributes = new Dictionary<string, string>()
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