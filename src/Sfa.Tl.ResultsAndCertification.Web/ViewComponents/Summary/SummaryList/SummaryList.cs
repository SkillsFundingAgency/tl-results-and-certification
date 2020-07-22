using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList
{
    public class SummaryList : ViewComponent
    {
        public IViewComponentResult Invoke(SummaryListModel model)
        {
            return View("~/ViewComponents/Summary/SummaryList/Index.cshtml", model);
        }
    }
}
