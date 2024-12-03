using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner
{
    public class AdminAddBannerViewModel : AdminBannerBaseViewModel
    {
        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindBanner
        };
    }
}