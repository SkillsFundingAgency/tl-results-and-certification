using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //[Route("admin/search-learner-records-clear", Name = RouteConstants.AdminSearchLearnersRecordsClear)]
        public async Task<IActionResult> AdminSearchLearnersRecordsClearAsync()
        {
            await _cacheService.RemoveAsync<AdminSearchChangeLogViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminSearchLearnersRecords);
        }

        [HttpGet]
        //[Route("admin/search-learner-records/{pageNumber:int?}", Name = RouteConstants.AdminSearchLearnersRecords)]
        public async Task<IActionResult> AdminSearchLearnersAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminSearchChangeLogViewModel>(CacheKey);
            if (viewModel == null)
            {
                AdminSearchChangeLogViewModel loadedViewModel = await _loader.SearchChangeLogsAsync(string.Empty, null);

                await _cacheService.SetAsync(CacheKey, loadedViewModel);
                return View(loadedViewModel);
            }

            var searchCriteria = viewModel.SearchCriteriaViewModel;

            if (!searchCriteria.IsSearchKeyApplied)
            {
                viewModel.ClearChangeLogDetails();
                return View(viewModel);
            }

            AdminSearchChangeLogViewModel adminSearchChangeLogViewModel = await _loader.SearchChangeLogsAsync(searchCriteria.SearchKey, pageNumber);

            await _cacheService.SetAsync(CacheKey, adminSearchChangeLogViewModel);
            return View(viewModel);
        }
    }
}