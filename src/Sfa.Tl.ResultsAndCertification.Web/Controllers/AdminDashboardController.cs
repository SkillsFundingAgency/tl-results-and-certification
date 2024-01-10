using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnerRecord = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardLoader _loader;
        private readonly IProviderLoader _providerLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;
        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey); } }
        private string InformationCacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardInformationCacheKey); } }

        public AdminDashboardController(
            IAdminDashboardLoader loader,
            IProviderLoader providerLoader,
            ICacheService cacheService,
            ILogger<AdminDashboardController> logger)
        {
            _loader = loader;
            _providerLoader = providerLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/home", Name = RouteConstants.AdminHome)]
        public async Task<IActionResult> AdminHome()
        {
            await _cacheService.RemoveAsync<AdminSearchLearnerViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.Home);
        }

        #region Search learner   
        [HttpGet]
        [Route("admin/search-learner-records-clear", Name = RouteConstants.AdminSearchLearnersRecordsClear)]
        public async Task<IActionResult> AdminSearchLearnersRecordsClearAsync()
        {
            await _cacheService.RemoveAsync<AdminSearchLearnerViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminSearchLearnersRecords);
        }

        [HttpGet]
        [Route("admin/search-learner-records/{pageNumber:int?}", Name = RouteConstants.AdminSearchLearnersRecords)]
        public async Task<IActionResult> AdminSearchLearnersAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey);
            if (viewModel == null)
            {
                AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();
                viewModel = new AdminSearchLearnerViewModel(filters);

                await _cacheService.SetAsync(CacheKey, viewModel);
                return View(viewModel);
            }

            var searchCriteria = viewModel.SearchLearnerCriteria;

            if (!searchCriteria.IsSearchKeyApplied && !searchCriteria.AreFiltersApplied)
            {
                viewModel.ClearLearnerDetails();
                return View(viewModel);
            }

            searchCriteria.PageNumber = pageNumber;

            AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel = await _loader.GetAdminSearchLearnerDetailsListAsync(searchCriteria);
            viewModel.SetLearnerDetails(learnerDetailsListViewModel);

            viewModel.SearchLearnerDetailsList = learnerDetailsListViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/search-learner-records-search-key", Name = RouteConstants.SubmitAdminSearchLearnerRecordsApplySearchKey)]
        public Task<IActionResult> AdminSearchLearnersRecordsApplySearchKeyAsync(AdminSearchLearnerCriteriaViewModel searchCriteriaViewModel)
            => RunAsync(RouteConstants.SubmitAdminSearchLearnerRecordsApplySearchKey, p => p.SetSearchKey(searchCriteriaViewModel.SearchKey));

        [HttpPost]
        [Route("admin/search-learner-records-clear-key", Name = RouteConstants.SubmitAdminSearchLearnerClearKey)]
        public Task<IActionResult> AdminSearchLearnerClearKeyAsync()
            => RunAsync(RouteConstants.SubmitAdminSearchLearnerClearKey, p => p.ClearSearchKey());

        [HttpPost]
        [Route("admin/search-learner-records-filters", Name = RouteConstants.SubmitAdminSearchLearnerRecordsApplyFilters)]
        public async Task<IActionResult> AdminSearchLearnerRecordsApplyFiltersAsync(AdminSearchLearnerFiltersViewModel filtersViewModel)
        {
            int? providerId = await GetFilterProviderId(filtersViewModel.Search);
            filtersViewModel.SelectedProviderId = providerId;

            return await RunAsync(RouteConstants.SubmitAdminSearchLearnerRecordsApplyFilters, p => p.SetFilters(filtersViewModel));
        }

        [HttpPost]
        [Route("admin/search-learner-records-clear-filters", Name = RouteConstants.SubmitAdminSearchLearnerClearFilters)]
        public Task<IActionResult> AdminSearchLearnerClearFiltersAsync()
            => RunAsync(RouteConstants.SubmitAdminSearchLearnerClearFilters, p => p.ClearFilters());

        private async Task<IActionResult> RunAsync(string endpoint, Action<AdminSearchLearnerViewModel> action)
        {
            var viewModel = await _cacheService.GetAsync<AdminSearchLearnerViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminSearchLearnerViewModel cache data found. Method: {endpoint}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            action(viewModel);

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminSearchLearnersRecords, new { pageNumber = viewModel.SearchLearnerCriteria.PageNumber });
        }

        private async Task<int?> GetFilterProviderId(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return null;
            }

            IEnumerable<ProviderLookupData> providerData = await _providerLoader.GetProviderLookupDataAsync(providerName, isExactMatch: true);
            if (!providerData.IsNullOrEmpty() && providerData.Count() == 1)
            {
                return providerData.Single().Id;
            }

            return 0;
        }

        #endregion

        [HttpGet]
        [Route("admin/change-start-year/{pathwayId}/{isBack:bool?}", Name = RouteConstants.ChangeStartYear)]
        public async Task<IActionResult> ChangeStartYearAsync(int pathwayId, bool isBack = false)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(pathwayId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (isBack)
                viewModel.AcademicYearTo = TempData.Get<string>(Constants.AcademicYearTo) ?? viewModel.AcademicYearTo;
           
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-change-start-year", Name = RouteConstants.SubmitChangeStartYear)]
        public async Task<IActionResult> ChangeStartYearAsync(AdminChangeStartYearViewModel model)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(model.PathwayId);
            
            if (viewModel.AcademicStartYearsToBe.Count() == 0 && !ModelState.IsValid) 
                ModelState[nameof(model.AcademicYearTo)].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _cacheService.SetAsync<AdminChangeStartYearViewModel>(CacheKey, model);
            return RedirectToAction(nameof(RouteConstants.ReviewChangeStartYear), new { pathwayId = model.PathwayId });
        }

        [HttpGet]
        [Route("admin/learner-record/{pathwayid}", Name = RouteConstants.AdminLearnerRecord)]
        public async Task<IActionResult> AdminLearnerRecordAsync(int pathwayId)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(pathwayId);
            if (viewModel == null || !viewModel.IsLearnerRegistered)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner is not registerd or learner record not added. Method: LearnerRecordDetailsAsync({pathwayId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.InformationBanner = await _cacheService.GetAndRemoveAsync<InformationBannerModel>(InformationCacheKey);
            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);

            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/review-changes-start-year/{pathwayId}", Name = RouteConstants.ReviewChangeStartYear)]
        public async Task<IActionResult> ReviewChangeStartYearAsync(int pathwayId)
        {
            var _cachedModel = await _cacheService.GetAsync<AdminChangeStartYearViewModel>(CacheKey);
            var viewModel = await _loader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(pathwayId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            TempData.Set(Constants.AcademicYearTo, _cachedModel.AcademicYearTo);

            viewModel.AcademicYearTo = _cachedModel.AcademicYearTo;
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-start-year", Name = RouteConstants.SubmitReviewChangeStartYear)]
        public async Task<IActionResult> ReviewChangeStartYearAsync(ReviewChangeStartYearViewModel model)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(model.PathwayId);

            if (!ModelState.IsValid)
                return View(model);

            await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
            {
                DisplayMessageBody = true,
                Message = LearnerRecord.Message_Notification_Success
            },
            CacheExpiryTime.XSmall); 

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.PathwayId });
        }

        [HttpGet]
        [Route("admin/admin-change-industry-placement/{pathwayId}", Name = RouteConstants.AdminChangeIndustryPlacement)]
        public async Task<IActionResult> ChangeIndustryPlacementAsync(int pathwayId)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(pathwayId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/admin-submit-change-industry-placement", Name = RouteConstants.AdminSubmitChangeIndustryPlacement)]
        public async Task<IActionResult> ChangeIndustryPlacementAsync(AdminChangeIndustryPlacementViewModel model)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(model.PathwayId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await Task.CompletedTask;
            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.PathwayId });
        }
    }
}