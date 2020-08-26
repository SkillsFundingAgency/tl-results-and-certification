using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.SelectCore
{
    public class SelectCore : ViewComponent
    {
        public IViewComponentResult Invoke(SelectCoreViewModel model)
        {
            return View("~/ViewComponents/Registration/SelectCore/Index.cshtml", model);
        }
    }
}
