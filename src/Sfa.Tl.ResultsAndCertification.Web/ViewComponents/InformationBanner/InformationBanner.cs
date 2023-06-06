using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner
{
    public class InformationBanner : ViewComponent
    {
        public IViewComponentResult Invoke(InformationBannerModel model)
        {
            return View("~/ViewComponents/InformationBanner/Default.cshtml", model);
        }
    }
}
