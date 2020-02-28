using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        [Route("access-denied", Name = "AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("no-service-permission", Name = "ServiceAccessDenied")]
        public IActionResult ServiceAccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("page-not-found", Name = "PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return RedirectToRoute(RouteConstants.PageNotFound);
                case 500:
                default:
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}