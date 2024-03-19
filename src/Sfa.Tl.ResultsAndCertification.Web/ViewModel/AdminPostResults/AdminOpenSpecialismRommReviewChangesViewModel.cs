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
    public class AdminOpenSpecialismRommReviewChangesViewModel
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
            => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_Learner_Id, AdminOpenSpecialismRommReviewChanges.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_ULN_Id, AdminOpenSpecialismRommReviewChanges.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_Provider_Id, AdminOpenSpecialismRommReviewChanges.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_TLevel_Id, AdminOpenSpecialismRommReviewChanges.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_StartYear_Id, AdminOpenSpecialismRommReviewChanges.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_Exam_Period_Id, AdminOpenSpecialismRommReviewChanges.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
          => CreateSummaryItemModel(AdminOpenSpecialismRommReviewChanges.Summary_Grade_Id, AdminOpenSpecialismRommReviewChanges.Summary_Grade_Text, Grade);

        #endregion

        #region Change summary

        public AdminReviewSummaryItemModel SummaryRomm
            => new()
            {
                Id = AdminOpenSpecialismRommReviewChanges.Summary_Romm_Id,
                Title = AdminOpenSpecialismRommReviewChanges.Change_Summary_Change,
                Value = AdminOpenSpecialismRommReviewChanges.Change_Summary_From,
                Value2 = AdminOpenSpecialismRommReviewChanges.Change_Summary_To,
                ActionText = AdminOpenSpecialismRommReviewChanges.Link_Change_Text,
                RouteName = RouteConstants.AdminOpenPathwayRomm,
                RouteAttributes = new Dictionary<string, string>()
                {
                    { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                    { Constants.AssessmentId, SpecialismAssessmentId.ToString() }
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
            RouteName = RouteConstants.AdminAddSpecialismResult,
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