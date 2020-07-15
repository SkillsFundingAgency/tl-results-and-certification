using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.DateOfBirth
{
    public class DateOfBirth : ViewComponent
    {
        public IViewComponentResult Invoke(DateofBirthViewModel model)
        {
            return View("~/ViewComponents/Registration/DateofBirth/Index.cshtml", model);
        }
    }
}
