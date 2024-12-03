using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner
{
    public class AdminBannerDetailsViewModel
    {
        public int BannerId { get; set; }

        public string Title { get; set; }

        public SummaryItemModel SummaryTarget { get; set; }

        public SummaryItemModel SummaryIsActive { get; set; }

        public SummaryItemModel SummaryStartDate { get; set; }

        public SummaryItemModel SummaryEndDate { get; set; }

        public string Content { get; set; }

        public NotificationBannerModel SuccessBanner { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindBanner
        };
    }
}