using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Web.Models;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("page-not-found", Name = "PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("/home/error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                404 => RedirectToRoute("PageNotFound"),
                _ => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }),
            };
        }
    }
}
