using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public class AdminNotificationDetailsViewModel
    {
        public int NotificationId { get; set; }

        public string Title { get; set; }

        public SummaryItemModel SummaryTarget { get; set; }

        public SummaryItemModel SummaryStartDate { get; set; }

        public SummaryItemModel SummaryEndDate { get; set; }

        public SummaryItemModel SummaryIsActive { get; set; }

        public string Content { get; set; }

        public NotificationBannerModel SuccessBanner { get; set; }

        public DashboardBannerModel DashboardBanner { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindNotification
        };
    }
}