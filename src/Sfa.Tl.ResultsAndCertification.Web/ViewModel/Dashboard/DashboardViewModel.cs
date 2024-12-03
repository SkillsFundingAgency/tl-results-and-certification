using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard
{
    public class DashboardViewModel
    {
        public bool HasAccessToService { get; set; }

        public LoginUserType LoginUserType { get; set; }

        public IEnumerable<DashboardBannerModel> Banners { get; set; }
    }
}