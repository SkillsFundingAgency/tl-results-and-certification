using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LearnerRecord = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardLoader _loader;
        private readonly IIndustryPlacementLoader _industryPlacementLoader;

        private readonly IProviderLoader _providerLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey); } }
        private string InformationCacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardInformationCacheKey); } }

        public AdminDashboardController(
            IAdminDashboardLoader loader,
            IProviderLoader providerLoader,
            IIndustryPlacementLoader industryPlacementLoader,
            ICacheService cacheService,
            ILogger<AdminDashboardController> logger)
        {
            _loader = loader;
            _providerLoader = providerLoader;
            _industryPlacementLoader = industryPlacementLoader;
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

        #region Change Start Year

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

            if (viewModel.AcademicStartYearsToBe.Count == 0 && !ModelState.IsValid)
                ModelState[nameof(model.AcademicYearTo)].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _cacheService.SetAsync<AdminChangeStartYearViewModel>(CacheKey, model);
            return RedirectToAction(nameof(RouteConstants.ReviewChangeStartYear), new { pathwayId = model.PathwayId });
        }

        [HttpGet]
        [Route("admin/review-changes-start-year/{pathwayId}", Name = RouteConstants.ReviewChangeStartYear)]
        public async Task<IActionResult> ReviewChangeStartYearAsync(int pathwayId)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(pathwayId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var _cachedModel = await _cacheService.GetAsync<AdminChangeStartYearViewModel>(CacheKey);
            TempData.Set(Constants.AcademicYearTo, _cachedModel.AcademicYearTo);

            viewModel.AcademicYearTo = _cachedModel.AcademicYearTo;
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-start-year", Name = RouteConstants.SubmitReviewChangeStartYear)]
        public async Task<IActionResult> ReviewChangeStartYearAsync(ReviewChangeStartYearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isSuccess = await _loader.ProcessChangeStartYearAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = LearnerRecord.Message_Notification_Success,
                    IsRawHtml = true
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }
            else { return RedirectToRoute(RouteConstants.ProblemWithService); }
        }

        #endregion

        #region Change Industry Placement

        [HttpGet]
        [Route("admin/change-industry-placement-clear/{registrationPathwayId}", Name = RouteConstants.AdminChangeIndustryPlacementClear)]
        public async Task<IActionResult> ChangeIndustryPlacementClearAsync(int registrationPathwayId)
        {
            await _cacheService.RemoveAsync<AdminChangeIpViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminChangeIndustryPlacement, new { registrationPathwayId });
        }

        [HttpGet]
        [Route("admin/change-industry-placement/{registrationPathwayId}", Name = RouteConstants.AdminChangeIndustryPlacement)]
        public async Task<IActionResult> AdminChangeIndustryPlacementAsync(int registrationPathwayId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (cachedModel != null)
            {
                return View(cachedModel.AdminIpCompletion);
            }

            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(registrationPathwayId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-change-industry-placement", Name = RouteConstants.AdminSubmitChangeIndustryPlacement)]
        public async Task<IActionResult> AdminChangeIndustryPlacementAsync(AdminIpCompletionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IndustryPlacementStatusTo == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

                var modelToCache = cachedModel ?? new AdminChangeIpViewModel();
                modelToCache.AdminIpCompletion = model;

                await _cacheService.SetAsync(CacheKey, modelToCache);
                return RedirectToAction(nameof(RouteConstants.AdminIndustryPlacementSpecialConsiderationHours));
            }

            await _cacheService.SetAsync(CacheKey, new AdminChangeIpViewModel { AdminIpCompletion = model });
            return RedirectToAction(nameof(RouteConstants.AdminReviewChangesIndustryPlacement), new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/industry-placement-hours", Name = RouteConstants.AdminIndustryPlacementSpecialConsiderationHours)]
        public async Task<IActionResult> AdminIndustryPlacementSpecialConsiderationHoursAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (cachedModel?.AdminIpCompletion?.IndustryPlacementStatusTo != IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (cachedModel.HoursViewModel != null)
            {
                return View(cachedModel.HoursViewModel);
            }

            var viewModel = new AdminIpSpecialConsiderationHoursViewModel
            {
                RegistrationPathwayId = cachedModel.AdminIpCompletion.RegistrationPathwayId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/industry-placement-hours", Name = RouteConstants.SubmitAdminIndustryPlacementSpecialConsiderationHours)]
        public async Task<IActionResult> AdminIndustryPlacementSpecialConsiderationHoursAsync(AdminIpSpecialConsiderationHoursViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (cachedModel?.AdminIpCompletion?.IndustryPlacementStatusTo != IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            cachedModel.HoursViewModel = model;
            await _cacheService.SetAsync(CacheKey, cachedModel);

            return RedirectToRoute(RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons);
        }

        [HttpGet]
        [Route("admin/industry-placement-incomplete", Name = RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons)]
        public async Task<IActionResult> AdminIndustryPlacementSpecialConsiderationReasonsAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (cachedModel?.AdminIpCompletion?.IndustryPlacementStatusTo != IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (cachedModel?.ReasonsViewModel != null)
            {
                return View(cachedModel.ReasonsViewModel);
            }

            var viewModel = new AdminIpSpecialConsiderationReasonsViewModel
            {
                RegistrationPathwayId = cachedModel.AdminIpCompletion.RegistrationPathwayId,
                ReasonsList = await _industryPlacementLoader.GetSpecialConsiderationReasonsListAsync(cachedModel.AdminIpCompletion.AcademicYear)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/industry-placement-incomplete", Name = RouteConstants.SubmitAdminIndustryPlacementSpecialConsiderationReasons)]
        public async Task<IActionResult> AdminIndustryPlacementSpecialConsiderationReasonsAsync(AdminIpSpecialConsiderationReasonsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (cachedModel?.AdminIpCompletion?.IndustryPlacementStatusTo != IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (cachedModel.HoursViewModel == null || string.IsNullOrWhiteSpace(cachedModel.HoursViewModel.Hours))
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            cachedModel.ReasonsViewModel = model;
            await _cacheService.SetAsync(CacheKey, cachedModel);

            return RedirectToRoute(nameof(RouteConstants.AdminReviewChangesIndustryPlacement), new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/review-changes-industry-placement/{pathwayId}", Name = RouteConstants.AdminReviewChangesIndustryPlacement)]
        public async Task<IActionResult> AdminReviewChangesIndustryPlacementAsync(int pathwayId)
        {
            AdminReviewChangesIndustryPlacementViewModel viewModel = new();

            var _cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);

            if (_cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.AdminChangeIpViewModel = _cachedModel;

            await _cacheService.SetAsync<AdminReviewChangesIndustryPlacementViewModel>(CacheKey, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-industry-placement", Name = RouteConstants.SubmitReviewChangesIndustryPlacement)]
        public async Task<IActionResult> AdminReviewChangesIndustryPlacementAsync(AdminReviewChangesIndustryPlacementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var _cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);
            model.AdminChangeIpViewModel = _cachedModel ?? new AdminChangeIpViewModel();
            var isSuccess = await _loader.ProcessChangeIndustryPlacementAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = ReviewChangesIndustryPlacement.Message_Notification_Success,
                    IsRawHtml = true,
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.AdminChangeIpViewModel.AdminIpCompletion.RegistrationPathwayId });
            }
            else { return RedirectToAction(RouteConstants.ProblemWithService); }


        }
        #endregion

        #region Remove assessments

        [HttpGet]
        [Route("admin/remove-assessment-entry-core-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.RemoveAssessmentEntryCoreClear)]
        public async Task<IActionResult> RemoveAssessmentEntryCoreClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.RemoveAssessmentEntryCore, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/remove-assessment-entry-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.RemoveAssessmentEntryCore)]
        public async Task<IActionResult> RemoveAssessmentEntryCoreAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminRemovePathwayAssessmentEntryViewModel viewModel = await _loader.GetRemovePathwayAssessmentEntryAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core assessment details found. Method: RemoveAssessmentEntryCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/remove-assessment-entry-core", Name = RouteConstants.SubmitRemoveAssessmentEntryCore)]
        public async Task<IActionResult> RemoveAssessmentEntryCoreAsync(AdminRemovePathwayAssessmentEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool noSelected = model.DoYouWantToRemoveThisAssessmentEntry.HasValue && !model.DoYouWantToRemoveThisAssessmentEntry.Value;
            if (noSelected)
            {
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await Task.CompletedTask;
            return RedirectToRoute(RouteConstants.PageNotFound);
        }

        [HttpGet]
        [Route("admin/remove-assessment-entry-specialism-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.RemoveAssessmentSpecialismEntryClear)]
        public async Task<IActionResult> RemoveAssessmentEntrySpecialismClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.RemoveAssessmentSpecialismEntry, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/remove-assessment-entry-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.RemoveAssessmentSpecialismEntry)]
        public async Task<IActionResult> RemoveAssessmentEntrySpecialismAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminRemoveSpecialismAssessmentEntryViewModel viewModel = await _loader.GetRemoveSpecialismAssessmentEntryAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No specialism assessment details found. Method: RemoveSpecialismEntryCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/remove-assessment-entry-specialism", Name = RouteConstants.SubmitRemoveAssessmentSpecialismEntry)]
        public async Task<IActionResult> RemoveAssessmentEntrySpecialismAsync(AdminRemoveSpecialismAssessmentEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool noSelected = model.DoYouWantToRemoveThisAssessmentEntry.HasValue && !model.DoYouWantToRemoveThisAssessmentEntry.Value;
            if (noSelected)
            {
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await Task.CompletedTask;
            return RedirectToRoute(RouteConstants.PageNotFound);
        }

        #endregion
    }
}