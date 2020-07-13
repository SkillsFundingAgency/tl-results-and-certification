using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.Provider
{
    public class SelectProvider : ViewComponent
    {
        public IViewComponentResult Invoke(SelectProviderViewModel model)
        {
            return View("~/ViewComponents/Registration/SelectProvider/Index.cshtml", model);
        }
    }
}
