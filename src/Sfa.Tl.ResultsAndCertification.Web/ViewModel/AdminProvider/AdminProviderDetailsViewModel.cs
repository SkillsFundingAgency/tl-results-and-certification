using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider
{
    public class AdminProviderDetailsViewModel
    {
        public int ProviderId { get; set; }

        public string ProviderName { get; set; }

        public SummaryItemModel SummaryUkprn { get; set; }

        public SummaryItemModel SummaryName { get; set; }

        public SummaryItemModel SummaryDisplayName { get; set; }

        public SummaryItemModel SummaryIsActive { get; set; }

        public NotificationBannerModel SuccessBanner { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindProvider
        };
    }
}