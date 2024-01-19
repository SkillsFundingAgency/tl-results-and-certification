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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public string Learner => $"{FirstName} {LastName}";
        public AdminChangeIpViewModel AdminChangeIpViewModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangesIndustryPlacement), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(ReviewChangesIndustryPlacement), ErrorResourceName = "Validation_Date_When_Change_Requested_Blank_Text")]
        public string RequestDate => $"{Day}/{Month}/{Year}";
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

        #region Summary

        public SummaryItemModel SummaryIndustryPlacement => new()
        {
            Id = "industryplacementstatus",
            Title2 = AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration ? ReviewChangesIndustryPlacement.Title_Industry_Placement_Status : ReviewChangesIndustryPlacement.Title_Status_Text,
            Value = GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
            Value2 = GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatusTo),
            ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
            RouteName = RouteConstants.AdminChangeIndustryPlacement,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public SummaryItemModel SummaryNoOfHours => new()
        {
            Id = "noofhours",
            Title2 = ReviewChangesIndustryPlacement.Title_Number_Of_Hours,
            Value = GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
            Value2 = AdminChangeIpViewModel?.HoursViewModel?.Hours.ToString(),
            ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
            RouteName = RouteConstants.AdminIndustryPlacementSpecialConsiderationHours,
            RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public List<SummaryItemModel> SummaryIPReasonsList => AdminChangeIpViewModel?.ReasonsViewModel?.ReasonsList
           .Where(e => e.IsSelected).Select(e => new SummaryItemModel
           {
               Id = "ipreasonslist",
               Title2 = ReviewChangesIndustryPlacement.Title_Reasons_For_Reduced_Hours,
               Value = GetIndustryPlacementDisplayText(AdminChangeIpViewModel?.AdminIpCompletion?.IndustryPlacementStatus),
               Value2 = e.Name,
               ActionText = ReviewChangesIndustryPlacement.Link_Change_Text,
               RouteName = RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons,
               RouteAttributes = new Dictionary<string, string>() { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
           })
           .ToList();

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ReviewChangesIndustryPlacement.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ReviewChangesIndustryPlacement.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ReviewChangesIndustryPlacement.Title_Provider_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ReviewChangesIndustryPlacement.Title_TLevel_Text,
            Value = TlevelName
        };

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

        #endregion

        public string GetIndustryPlacementDisplayText(IpStatus? status) => status switch
        {
            IpStatus.Completed => AdminChangeIndustryPlacement.Status_Placement_Completed_Text,
            IpStatus.CompletedWithSpecialConsideration => AdminChangeIndustryPlacement.Status_Placement_Completed_With_Special_Consideration_Text,
            IpStatus.NotCompleted => AdminChangeIndustryPlacement.Status_Still_To_Be_Completed_Text,
            IpStatus.WillNotComplete => AdminChangeIndustryPlacement.Status_Placement_Will_Not_Be_Completed_Text,
            _ => AdminChangeIndustryPlacement.Status_Not_Yet_Recieved_Text,
        };
    }
}