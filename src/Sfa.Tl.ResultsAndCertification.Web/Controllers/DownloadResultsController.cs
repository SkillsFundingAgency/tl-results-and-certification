using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.DownloadResults;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class DownloadResultsController : Controller
    {
        private readonly ResultsAndCertificationConfiguration _configuration;

        public DownloadResultsController(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("download-tlevel-results", Name = RouteConstants.DownloadTlevelResults)]
        public IActionResult DownloadTlevelResults()
        {
            var viewModel = new DownloadTlevelResultsViewModel
            {
                IsOverallResultsAvailable =
                    _configuration.OverallResultsAvailableDate == null ||
                    _configuration.OverallResultsAvailableDate >= DateTime.Today
            };

            return View(viewModel);
        }
    }
}
