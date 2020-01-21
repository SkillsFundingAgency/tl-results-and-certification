using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
