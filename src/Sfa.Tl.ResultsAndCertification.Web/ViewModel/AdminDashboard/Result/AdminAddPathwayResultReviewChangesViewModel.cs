﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result
{
    public class AdminAddPathwayResultReviewChangesViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public int PathwayAssessmentId { get; set; }

        public string PathwayName { get; set; }

        public int SelectedGradeId { get; set; }

        public string SelectedGradeValue { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_Learner_Id, AdminAddPathwayResultReviewChanges.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_ULN_Id, AdminAddPathwayResultReviewChanges.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_Provider_Id, AdminAddPathwayResultReviewChanges.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_TLevel_Id, AdminAddPathwayResultReviewChanges.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_StartYear_Id, AdminAddPathwayResultReviewChanges.Summary_StartYear_Text, StartYear);

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public SummaryItemModel SummaryExamPeriod
           => CreateSummaryItemModel(AdminAddPathwayResultReviewChanges.Summary_Exam_Period_Id, AdminAddPathwayResultReviewChanges.Summary_Exam_Period_Text, ExamPeriod);

        public SummaryItemModel SummaryGrade
           => CreateSummaryItemModel(
               AdminAddPathwayResult.Summary_Grade_Id,
               AdminAddPathwayResult.Summary_Grade_Text,
               AdminAddPathwayResult.No_Grade_Entered);

        #endregion

        public AdminReviewSummaryItemModel SummarySelectedGrade => new()
        {
            Id = AdminAddPathwayResultReviewChanges.Summary_Selected_Grade_Id,
            Title = AdminAddPathwayResultReviewChanges.Summary_Grade_Text,
            Value = AdminAddPathwayResultReviewChanges.No_Grade_Entered,
            Value2 = SelectedGradeValue,
            ActionText = AdminAddPathwayResultReviewChanges.Link_Change_Text,
            RouteName = RouteConstants.AdminAddPathwayResult,
            RouteAttributes = new Dictionary<string, string>()
            {
                { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, PathwayAssessmentId.ToString() }
            }
        };

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