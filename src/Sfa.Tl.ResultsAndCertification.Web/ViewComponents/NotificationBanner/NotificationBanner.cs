using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner
{
    public class NotificationBanner : ViewComponent
    {
        public IViewComponentResult Invoke(NotificationBannerModel model)
        {
            return View("~/ViewComponents/NotificationBanner/Default.cshtml", model);
        }
    }
}
