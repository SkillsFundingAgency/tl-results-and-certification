using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement
{
    public class AdminReviewChangesIndustryPlacementViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public AdminChangeIpViewModel AdminChangeIpViewModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangesIndustryPlacement), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(ReviewChangesIndustryPlacement), ErrorResourceName = "Validation_Date_When_Change_Requested_Blank_Text")]
        public string RequestDate => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangesIndustryPlacement), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminChangeIndustryPlacement,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public SummaryItemModel SummaryIndustryPlacement => new()
        {
            Id = "industryplacementstatus",
            Title = AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration ? ReviewChangesIndustryPlacement.Title_Industry_Placement_Status : ReviewChangesIndustryPlacement.Title_Status_Text,
            Value = AdminChangeIpViewModel.AdminIpCompletion.GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
            Value2 = AdminChangeIpViewModel.AdminIpCompletion.GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatusTo),
            TitleCss = AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration ? "govuk-summary-list__value" : default,
            ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
            RouteName = RouteConstants.AdminChangeIndustryPlacement,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public SummaryItemModel SummaryNoOfHours => new()
        {
            Id = "noofhours",
            Title = ReviewChangesIndustryPlacement.Title_Number_Of_Hours,
            Value = AdminChangeIpViewModel.AdminIpCompletion.GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
            Value2 = AdminChangeIpViewModel?.HoursViewModel?.Hours.ToString(),
            TitleCss = "govuk-summary-list__value",
            
            ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
            RouteName = RouteConstants.AdminIndustryPlacementSpecialConsiderationHours,
            RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public List<SummaryItemModel> SummaryIPReasonsList => AdminChangeIpViewModel?.ReasonsViewModel?.ReasonsList
           .Where(e => e.IsSelected).Select(e => new SummaryItemModel
           {
               Id = "ipreasonslist",
               Title = ReviewChangesIndustryPlacement.Title_Reasons_For_Reduced_Hours,
               Value = AdminChangeIpViewModel.AdminIpCompletion.GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
               Value2 = e.Name,
               TitleCss = "govuk-summary-list__value",
               ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
               RouteName = RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons,
               RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
           })
           .ToList();

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = ReviewChangesIndustryPlacement.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = ReviewChangesIndustryPlacement.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = ReviewChangesIndustryPlacement.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = ReviewChangesIndustryPlacement.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = ReviewChangesIndustryPlacement.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = ReviewChangesIndustryPlacement.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };
    }
}