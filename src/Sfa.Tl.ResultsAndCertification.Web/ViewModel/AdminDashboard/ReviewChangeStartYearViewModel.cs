using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
//using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class ReviewChangeStartYearViewModel
    {
        public int PathwayId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string AcademicYearTo { get; set; }
        public string DisplayAcademicYear { get; set; }
        public string Learner => $"{FirstName} {LastName}";
        [Required(ErrorMessageResourceType = typeof(ErrorResource.ReviewChangeStartYear), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }
        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(ErrorResource.ReviewChangeStartYear), ErrorResourceName = "Validation_Date_When_Change_Requested_Blank_Text")]
        public string RequestDate => $"{Day}/{Month}/{Year}";
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorResource.ReviewChangeStartYear), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.ChangeStartYear,
            RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, PathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ReviewChangeStartYear.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ReviewChangeStartYear.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ReviewChangeStartYear.Title_Provider_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ReviewChangeStartYear.Title_TLevel_Text,
            Value = TlevelName
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = ReviewChangeStartYear.Title_StartYear_Text,
            Value = DisplayAcademicYear,
            Value2 = $"{AcademicYearTo} to {AcademicYearTo + 1}",
            ActionText = ReviewChangeStartYear.Link_Change_Text,
            RouteName = RouteConstants.ChangeStartYear,
            RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, PathwayId.ToString() } }
        };

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = ReviewChangeStartYear.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = ReviewChangeStartYear.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = ReviewChangeStartYear.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = ReviewChangeStartYear.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = ReviewChangeStartYear.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = ReviewChangeStartYear.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };
    }
}