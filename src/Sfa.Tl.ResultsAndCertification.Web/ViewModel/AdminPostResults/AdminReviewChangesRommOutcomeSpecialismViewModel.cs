using Microsoft.AspNetCore.Routing;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminReviewChangesRommOutcomeSpecialismViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int SpecialismAssessmentId { get; set; }

        public int SpecialismResultId { get; set; }

        public string SpecialismName { get; set; }

        public int SelectedGradeId { get; set; }

        public string SelectedGradeValue { get; set; }

        public string Grade { get; set; }

        public bool IsSameGrade { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_Learner_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_ULN_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_Provider_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_TLevel_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_StartYear_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminReviewChangesRommOutcomeSpecialism.Summary_Exam_Period_Id, AdminReviewChangesRommOutcomeSpecialism.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminReviewChangesRommOutcomeSpecialism.Summary_Grade_Id,
               AdminReviewChangesRommOutcomeSpecialism.Summary_Grade_Text,
               Grade);

        #endregion

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminReviewChangesRommOutcomeSpecialism.Summary_Selected_Grade_Id,
            Title = AdminReviewChangesRommOutcomeSpecialism.Summary_Grade_Text,
            Value = Grade,
            Value2 = IsSameGrade ? Grade : SelectedGradeValue,
            ActionText = AdminReviewChangesRommOutcomeSpecialism.Link_Change_Text,
            RouteName = IsSameGrade ? RouteConstants.AdminAddSpecialismRommOutcome : RouteConstants.AdminAddRommOutcomeChangeGradeSpecialism,
            RouteAttributes = new Dictionary<string, string>()
            {
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, SpecialismAssessmentId.ToString() }
            }
        };

        [Required(ErrorMessageResourceType = typeof(AdminReviewChangesRommOutcomeSpecialism), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminReviewChangesRommOutcomeSpecialism))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewChangesRommOutcomeSpecialism), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = IsSameGrade ? RouteConstants.AdminAddSpecialismRommOutcome : RouteConstants.AdminAddRommOutcomeChangeGradeSpecialism,
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