using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults
{
    public class AdminReviewChangesLevelTwoMathsViewModel
    {
        public AdminChangeResultsViewModel AdminChangeResultsViewModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangeLevelTwoMaths), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        public string RequestDate => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangeLevelTwoMaths), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminChangeLevelTwoMaths,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminChangeResultsViewModel.RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = ReviewChangeLevelTwoMaths.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = ReviewChangeLevelTwoMaths.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = ReviewChangeLevelTwoMaths.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = ReviewChangeLevelTwoMaths.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = ReviewChangeLevelTwoMaths.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = ReviewChangeLevelTwoMaths.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };

        public List<SummaryItemModel> GetMathsResultDetailsList()
        {
            var detailsList = new List<SummaryItemModel>();
            var mathsResult = AdminChangeResultsViewModel;

            // Status Row
            detailsList.Add(new()
            {
                Id = "mathsresultstatus",
                Title = ReviewChangeLevelTwoMaths.Title_Status_Text,
                Value = mathsResult.GetSubjectStatusDisplayText(mathsResult?.MathsStatus),
                Value2 = mathsResult.GetSubjectStatusDisplayText(mathsResult?.MathsStatusTo),
                TitleCss = default,
                ActionText = ReviewChangeLevelTwoMaths.Link_Change_Text,
                RouteName = RouteConstants.AdminChangeLevelTwoMaths,
                RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminChangeResultsViewModel.RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
            });

            return detailsList;
        }
    }
}