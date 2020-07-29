using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.SelectAcademicYear
{
    public class SelectAcademicYear : ViewComponent
    {
        public IViewComponentResult Invoke(SelectAcademicYearViewModel model)
        {
            return View("~/ViewComponents/Registration/SelectAcademicYear/Index.cshtml", model);
        }
    }
}
