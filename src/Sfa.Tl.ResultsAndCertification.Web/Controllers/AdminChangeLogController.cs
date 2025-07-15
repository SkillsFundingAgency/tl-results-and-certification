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
            ILogger<AdminChangeLogController> logger)
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

        #region View change log

        [HttpGet]
        [Route("admin/view-change-record/1/{changeLogId}", Name = RouteConstants.AdminViewChangeStartYearRecord)]
        public async Task<IActionResult> AdminViewChangeRecordStartYearAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeStartYearRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/2/{changeLogId}", Name = RouteConstants.AdminViewChangeIPRecord)]
        public async Task<IActionResult> AdminViewChangeRecordIPAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeIPRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/3/{changeLogId}", Name = RouteConstants.AdminViewChangeCoreAssessmentRecord)]
        public async Task<IActionResult> AdminViewChangeRecordCoreAssessmentAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeCoreAssessmentRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/4/{changeLogId}", Name = RouteConstants.AdminViewChangeSpecialismAssessmentRecord)]
        public async Task<IActionResult> AdminViewChangeRecordSpecialismAssessmentAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeSpecialismAssessmentRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/5/{changeLogId}", Name = RouteConstants.AdminViewChangeRemoveCoreAssessmentRecord)]
        public async Task<IActionResult> AdminViewChangeRecordRemoveCoreAssessmentAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeRemoveCoreAssessmentRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/6/{changeLogId}", Name = RouteConstants.AdminViewChangeRemoveSpecialismAssessmentRecord)]
        public async Task<IActionResult> AdminViewChangeRecordRemoveSpecialismAssessmentAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeRemoveSpecialismAssessmentRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/7/{changeLogId}", Name = RouteConstants.AdminViewChangeAddPathwayResultRecord)]
        public async Task<IActionResult> AdminViewChangeRecordAddPathwayResultAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeAddPathwayResultRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/8/{changeLogId}", Name = RouteConstants.AdminViewChangeAddSpecialismResultRecord)]
        public async Task<IActionResult> AdminViewChangeRecordAddSpecialismResultAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeAddSpecialismResultRecord(changeLogId);
            return View(result);
        }


        [HttpGet]
        [Route("admin/view-change-record/9/{changeLogId}", Name = RouteConstants.AdminViewChangePathwayResultRecord)]
        public async Task<IActionResult> AdminViewChangeRecordChangePathwayResultAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangePathwayResultRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/10/{changeLogId}", Name = RouteConstants.AdminViewChangeSpecialismResultRecord)]
        public async Task<IActionResult> AdminViewChangeRecordChangeSpecialismResultAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeSpecialismResultRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/11/{changeLogId}", Name = RouteConstants.AdminViewOpenPathwayRommRecord)]
        public async Task<IActionResult> AdminViewChangeRecordOpenPathwayRommAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeOpenPathwayRommRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/12/{changeLogId}", Name = RouteConstants.AdminViewOpenSpecialismRommRecord)]
        public async Task<IActionResult> AdminViewChangeRecordOpenSpecialismRommAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeOpenSpecialismRommRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/13/{changeLogId}", Name = RouteConstants.AdminViewPathwayRommOutcomeRecord)]
        public async Task<IActionResult> AdminViewChangeRecordPathwayRommOutcomeAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangePathwayRommOutcomeRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/14/{changeLogId}", Name = RouteConstants.AdminViewSpecialismRommOutcomeRecord)]
        public async Task<IActionResult> AdminViewChangeRecordSpecialismRommOutcomeAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeSpecialismRommOutcomeRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/15/{changeLogId}", Name = RouteConstants.AdminViewOpenPathwayAppealRecord)]
        public async Task<IActionResult> AdminViewChangeRecordOpenPathwayAppealAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeOpenPathwayAppealRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/16/{changeLogId}", Name = RouteConstants.AdminViewOpenSpecialismAppealRecord)]
        public async Task<IActionResult> AdminViewChangeRecordOpenSpecialismAppealAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeOpenSpecialismAppealRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/17/{changeLogId}", Name = RouteConstants.AdminViewPathwayAppealOutcomeRecord)]
        public async Task<IActionResult> AdminViewChangeRecordPathwayAppealOutcomeAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangePathwayAppealOutcomeRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/18/{changeLogId}", Name = RouteConstants.AdminViewSpecialismAppealOutcomeRecord)]
        public async Task<IActionResult> AdminViewChangeRecordSpecialismAppealOutcomeAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeSpecialismAppealOutcomeRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/19/{changeLogId}", Name = RouteConstants.AdminViewChangeMathsStatusRecord)]
        public async Task<IActionResult> AdminViewChangeRecordMathsStatusAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeMathsStatusRecord(changeLogId);
            return View(result);
        }

        [HttpGet]
        [Route("admin/view-change-record/20/{changeLogId}", Name = RouteConstants.AdminViewChangeEnglishStatusRecord)]
        public async Task<IActionResult> AdminViewChangeRecordEnglishStatusAsync(int changeLogId)
        {
            var result = await _loader.GetAdminViewChangeEnglishStatusRecord(changeLogId);
            return View(result);
        }

        #endregion View change log
    }
}