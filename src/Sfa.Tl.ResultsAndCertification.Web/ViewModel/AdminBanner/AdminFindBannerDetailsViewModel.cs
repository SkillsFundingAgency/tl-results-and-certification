using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner
{
    public class AdminFindBannerDetailsViewModel
    {
        public string Content { get; set; }

        public string Target { get; set; }

        public bool Active { get; set; }

        public ChangeRecordModel BannerDetailsLink { get; set; }
    }
}