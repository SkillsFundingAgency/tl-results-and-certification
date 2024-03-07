using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminChangeLogController : Controller
    {
        private readonly IAdminChangeLogLoader _loader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminChangeLogCacheKey);

        public AdminChangeLogController(
            IAdminChangeLogLoader loader,
            ICacheService cacheService,
            ILogger<AdminDashboardController> logger)
        {
            _loader = loader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/change-log-clear", Name = RouteConstants.AdminSearchChangeLogClear)]
        public async Task<IActionResult> AdminSearchChangeLogClearAsync()
        {
            await _cacheService.RemoveAsync<AdminSearchChangeLogViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminSearchChangeLog);
        }

        [HttpGet]
        [Route("admin/change-log/{pageNumber:int?}", Name = RouteConstants.AdminSearchChangeLog)]
        public async Task<IActionResult> AdminSearchChangeLogAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            if (viewModel == null)
            {
                AdminSearchChangeLogViewModel loadedViewModel = await _loader.SearchChangeLogsAsync();

                await _cacheService.SetAsync(CacheKey, loadedViewModel);
                return View(loadedViewModel);
            }

            var searchCriteria = viewModel.SearchCriteriaViewModel;
            searchCriteria.PageNumber = pageNumber;

            AdminSearchChangeLogViewModel adminSearchChangeLogViewModel = await _loader.SearchChangeLogsAsync(searchCriteria.SearchKey, pageNumber);

            await _cacheService.SetAsync(CacheKey, adminSearchChangeLogViewModel);
            return View(adminSearchChangeLogViewModel);
        }

        [HttpPost]
        [Route("admin/change-log-search-key", Name = RouteConstants.SubmitAdminSearchChangeLogSearchKey)]
        public Task<IActionResult> AdminSearchChangeLogSearchKeyAsync(AdminSearchChangeLogCriteriaViewModel searchCriteriaViewModel)
            => RunAsync(RouteConstants.SubmitAdminSearchChangeLogSearchKey, p => p.SetSearchKey(searchCriteriaViewModel.SearchKey));

        [HttpPost]
        [Route("admin/change-log-clear-key", Name = RouteConstants.SubmitAdminSearchChangeLogClearKey)]
        public Task<IActionResult> AdminSearchChangeLogClearKeyAsync()
            => RunAsync(RouteConstants.SubmitAdminSearchChangeLogClearKey, p => p.ClearSearchKey());

        private async Task<IActionResult> RunAsync(string endpoint, Action<AdminSearchChangeLogViewModel> action)
        {
            var viewModel = await _cacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminSearchChangeLogViewModel cache data found. Method: {endpoint}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            action(viewModel);

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminSearchChangeLog, new { pageNumber = viewModel.SearchCriteriaViewModel.PageNumber });
        }
    }
}