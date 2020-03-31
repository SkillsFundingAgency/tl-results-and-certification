using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        [Route("dashboard", Name = RouteConstants.Dashboard)]
        public IActionResult Index()
        {
            return HttpContext.User.HasAccessToService() ? View() : (IActionResult)RedirectToRoute(RouteConstants.ServiceAccessDenied);
        }
    }
}
