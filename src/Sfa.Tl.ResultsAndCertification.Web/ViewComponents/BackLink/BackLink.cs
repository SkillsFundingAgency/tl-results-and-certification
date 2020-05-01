using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink
{
    public class BackLink : ViewComponent
    {
        public IViewComponentResult Invoke(BackLinkModel model)
        {            
            return View(model);
        }
    }
}
