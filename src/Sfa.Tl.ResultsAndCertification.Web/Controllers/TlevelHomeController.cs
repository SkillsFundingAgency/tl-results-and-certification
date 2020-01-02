using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TlevelHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
