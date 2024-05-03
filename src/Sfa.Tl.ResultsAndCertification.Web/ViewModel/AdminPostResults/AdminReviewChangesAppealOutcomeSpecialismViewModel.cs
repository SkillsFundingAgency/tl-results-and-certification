using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults
{
    public class AdminReviewChangesAppealOutcomeSpecialismViewModel
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
            => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_Learner_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_ULN_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_Provider_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_TLevel_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_StartYear_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminReviewChangesAppealOutcomeSpecialism.Summary_Exam_Period_Id, AdminReviewChangesAppealOutcomeSpecialism.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminReviewChangesAppealOutcomeSpecialism.Summary_Grade_Id,
               AdminReviewChangesAppealOutcomeSpecialism.Summary_Grade_Text,
               Grade);

        #endregion

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminReviewChangesAppealOutcomeSpecialism.Summary_Selected_Grade_Id,
            Title = AdminReviewChangesAppealOutcomeSpecialism.Summary_Grade_Text,
            Value = Grade,
            Value2 = IsSameGrade ? Grade : SelectedGradeValue,
            ActionText = AdminReviewChangesAppealOutcomeSpecialism.Link_Change_Text,
            RouteName = IsSameGrade ? RouteConstants.AdminAddSpecialismAppealOutcome : RouteConstants.AdminAddAppealOutcomeChangeGradeSpecialism,
            RouteAttributes = new Dictionary<string, string>()
            {
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, SpecialismAssessmentId.ToString() }
            }
        };

        [Required(ErrorMessageResourceType = typeof(AdminReviewChangesAppealOutcomeSpecialism), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminReviewChangesAppealOutcomeSpecialism))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewChangesAppealOutcomeSpecialism), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = IsSameGrade ? RouteConstants.AdminAddSpecialismAppealOutcome : RouteConstants.AdminAddAppealOutcomeChangeGradeSpecialism,
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