using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminAppealSpecialismReviewChangesViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int SpecialismAssessmentId { get; set; }

        public int SpecialismResultId { get; set; }

        public string SpecialismName { get; set; }

        public string Grade { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_Learner_Id, AdminReviewSpecialismAppealChanges.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_ULN_Id, AdminReviewSpecialismAppealChanges.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_Provider_Id, AdminReviewSpecialismAppealChanges.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_TLevel_Id, AdminReviewSpecialismAppealChanges.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_StartYear_Id, AdminReviewSpecialismAppealChanges.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_Exam_Period_Id, AdminReviewSpecialismAppealChanges.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
          => CreateSummaryItemModel(AdminReviewSpecialismAppealChanges.Summary_Grade_Id, AdminReviewSpecialismAppealChanges.Summary_Grade_Text, Grade);

        #endregion

        #region Change summary

        public AdminReviewSummaryItemModel SummaryRomm
            => new()
            {
                Id = AdminReviewSpecialismAppealChanges.Summary_Romm_Id,
                Title = AdminReviewSpecialismAppealChanges.Change_Summary_Change,
                Value = AdminReviewSpecialismAppealChanges.Change_Summary_From,
                Value2 = AdminReviewSpecialismAppealChanges.Change_Summary_To,
                ActionText = AdminReviewSpecialismAppealChanges.Link_Change_Text,
                RouteName = RouteConstants.AdminOpenPathwayRomm,
                RouteAttributes = new Dictionary<string, string>()
                {
                    { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                    { Constants.AssessmentId, SpecialismAssessmentId.ToString() }
                }
            };

        #endregion

        #region Who has asked for this change?

        [Required(ErrorMessageResourceType = typeof(AdminReviewSpecialismAppealChanges), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminReviewSpecialismAppealChanges))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewSpecialismAppealChanges), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        #endregion

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminOpenPathwayRomm,
            RouteAttributes = new Dictionary<string, string>()
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