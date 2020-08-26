using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.SelectSpecialism
{
    public class SelectSpecialism : ViewComponent
    {
        public IViewComponentResult Invoke(SelectSpecialismViewModel model)
        {
            return View("~/ViewComponents/Registration/SelectSpecialism/Index.cshtml", model);
        }
    }
}
