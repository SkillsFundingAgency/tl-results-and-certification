using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminReviewRemoveCoreAssessmentEntryViewModel
    {
        public AdminRemovePathwayAssessmentEntryViewModel PathwayAssessmentViewModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewRemoveAssessmentEntry), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(AdminReviewRemoveAssessmentEntry))]
        public string RequestDate => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminReviewRemoveAssessmentEntry), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.RemoveAssessmentEntryCore,
            RouteAttributes = new Dictionary<string, string>() { {
                    Constants.RegistrationPathwayId, PathwayAssessmentViewModel.RegistrationPathwayId.ToString() },
                { Constants.AssessmentId, PathwayAssessmentViewModel.PathwayAssessmentId.ToString() } }
        };

        public SummaryItemModel SummaryAssessmentYear => new()
        {
            Id = "assessmentyear",
            Title = string.Format(AdminReviewRemoveAssessmentEntry.Label_Core_Component,PathwayAssessmentViewModel.PathwayName),
            Value = PathwayAssessmentViewModel.ExamPeriod,
            Value2 = string.Format(AdminReviewRemoveAssessmentEntry.Label_No_Assessment_Entry_Recorded, PathwayAssessmentViewModel.ExamPeriod),
            TitleCss = "govuk-summary-list__value",
            ActionText = AdminReviewRemoveAssessmentEntry.Link_Change_Text,
            RouteName = RouteConstants.RemoveAssessmentEntryCore,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, PathwayAssessmentViewModel.RegistrationPathwayId.ToString() }, 
                { Constants.AssessmentId, PathwayAssessmentViewModel.PathwayAssessmentId.ToString() } }
        };

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = AdminReviewRemoveAssessmentEntry.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = AdminReviewRemoveAssessmentEntry.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = AdminReviewRemoveAssessmentEntry.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = AdminReviewRemoveAssessmentEntry.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = AdminReviewRemoveAssessmentEntry.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = AdminReviewRemoveAssessmentEntry.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };
    }
}