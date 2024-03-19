using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminOpenPathwayRommReviewChangesViewModel
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
            => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_Learner_Id, AdminOpenPathwayRommReviewChanges.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_ULN_Id, AdminOpenPathwayRommReviewChanges.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_Provider_Id, AdminOpenPathwayRommReviewChanges.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_TLevel_Id, AdminOpenPathwayRommReviewChanges.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_StartYear_Id, AdminOpenPathwayRommReviewChanges.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_Exam_Period_Id, AdminOpenPathwayRommReviewChanges.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
          => CreateSummaryItemModel(AdminOpenPathwayRommReviewChanges.Summary_Grade_Id, AdminOpenPathwayRommReviewChanges.Summary_Grade_Text, Grade);

        #endregion

        #region Change summary

        public AdminReviewSummaryItemModel SummaryRomm
            => new()
            {
                Id = AdminOpenPathwayRommReviewChanges.Summary_Romm_Id,
                Title = AdminOpenPathwayRommReviewChanges.Change_Summary_Change,
                Value = AdminOpenPathwayRommReviewChanges.Change_Summary_From,
                Value2 = AdminOpenPathwayRommReviewChanges.Change_Summary_To,
                ActionText = AdminOpenPathwayRommReviewChanges.Link_Change_Text,
                RouteName = RouteConstants.AdminOpenPathwayRomm,
                RouteAttributes = new Dictionary<string, string>()
                {
                    { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                    { Constants.AssessmentId, PathwayAssessmentId.ToString() }
                }
            };

        #endregion

        #region Who has asked for this change?

        [Required(ErrorMessageResourceType = typeof(AdminAddPathwayResultReviewChanges), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminAddPathwayResultReviewChanges))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminAddPathwayResultReviewChanges), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        #endregion

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminAddPathwayResult,
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