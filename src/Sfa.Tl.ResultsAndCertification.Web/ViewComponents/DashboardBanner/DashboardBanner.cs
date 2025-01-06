using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner
{
    public class DashboardBanner : ViewComponent
    {
        public IViewComponentResult Invoke(DashboardBannerModel model)
            => View("~/ViewComponents/DashboardBanner/Default.cshtml", model);
    }
}
