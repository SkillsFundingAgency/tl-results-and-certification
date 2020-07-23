using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem
{
    public class SummaryItem : ViewComponent
    {
        public IViewComponentResult Invoke(SummaryItemModel model)
        {
            return View("~/ViewComponents/Summary/SummaryItem/Index.cshtml", model);
        }
    }
}
