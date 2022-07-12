using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.DownloadResults;
using System;
using System.Threading.Tasks;

using DownloadOverallResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.DownloadOverallResults.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class DownloadOverallResultsController : Controller
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IDownloadOverallResultsLoader _downloadOverallResultsLoader;
        private readonly ILogger _logger;

        public DownloadOverallResultsController(ResultsAndCertificationConfiguration configuration,
            IDownloadOverallResultsLoader downloadOverallResultsLoader, 
            ILogger<DownloadOverallResultsController> logger)
        {
            _configuration = configuration;
            _downloadOverallResultsLoader = downloadOverallResultsLoader;
            _logger = logger;
        }

        [HttpGet]
        [Route("download-tlevel-results", Name = RouteConstants.DownloadOverallResultsPage)]
        public IActionResult DownloadOverallResults()
        {
            var viewModel = new DownloadOverallResultsViewModel
            {
                IsOverallResultsAvailable =
                    _configuration.OverallResultsAvailableDate == null ||
                    _configuration.OverallResultsAvailableDate >= DateTime.Today
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("download-tlevel-results-file", Name = RouteConstants.DownloadOverallResultsFile)]
        public async Task<IActionResult> DownloadOverallResultsFileAsync()
        {
            var fileStream = await _downloadOverallResultsLoader.DownloadOverallResultsDataAsync(User.GetUkPrn(), User.GetUserEmail());
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download overall results. Method: DownloadOverallResultsDataAsync({User.GetUkPrn()}, {User.GetUserEmail()})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/csv")
            {
                FileDownloadName = DownloadOverallResultContent.Download_Filename
            };
        }
    }
}
