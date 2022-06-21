using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel model)
        {
            return View("~/ViewComponents/Pagination/Default.cshtml", model);
        }
    }
}