using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.Linq;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminViewChangeRecordIndustryPlacementViewModel : AdminViewChangeRecordViewModel
    {
        public ChangeIndustryPlacementRequest ChangeIPDetails { get; set; }

        public IList<string> Reasons { get; set; }

        public List<SummaryItemModel> GetIpSummaryDetailsList()
        {
            var detailsList = new List<SummaryItemModel>();

            // Status Row
            detailsList.Add(new()
            {
                Id = AdminViewChangeRecord.Industry_Placement_Status_Id,
                Title = ChangeIPDetails.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration ? AdminViewChangeRecord.Title_Industry_Placement_Status : AdminViewChangeRecord.Title_Status_Text,
                Value = GetIndustryPlacementDisplayText(ChangeIPDetails.IndustryPlacementStatusFrom),
                Value2 = GetIndustryPlacementDisplayText(ChangeIPDetails.IndustryPlacementStatusTo),
                TitleCss = ChangeIPDetails.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration ? "govuk-summary-list__value" : default
            });

            if (ChangeIPDetails.IndustryPlacementStatusTo == IpStatus.CompletedWithSpecialConsideration)
            {
                // SpecialConsideration Rows
                AddSummaryItemForSpecialConsideration(detailsList);
            }

            return detailsList;
        }

        private bool AddSummaryItemForSpecialConsideration(List<SummaryItemModel> detailsList)
        {
            // Hours Row
            detailsList.Add(new()
            {
                Id = AdminViewChangeRecord.No_Of_Hours_Id,
                Title = AdminViewChangeRecord.Title_Number_Of_Hours,
                Value = GetIndustryPlacementDisplayText(ChangeIPDetails.IndustryPlacementStatusFrom),
                Value2 = ChangeIPDetails.HoursSpentOnPlacementTo.ToString(),
                TitleCss = "govuk-summary-list__value"
            });

            // Reasons Row
            detailsList.Add(new SummaryItemModel
            {
                Id = AdminViewChangeRecord.IP_Reasons_List_Id,
                Title = AdminViewChangeRecord.Title_Reasons_For_Reduced_Hours,
                Value = GetIndustryPlacementDisplayText(ChangeIPDetails.IndustryPlacementStatusTo),
                Value2 = ConvertListToRawHtmlString(Reasons),
                TitleCss = "govuk-summary-list__value"
            });

            return true;
        }
        private string GetIndustryPlacementDisplayText(IpStatus? status) => status switch
        {
            IpStatus.Completed => AdminChangeIndustryPlacement.Status_Placement_Completed_Text,
            IpStatus.CompletedWithSpecialConsideration => AdminChangeIndustryPlacement.Status_Placement_Completed_With_Special_Consideration_Text,
            IpStatus.NotCompleted => AdminChangeIndustryPlacement.Status_Still_To_Be_Completed_Text,
            IpStatus.WillNotComplete => AdminChangeIndustryPlacement.Status_Placement_Will_Not_Be_Completed_Text,
            _ => AdminChangeIndustryPlacement.Status_Not_Yet_Recieved_Text,
        };

        private static string ConvertListToRawHtmlString(IEnumerable<string> selectedList)
        {
            var htmlRawList = selectedList.Select(x => string.Format(AdminViewChangeRecord.Para_Item, x));
            return string.Join(string.Empty, htmlRawList);
        }
    }
}