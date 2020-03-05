using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("view-tlevels", Name = RouteConstants.ViewTlevels)]
        public IActionResult ViewTlevels()
        {
            return RedirectToRoute(RouteConstants.Tlevels);
        }
    }
}
