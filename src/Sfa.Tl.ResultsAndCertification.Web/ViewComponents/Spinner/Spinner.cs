using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Spinner
{
    public class Spinner : ViewComponent
    {
        public IViewComponentResult Invoke(SpinnerModel model)
        {
            return View("~/ViewComponents/Spinner/Default.cshtml", model);
        }
    }
}
