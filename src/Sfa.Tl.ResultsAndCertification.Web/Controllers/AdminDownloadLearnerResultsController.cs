using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDownloadLearnerResults;
using Sfa.Tl.ResultsAndCertification.Web.FileResult;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminDownloadLearnerResultsController : Controller
    {
        private readonly IProviderLoader _providerLoader;
        private readonly IAdminDownloadLearnerResultsLoader _adminDownloadLearnerResultsLoader;
        private readonly IDownloadOverallResultsLoader _downloadOverallResultsLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminDownloadLearnerResultsController> _logger;

        private string CacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

        public AdminDownloadLearnerResultsController(
            IProviderLoader providerLoader,
            IAdminDownloadLearnerResultsLoader adminDownloadLearnerResultsLoader,
            IDownloadOverallResultsLoader downloadOverallResultsLoader,
            ICacheService cacheService,
            ILogger<AdminDownloadLearnerResultsController> logger)
        {
            _providerLoader = providerLoader;
            _adminDownloadLearnerResultsLoader = adminDownloadLearnerResultsLoader;
            _downloadOverallResultsLoader = downloadOverallResultsLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/download-learner-results-find-provider-clear", Name = RouteConstants.AdminDownloadLearnerResultsFindProviderClear)]
        public async Task<IActionResult> AdminDownloadLearnerResultsFindProviderClearAsync()
        {
            await _cacheService.RemoveAsync<AdminDownloadLearnerResultsFindProviderViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminDownloadLearnerResultsFindProvider);
        }

        [HttpGet]
        [Route("admin/download-learner-results-find-provider", Name = RouteConstants.AdminDownloadLearnerResultsFindProvider)]
        public async Task<IActionResult> AdminDownloadLearnerResultsFindProviderAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminDownloadLearnerResultsFindProviderViewModel>(CacheKey);
            return viewModel == null ? View(new AdminDownloadLearnerResultsFindProviderViewModel()) : View(viewModel);
        }

        [HttpPost]
        [Route("admin/download-learner-results-find-provider", Name = RouteConstants.AdminDownloadLearnerResultsSubmitFindProvider)]
        public async Task<IActionResult> AdminDownloadLearnerResultsFindProviderAsync(AdminDownloadLearnerResultsFindProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            (bool found, int providerId) = await GetProviderId(viewModel.Search);
            if (!found)
            {
                ModelState.AddModelError("Search", AdminDownloadLearnerResultsFindProvider.ProviderName_NotValid_Validation_Message);
                return View(viewModel);
            }

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminDownloadLearnerResultsByProvider, new { providerId });
        }

        [HttpGet]
        [Route("admin/download-learner-results-provider/{providerId}", Name = RouteConstants.AdminDownloadLearnerResultsByProvider)]
        public async Task<IActionResult> AdminDownloadLearnerResultsByProviderAsync(int providerId)
        {
            AdminDownloadLearnerResultsByProviderViewModel viewModel = await _adminDownloadLearnerResultsLoader.GetDownloadLearnerResultsByProviderViewModel(providerId);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ProviderNotFound, $"No provider found. Method: AdminDownloadLearnerResultsByProviderAsync({providerId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/download-learner-results-file-csv/{ukprn}", Name = RouteConstants.AdminDownloadLearnerResultsCsv)]
        public async Task<IActionResult> AdminDownloadLearnerResultsCsvAsync(long ukprn)
        {
            var fileStream = await _downloadOverallResultsLoader.DownloadOverallResultsDataAsync(ukprn, User.GetUserEmail());
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download overall results. Method: AdminDownloadLearnerResultsCsvAsync({ukprn}, {User.GetUserEmail()})");
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            fileStream.Position = 0;
            return new CsvFileStreamResult(fileStream, AdminDownloadLearnerResultsByProvider.Download_Filename);
        }

        [HttpGet]
        [Route("admin/download-learner-results-file-pdf/{ukprn}", Name = RouteConstants.AdminDownloadLearnerResultsPdf)]
        public async Task<IActionResult> AdminDownloadLearnerResultsPdfAsync(long ukprn)
        {
            var fileStream = await _downloadOverallResultsLoader.DownloadOverallResultSlipsDataAsync(ukprn, User.GetUserEmail());
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download overall results. Method: DownloadOverallResultSlipsFileAsync({ukprn}, {User.GetUserEmail()})");
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            fileStream.Position = 0;
            return new PdfFileStreamResult(fileStream, AdminDownloadLearnerResultsByProvider.Download_ResultSlips_Filename);
        }

        private async Task<(bool found, int providerId)> GetProviderId(string providerName)
        {
            (bool, int) notFoundResult = (false, 0);

            if (string.IsNullOrWhiteSpace(providerName))
            {
                return notFoundResult;
            }

            IEnumerable<ProviderLookupData> providerData = await _providerLoader.GetProviderLookupDataAsync(providerName, isExactMatch: true);
            if (!providerData.IsNullOrEmpty() && providerData.Count() == 1)
            {
                return (true, providerData.Single().Id);
            }

            return notFoundResult;
        }
    }
}