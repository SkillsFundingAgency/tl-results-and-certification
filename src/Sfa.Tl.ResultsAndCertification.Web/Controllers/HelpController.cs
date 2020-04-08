using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class HelpController : Controller
    {
        [HttpGet]
        [Route("cookies", Name = RouteConstants.Cookies)]
        public IActionResult Cookies()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("terms-and-conditions", Name = RouteConstants.TermsAndConditions)]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
    }
}