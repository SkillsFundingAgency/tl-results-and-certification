using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner
{
    public class AdminEditBannerViewModel : AdminBannerBaseViewModel
    {
        public int BannerId { get; set; }

        public bool IsActive { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminBannerDetails,
            RouteAttributes = new Dictionary<string, string>
            {
                ["bannerId"] = BannerId.ToString()
            }
        };
    }
}