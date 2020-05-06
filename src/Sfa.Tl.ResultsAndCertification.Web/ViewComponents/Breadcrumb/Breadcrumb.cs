using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb
{
    public class Breadcrumb : ViewComponent
    {
        public IViewComponentResult Invoke(BreadcrumbModel model)
        {
            return View("~/ViewComponents/Breadcrumb/Default.cshtml", model);
        }
    }
}
