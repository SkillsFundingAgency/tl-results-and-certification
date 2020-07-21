using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.Uln
{
    public class Uln : ViewComponent
    {
        public IViewComponentResult Invoke(UlnViewModel model)
        {
            return View("~/ViewComponents/Registration/Uln/Index.cshtml", model);
        }
    }
}
