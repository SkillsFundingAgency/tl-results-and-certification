using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminReviewChangesSpecialismAssessmentViewModel
    {
        public AdminOccupationalSpecialismViewModel AdminOccupationalSpecialismViewModel { get; set; }
        public int RegistrationPathwayId { get; set; }


        [Required(ErrorMessageResourceType = typeof(ReviewChangeAssessment), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ReviewChangeAssessment), ErrorMessageResourceName = "Validation_Contact_Name_Max_Length")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(ReviewChangeAssessment), ErrorResourceName = "Validation_Date_When_Change_Requested_Blank_Text")]
        public string RequestDate => $"{Year}/{Month}/{Day}";
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string CoreChangeTo => AdminOccupationalSpecialismViewModel.AssessmentYearTo;
        public string SpecialismDisplayName => AdminOccupationalSpecialismViewModel.SpecialismAssessmentName; 

        [Required(ErrorMessageResourceType = typeof(ReviewChangeAssessment), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminOccupationalSpecialisAssessmentEntry,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminOccupationalSpecialismViewModel.RegistrationPathwayId.ToString() }, 
                                                                { Constants.SpecialismsId, AdminOccupationalSpecialismViewModel.SpecialismAssessmentId.ToString() } }
        };
        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ReviewChangeAssessment.Title_Learner_Text,
            Value = AdminOccupationalSpecialismViewModel.LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ReviewChangeAssessment.Title_ULN_Text,
            Value = AdminOccupationalSpecialismViewModel.Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ReviewChangeStartYear.Title_Provider_Text,
            Value = AdminOccupationalSpecialismViewModel.Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ReviewChangeStartYear.Title_TLevel_Text,
            Value = AdminOccupationalSpecialismViewModel.TlevelName
        };

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = ReviewChangeAssessment.Title_StartYear_Text,
            Value = AdminOccupationalSpecialismViewModel.DisplayStartYear
        };


        public SummaryItemModel SummaryAssessment => new()
        {
            Id = "assessment",
            Title = $"{ReviewChangeAssessment.Occupational_Specialism}:{SpecialismDisplayName}",
            Value = $"{ReviewChangeAssessment.Text_No_Assement_Message} {CoreChangeTo.ToLower()}",
            Value2 = CoreChangeTo,
            ActionText = ReviewChangeAssessment.Link_Change_Text,
            RouteName = RouteConstants.AdminOccupationalSpecialisAssessmentEntry,
            TitleCss = "govuk-summary-list__value",
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminOccupationalSpecialismViewModel.RegistrationPathwayId.ToString() }, 
                                                                { Constants.SpecialismsId, AdminOccupationalSpecialismViewModel.SpecialismAssessmentId.ToString() } }
        };

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = ReviewChangeAssessment.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = ReviewChangeAssessment.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = ReviewChangeAssessment.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = ReviewChangeAssessment.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = ReviewChangeAssessment.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = ReviewChangeAssessment.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };

    }
}
