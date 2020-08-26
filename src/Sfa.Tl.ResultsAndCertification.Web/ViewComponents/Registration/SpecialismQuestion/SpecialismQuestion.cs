using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.SpecialismQuestion
{
    public class SpecialismQuestion : ViewComponent
    {
        public IViewComponentResult Invoke(SpecialismQuestionViewModel model)
        {
            return View("~/ViewComponents/Registration/SpecialismQuestion/Index.cshtml", model);
        }
    }
}
