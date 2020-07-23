using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.LearnerName
{
    public class LearnerName : ViewComponent
    {
        public IViewComponentResult Invoke(LearnersNameViewModel model)
        {
            return View("~/ViewComponents/Registration/LearnerName/Index.cshtml", model);
        }
    }
}
