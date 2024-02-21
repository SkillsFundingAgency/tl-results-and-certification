using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.TableButton
{
    public class TableButton : ViewComponent
    {
        public IViewComponentResult Invoke(TableButtonModel model)
        {
            return View("~/ViewComponents/TableButton/Default.cshtml", model);
        }
    }
}