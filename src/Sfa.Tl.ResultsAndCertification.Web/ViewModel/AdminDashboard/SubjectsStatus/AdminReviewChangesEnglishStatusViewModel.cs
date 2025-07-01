using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus
{
    public class AdminReviewChangesEnglishStatusViewModel
    {
        public AdminChangeEnglishStatusViewModel AdminChangeStatusViewModel { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangesEnglishStatus), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        public string ContactName { get; set; }

        public string RequestDate => $"{Year}/{Month}/{Day}";

        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(ReviewChangesEnglishStatus), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        public string ChangeReason { get; set; }

        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminChangeEnglishStatus,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminChangeStatusViewModel.RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
        };

        public SummaryItemModel SummaryContactName => new()
        {
            Id = "contactname",
            Title = ReviewChangesEnglishStatus.Title_Contact_Name_Text,
            Value = ContactName
        };

        public SummaryItemModel SummaryDay => new()
        {
            Id = "dateofrequestday",
            Title = ReviewChangesEnglishStatus.Title_Day_Text,
            Value = Day
        };
        public SummaryItemModel SummaryMonth => new()
        {
            Id = "dateofrequestmonth",
            Title = ReviewChangesEnglishStatus.Title_Month_Text,
            Value = Month
        };
        public SummaryItemModel SummaryYear => new()
        {
            Id = "dateofrequestyear",
            Title = ReviewChangesEnglishStatus.Title_Year_Text,
            Value = Year
        };
        public SummaryItemModel SummaryChangeReason => new()
        {
            Id = "changereason",
            Title = ReviewChangesEnglishStatus.Title_Reason_For_Change_Text,
            Value = ChangeReason
        };
        public SummaryItemModel SummaryZendeskTicketId => new()
        {
            Id = "zendeskticketid",
            Title = ReviewChangesEnglishStatus.Title_Zendesk_Ticket_Id,
            Value = ZendeskId
        };

        public List<SummaryItemModel> GetEnglishStatusDetailsList()
        {
            var detailsList = new List<SummaryItemModel>();
            var englishStatus = AdminChangeStatusViewModel;

            // Status Row
            detailsList.Add(new()
            {
                Id = "englishresultstatus",
                Title = ReviewChangesMathsStatus.Title_Status_Text,
                Value = englishStatus.GetSubjectStatusDisplayText(englishStatus?.EnglishStatus),
                Value2 = englishStatus.GetSubjectStatusDisplayText(englishStatus?.EnglishStatusTo),
                TitleCss = default,
                ActionText = ReviewChangesMathsStatus.Link_Change_Text,
                RouteName = RouteConstants.AdminChangeMathsStatus,
                RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, AdminChangeStatusViewModel.RegistrationPathwayId.ToString() }, { Constants.IsBack, "true" } }
            });

            return detailsList;
        }
    }
}