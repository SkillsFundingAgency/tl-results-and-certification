using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Error;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ResultsAndCertificationConfiguration _configuration;

        public ErrorController(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("access-denied", Name = RouteConstants.AccessDenied)]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("no-service-permission", Name = RouteConstants.ServiceAccessDenied)]
        public IActionResult ServiceAccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("page-not-found", Name = RouteConstants.PageNotFound)]
        public IActionResult PageNotFound()
        {
            var viewmodel = new PageNotFoundViewModel
            {
                TechnicalSupportEmailAddress = _configuration.TechnicalSupportEmailAddress
            };

            return View(viewmodel);
        }

        [AllowAnonymous]
        [Route("problem-with-service", Name = RouteConstants.ProblemWithService)]
        public IActionResult ProblemWithService()
        {
            var viewModel = new ProblemWithServiceViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                TechnicalSupportEmailAddress = _configuration.TechnicalSupportEmailAddress
            };

            return View(viewModel);
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
                    return RedirectToRoute(RouteConstants.ProblemWithService);
            }
        }
    }
}