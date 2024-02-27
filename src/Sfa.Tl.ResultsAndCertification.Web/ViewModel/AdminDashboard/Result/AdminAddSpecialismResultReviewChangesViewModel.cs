using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result
{
    public class AdminAddSpecialismResultReviewChangesViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int SpecialismAssessmentId { get; set; }

        public string SpecialismName { get; set; }

        public int SelectedGradeId { get; set; }

        public string SelectedGradeValue { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminAddPathwayResult.Summary_Learner_Id, AdminAddPathwayResult.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminAddPathwayResult.Summary_ULN_Id, AdminAddPathwayResult.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminAddPathwayResult.Summary_Provider_Id, AdminAddPathwayResult.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminAddPathwayResult.Summary_TLevel_Id, AdminAddPathwayResult.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminAddPathwayResult.Summary_StartYear_Id, AdminAddPathwayResult.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddPathwayResult.Summary_Exam_Period_Id, AdminAddPathwayResult.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddPathwayResult.Summary_Grade_Id,
               AdminAddPathwayResult.Summary_Grade_Text,
               AdminAddPathwayResult.No_Grade_Entered);

        #endregion

        public SummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminAddSpecialismResultReviewChanges.Summary_Selected_Grade_Id,
            Title = AdminAddSpecialismResultReviewChanges.Summary_Grade_Text,
            Value = AdminAddSpecialismResultReviewChanges.No_Grade_Entered,
            Value2 = SelectedGradeValue,
            ActionText = AdminAddSpecialismResultReviewChanges.Link_Change_Text,
            RouteName = RouteConstants.AdminAddSpecialismResult,
            RouteAttributes = new Dictionary<string, string>()
            {
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, SpecialismAssessmentId.ToString() }
            }
        };

        [Required(ErrorMessageResourceType = typeof(AdminAddSpecialismResultReviewChanges), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(DateOfRequest), ErrorResourceType = typeof(AdminAddSpecialismResultReviewChanges))]
        public string DateOfRequest
            => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminAddSpecialismResultReviewChanges), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminAddPathwayResult,
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