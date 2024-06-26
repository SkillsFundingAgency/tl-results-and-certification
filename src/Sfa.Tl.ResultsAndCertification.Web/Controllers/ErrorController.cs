﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Error;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        public ErrorController(ResultsAndCertificationConfiguration configuration, ILogger<ErrorController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [Route("uploaded-file-virus", Name = RouteConstants.VirusUploaded)]
        public IActionResult VirusUploaded()
        {
            return View();
        }

        [Route("access-denied-wrong-role", Name = RouteConstants.AccessDeniedWrongRole)]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("service-access-denied", Name = RouteConstants.ServiceAccessDenied)]
        public IActionResult ServiceAccessDenied()
        {
            return User.IsInFreezePeriod()
                ? RedirectToAction(nameof(HelpController.ServiceUnavailable), Constants.HelpController)
                : View();
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
        [Route("error/{statusCode}", Name = RouteConstants.Error)]
        public IActionResult HandleErrorCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return RedirectToRoute(RouteConstants.PageNotFound);
                case 405:
                    return RedirectToRoute(RouteConstants.Home);
                case 500:
                default:
                    return RedirectToRoute(RouteConstants.ProblemWithService);
            }
        }
    }
}