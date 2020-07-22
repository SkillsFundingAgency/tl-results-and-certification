using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryComponent
{
    public class SummaryComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SummaryComponentModel model)
        {
            return View("~/ViewComponents/Summary/SummaryComponent/Index.cshtml", model);
        }
    }
}
