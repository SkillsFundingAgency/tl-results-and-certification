using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
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
        [Route("admin/change-start-year-clear/{registrationPathwayId}", Name = RouteConstants.ChangeStartYearClear)]
        public async Task<IActionResult> ChangeStartYearClearAsync(int registrationPathwayId)
        {
            await _cacheService.RemoveAsync<AdminChangeStartYearViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.ChangeStartYear, new { registrationPathwayId });
        }

        [HttpGet]
        [Route("admin/change-start-year/{registrationPathwayId}", Name = RouteConstants.ChangeStartYear)]
        public async Task<IActionResult> ChangeStartYearAsync(int registrationPathwayId)
        {
            AdminChangeStartYearViewModel cachedModel = await _cacheService.GetAsync<AdminChangeStartYearViewModel>(CacheKey);

            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminChangeStartYearViewModel viewModel = await _loader.GetAdminLearnerRecordChangeYearAsync(registrationPathwayId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-change-start-year", Name = RouteConstants.SubmitChangeStartYear)]
        public async Task<IActionResult> ChangeStartYearAsync(AdminChangeStartYearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToAction(nameof(RouteConstants.ReviewChangeStartYear), new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/review-changes-start-year/{pathwayId}", Name = RouteConstants.ReviewChangeStartYear)]
        public async Task<IActionResult> ReviewChangeStartYearAsync(int pathwayId)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(pathwayId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var _cachedModel = await _cacheService.GetAsync<AdminChangeStartYearViewModel>(CacheKey);
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

            var success = await _loader.ProcessChangeStartYearAsync(model);

            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            await _cacheService.SetAsync(CacheKey, new AdminNotificationBannerModel(LearnerRecord.Change_Year_Notification_Success), CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
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
            var _cachedModel = await _cacheService.GetAsync<AdminChangeIpViewModel>(CacheKey);
            model.AdminChangeIpViewModel = _cachedModel ?? new AdminChangeIpViewModel();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
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

        #region Change Maths Status

        [HttpGet]
        [Route("admin/change-maths-status-clear/{registrationPathwayId}", Name = RouteConstants.AdminChangeMathsStatusClear)]
        public async Task<IActionResult> ChangeMathsStatusClearAsync(int registrationPathwayId)
        {
            await _cacheService.RemoveAsync<AdminChangeMathsStatusViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminChangeMathsStatus, new { registrationPathwayId });
        }

        [HttpGet]
        [Route("admin/change-maths-status/{registrationPathwayId}", Name = RouteConstants.AdminChangeMathsStatus)]
        public async Task<IActionResult> AdminChangeMathsStatusAsync(int registrationPathwayId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeMathsStatusViewModel>(CacheKey);

            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(registrationPathwayId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-change-maths-status", Name = RouteConstants.SubmitAdminChangeMathsStatus)]
        public async Task<IActionResult> AdminChangeMathsStatusAsync(AdminChangeMathsStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(AdminChangeMathsStatus), model);
            }

            if (model.MathsStatusTo == SubjectStatus.NotAchieved || model.MathsStatusTo == SubjectStatus.NotAchievedByLrs)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }

            var originalViewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(model.RegistrationPathwayId);
            if (originalViewModel != null)
            {
                // Keep the original MathsStatus for display in the "From" column
                model.MathsStatus = originalViewModel.MathsStatus;

                model.MathsStatusTo = SubjectStatus.Achieved;
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewChangesMathsStatus, new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/review-changes-maths-status/{pathwayId}", Name = RouteConstants.AdminReviewChangesMathsStatus)]
        public async Task<IActionResult> AdminReviewChangesMathsStatusAsync(int pathwayId)
        {
            AdminReviewChangesMathsStatusViewModel viewModel = new();

            var cachedModel = await _cacheService.GetAsync<AdminChangeMathsStatusViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.AdminChangeStatusViewModel = cachedModel;

            await _cacheService.SetAsync<AdminReviewChangesMathsStatusViewModel>(CacheKey, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-maths-status", Name = RouteConstants.SubmitReviewChangesMathsStatus)]
        public async Task<IActionResult> AdminReviewChangesMathsStatusAsync(AdminReviewChangesMathsStatusViewModel model)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeMathsStatusViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            model.AdminChangeStatusViewModel = cachedModel;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isSuccess = await _loader.ProcessChangeMathsStatusAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = ReviewChangesMathsStatus.Message_Notification_Success,
                    IsRawHtml = true,
                }, CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.AdminChangeStatusViewModel.RegistrationPathwayId });
            }
            else
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }
        }

        #endregion

        #region Change English status

        [HttpGet]
        [Route("admin/change-english-status-clear/{registrationPathwayId}", Name = RouteConstants.AdminChangeEnglishStatusClear)]
        public async Task<IActionResult> ChangeEnglishStatusClearAsync(int registrationPathwayId)
        {
            await _cacheService.RemoveAsync<AdminChangeEnglishStatusViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminChangeEnglishStatus, new { registrationPathwayId });
        }

        [HttpGet]
        [Route("admin/change-english-status/{registrationPathwayId}", Name = RouteConstants.AdminChangeEnglishStatus)]
        public async Task<IActionResult> AdminChangeEnglishStatusAsync(int registrationPathwayId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey);

            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeEnglishStatusViewModel>(registrationPathwayId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-change-english-status", Name = RouteConstants.SubmitAdminChangeEnglishStatus)]
        public async Task<IActionResult> AdminChangeEnglishStatusAsync(AdminChangeEnglishStatusViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(AdminChangeEnglishStatus), model);
            }

            if (model.EnglishStatusTo == SubjectStatus.NotAchieved || model.EnglishStatusTo == SubjectStatus.NotAchievedByLrs)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }

            var originalViewModel = await _loader.GetAdminLearnerRecordAsync<AdminChangeEnglishStatusViewModel>(model.RegistrationPathwayId);
            if (originalViewModel != null)
            {
                // Keep the original EnglishStatus for display in the "From" column
                model.EnglishStatus = originalViewModel.EnglishStatus;

                model.EnglishStatusTo = SubjectStatus.Achieved;
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewChangesEnglishStatus, new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/review-changes-english-status/{pathwayId}", Name = RouteConstants.AdminReviewChangesEnglishStatus)]
        public async Task<IActionResult> AdminReviewChangesEnglishStatusAsync(int pathwayId)
        {
            AdminReviewChangesEnglishStatusViewModel viewModel = new();

            var cachedModel = await _cacheService.GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.AdminChangeStatusViewModel = cachedModel;

            await _cacheService.SetAsync<AdminReviewChangesEnglishStatusViewModel>(CacheKey, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-english-status", Name = RouteConstants.SubmitReviewChangesEnglishStatus)]
        public async Task<IActionResult> AdminReviewChangesEnglishStatusAsync(AdminReviewChangesEnglishStatusViewModel model)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            model.AdminChangeStatusViewModel = cachedModel;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isSuccess = await _loader.ProcessChangeEnglishStatusAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = ReviewChangesEnglishStatus.Message_Notification_Success,
                    IsRawHtml = true,
                }, CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.AdminChangeStatusViewModel.RegistrationPathwayId });
            }
            else
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }
        }

        #endregion

        #region Assesment Entry

        [HttpGet]
        [Route("admin/add-assessment-entry-core/{registrationPathwayId}", Name = RouteConstants.AdminCoreComponentAssessmentEntry)]
        public async Task<IActionResult> AdminCoreComponentAssessmentEntry(int registrationPathwayId)
        {

            var cachedModel = await _cacheService.GetAsync<AdminCoreComponentViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }


            var viewModel = await _loader.GetAdminLearnerRecordWithCoreComponents(registrationPathwayId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/add-assessment-entry-specialism/{registrationPathwayId}/{specialismsId}", Name = RouteConstants.AdminOccupationalSpecialisAssessmentEntry)]
        public async Task<IActionResult> AdminOccupationalSpecialismAssessmentEntry(int registrationPathwayId, int specialismsId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOccupationalSpecialismViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            var viewModel = await _loader.GetAdminLearnerRecordWithOccupationalSpecialism(registrationPathwayId, specialismsId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-add-assessment-entry-core", Name = RouteConstants.SubmitCoreComponentAssessmentEntry)]
        public async Task<IActionResult> AdminCoreComponentAssessmentEntry(AdminCoreComponentViewModel model)
        {
            var adminCoreComponent = await _loader.GetAdminLearnerRecordWithCoreComponents(model.RegistrationPathwayId);
            if (!ModelState.IsValid)
            {
                return View(adminCoreComponent);
            }
            model.AssessmentDetails = adminCoreComponent.AssessmentDetails;
            model.ValidPathwayAssessmentSeries = adminCoreComponent.ValidPathwayAssessmentSeries;
            await _cacheService.SetAsync<AdminCoreComponentViewModel>(CacheKey, model);
            return RedirectToAction(nameof(RouteConstants.AdminReviewChangesCoreAssessmentEntry), new { registrationPathwayId = model.RegistrationPathwayId });

        }

        [HttpPost]
        [Route("admin/submit-add-assessment-entry-specialism", Name = RouteConstants.SubmitOccupationalSpecialisAssessmentEntry)]
        public async Task<IActionResult> AdminOccupationalSpecialismAssessmentEntry(AdminOccupationalSpecialismViewModel model)
        {
            var adminOccupationalSpecialism = await _loader.GetAdminLearnerRecordWithOccupationalSpecialism(model.RegistrationPathwayId, model.SpecialismAssessmentId);
            if (!ModelState.IsValid)
            {
                return View(adminOccupationalSpecialism);
            }
            model.AssessmentDetails = adminOccupationalSpecialism.AssessmentDetails;
            model.ValidPathwayAssessmentSeries = adminOccupationalSpecialism.ValidPathwayAssessmentSeries;
            await _cacheService.SetAsync<AdminOccupationalSpecialismViewModel>(CacheKey, model);
            return RedirectToAction(nameof(RouteConstants.AdminReviewChangesSpecialismAssessmentEntry), new { registrationPathwayId = model.RegistrationPathwayId });

        }

        [HttpGet]
        [Route("admin/review-changes-assesment-entry-core/{registrationPathwayId}", Name = RouteConstants.AdminReviewChangesCoreAssessmentEntry)]
        public async Task<IActionResult> AdminReviewChangesCoreAssessmentEntry(int registrationPathwayId)
        {
            AdminReviewChangesCoreAssessmentViewModel viewModel = new();
            var _cachedModel = await _cacheService.GetAsync<AdminCoreComponentViewModel>(CacheKey);
            if (_cachedModel is null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.AdminCoreComponentViewModel = _cachedModel;
            await _cacheService.SetAsync<AdminReviewChangesCoreAssessmentViewModel>(CacheKey, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-assessment-entry-core", Name = RouteConstants.SubmitReviewChangesCoreAssessmentEntry)]
        public async Task<IActionResult> AdminReviewChangesCoreAssessmentEntry(AdminReviewChangesCoreAssessmentViewModel model)
        {
            var cachedModel = await _cacheService.GetAsync<AdminReviewChangesCoreAssessmentViewModel>(CacheKey);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isSuccess = await _loader.ProcessAddCoreAssessmentRequestAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = ReviewChangeAssessment.Message_Notification_Success,
                    IsRawHtml = true,
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.AdminCoreComponentViewModel.RegistrationPathwayId });
            }
            else { return RedirectToAction(RouteConstants.ProblemWithService); }
        }

        [HttpGet]
        [Route("admin/review-changes-assesment-entry-specialism/{registrationPathwayId}", Name = RouteConstants.AdminReviewChangesSpecialismAssessmentEntry)]
        public async Task<IActionResult> AdminReviewChangesSpecialismAssessmentEntry(int registrationPathwayId)
        {
            AdminReviewChangesSpecialismAssessmentViewModel viewModel = new();
            var _cachedModel = await _cacheService.GetAsync<AdminOccupationalSpecialismViewModel>(CacheKey);
            if (_cachedModel is null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.AdminOccupationalSpecialismViewModel = _cachedModel;

            await _cacheService.SetAsync<AdminReviewChangesSpecialismAssessmentViewModel>(CacheKey, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/submit-review-changes-assessment-entry-specialism", Name = RouteConstants.SubmitReviewChangesSpecialismAssessmentEntry)]
        public async Task<IActionResult> AdminReviewChangesSpecialismAssessmentEntry(AdminReviewChangesSpecialismAssessmentViewModel model)
        {
            var cachedModel = await _cacheService.GetAsync<AdminReviewChangesSpecialismAssessmentViewModel>(CacheKey);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = await _loader.ProcessAddSpecialismAssessmentRequestAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = ReviewChangeAssessment.Message_Notification_Success,
                    IsRawHtml = true,
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.AdminOccupationalSpecialismViewModel.RegistrationPathwayId });
            }
            else { return RedirectToAction(RouteConstants.ProblemWithService); }

        }

        [HttpGet]
        [Route("admin/add-assessment-core-entry-clear/{registrationPathwayId}", Name = RouteConstants.AdminCoreComponentAssessmentEntryClear)]
        public async Task<IActionResult> AdminCoreComponentAssessmentEntryClearAsync(int registrationPathwayId)
        {
            await _cacheService.RemoveAsync<AdminCoreComponentViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminCoreComponentAssessmentEntry, new { registrationPathwayId });
        }

        [HttpGet]
        [Route("admin/add-assessment-specialism-entry-clear/{registrationPathwayId}/{specialismsId}", Name = RouteConstants.AdminOccupationalSpecialisAssessmentEntryClear)]
        public async Task<IActionResult> AdminOccupationalSpecialisAssessmentEntryClearAsync(int registrationPathwayId, int specialismsId)
        {
            await _cacheService.RemoveAsync<AdminOccupationalSpecialismViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminOccupationalSpecialisAssessmentEntry, new { registrationPathwayId, specialismsId });
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
                await _cacheService.RemoveAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewRemoveCoreAssessmentEntry);
        }

        [HttpGet]
        [Route("admin/review-remove-assessment-entry-core", Name = RouteConstants.AdminReviewRemoveCoreAssessmentEntry)]
        public async Task<IActionResult> AdminReviewRemoveCoreAssessmentEntryAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminRemovePathwayAssessmentEntryViewModel>(CacheKey);

            if (cachedModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            AdminReviewRemoveCoreAssessmentEntryViewModel viewModel = new()
            {
                PathwayAssessmentViewModel = cachedModel
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-remove-assessment-entry-core", Name = RouteConstants.SubmitReviewRemoveCoreAssessmentEntry)]
        public async Task<IActionResult> AdminReviewRemoveCoreAssessmentEntryAsync(AdminReviewRemoveCoreAssessmentEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = await _loader.ProcessRemoveAssessmentEntry(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = AdminReviewRemoveAssessmentEntry.Message_Notification_Success,
                    IsRawHtml = true,
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.PathwayAssessmentViewModel.RegistrationPathwayId });
            }
            else
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }
        }

        [HttpGet]
        [Route("admin/remove-assessment-entry-specialism-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.RemoveAssessmentSpecialismEntryClear)]
        public async Task<IActionResult> RemoveAssessmentEntrySpecialismClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
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
                await _cacheService.RemoveAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewRemoveSpecialismAssessmentEntry, new { registrationPathwayId = model.RegistrationPathwayId.ToString() });
        }

        [HttpGet]
        [Route("admin/review-remove-assessment-entry-specialism/{registrationPathwayId}", Name = RouteConstants.AdminReviewRemoveSpecialismAssessmentEntry)]
        public async Task<IActionResult> AdminReviewRemoveSpecialismAssessmentEntryAsync(int registrationPathwayId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);

            if (cachedModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            AdminReviewRemoveSpecialismAssessmentEntryViewModel viewModel = new()
            {
                PathwayAssessmentViewModel = cachedModel
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-remove-assessment-entry-specialism", Name = RouteConstants.SubmitReviewRemoveSpecialismAssessmentEntry)]
        public async Task<IActionResult> AdminReviewRemoveSpecialismAssessmentEntryAsync(AdminReviewRemoveSpecialismAssessmentEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSuccess = await _loader.ProcessRemoveSpecialismAssessmentEntryAsync(model);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new NotificationBannerModel
                {
                    DisplayMessageBody = true,
                    Message = AdminReviewRemoveAssessmentEntry.Message_Notification_Success,
                    IsRawHtml = true,
                },
                CacheExpiryTime.XSmall);

                return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.PathwayAssessmentViewModel.RegistrationPathwayId });
            }
            else
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }
        }

        #endregion

        #region Add pathway result

        [HttpGet]
        [Route("admin/add-assessment-result-core-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddPathwayResultClear)]
        public async Task<IActionResult> AdminAddPathwayResultClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddPathwayResultViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddPathwayResult, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/add-assessment-result-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddPathwayResult)]
        public async Task<IActionResult> AdminAddPathwayResultAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddPathwayResultViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddPathwayResultViewModel viewModel = await _loader.GetAdminAddPathwayResultAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AddResultCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-assessment-result-core", Name = RouteConstants.SubmitAdminAddPathwayResult)]
        public async Task<IActionResult> AdminAddPathwayResultAsync(AdminAddPathwayResultViewModel model)
        {
            await _loader.LoadAdminAddPathwayResultGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminAddPathwayResultReviewChanges);
        }

        [HttpGet]
        [Route("admin/review-changes-assessment-result-core", Name = RouteConstants.AdminAddPathwayResultReviewChanges)]
        public async Task<IActionResult> AdminAddPathwayResultReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddPathwayResultViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminAddPathwayResultReviewChangesViewModel viewModel = _loader.CreateAdminAddPathwayResultReviewChanges(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-assessment-result-core", Name = RouteConstants.SubmitAdminAddPathwayResultReviewChanges)]
        public async Task<IActionResult> AdminAddPathwayResultReviewChangesAsync(AdminAddPathwayResultReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAddPathwayResultReviewChangesAsync(model);
            if (!success)
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminAddPathwayResultReviewChanges.Notification_Message_Asessment_Result_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        #region Add specialism result

        [HttpGet]
        [Route("admin/add-assessment-result-specialism-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddSpecialismResultClear)]
        public async Task<IActionResult> AdminAddSpecialismResultClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddSpecialismResultViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddSpecialismResult, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/add-assessment-result-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddSpecialismResult)]
        public async Task<IActionResult> AdminAddSpecialismResultAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddSpecialismResultViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddSpecialismResultViewModel viewModel = await _loader.GetAdminAddSpecialismResultAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminAddSpecialismResultAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-assessment-result-specialism", Name = RouteConstants.SubmitAdminAddSpecialismResult)]
        public async Task<IActionResult> AdminAddSpecialismResultAsync(AdminAddSpecialismResultViewModel model)
        {
            await _loader.LoadAdminAddSpecialismResultGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminAddSpecialismResultReviewChanges);
        }

        [HttpGet]
        [Route("admin/review-changes-assessment-result-specialism", Name = RouteConstants.AdminAddSpecialismResultReviewChanges)]
        public async Task<IActionResult> AdminAddSpecialismResultReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddSpecialismResultViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminAddSpecialismResultReviewChangesViewModel viewModel = _loader.CreateAdminAddSpecialismResultReviewChanges(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-assessment-result-specialism", Name = RouteConstants.SubmitAdminAddSpecialismResultReviewChanges)]
        public async Task<IActionResult> AdminAddSpecialismResultReviewChangesAsync(AdminAddSpecialismResultReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAddSpecialismResultReviewChangesAsync(model);
            if (!success)
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminAddSpecialismResultReviewChanges.Notification_Message_Asessment_Result_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        #region Change result

        [HttpGet]
        [Route("admin/change-assessment-result-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminChangePathwayResult)]
        public async Task<IActionResult> AdminChangePathwayResultAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangePathwayResultViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminChangePathwayResultViewModel viewModel = await _loader.GetAdminChangePathwayResultAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AddResultCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/change-assessment-result-core", Name = RouteConstants.SubmitAdminChangePathwayResult)]
        public async Task<IActionResult> AdminChangePathwayResultAsync(AdminChangePathwayResultViewModel model)
        {
            await _loader.LoadAdminChangePathwayResultGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminChangePathwayResultReviewChanges);
        }

        [HttpGet]
        [Route("admin/change-assessment-result-core-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminChangePathwayResultClear)]
        public async Task<IActionResult> AdminChangePathwayResultClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminChangePathwayResultViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminChangePathwayResult, new { registrationPathwayId, assessmentId });
        }


        [HttpGet]
        [Route("admin/change-assessment-result-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminChangeSpecialismResult)]
        public async Task<IActionResult> AdminChangeSpecialismResultAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminChangeSpecialismResultViewModel viewModel = await _loader.GetAdminChangeSpecialismResultAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AddResultCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }


        [HttpPost]
        [Route("admin/change-assessment-result-specialism", Name = RouteConstants.SubmitAdminChangeSpecialismResult)]
        public async Task<IActionResult> AdminChangeSpecialismResultAsync(AdminChangeSpecialismResultViewModel model)
        {
            await _loader.LoadAdminChangeSpecialismResultGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(nameof(RouteConstants.AdminChangeSpecialismResultReviewChanges));
        }

        [HttpGet]
        [Route("admin/change-assessment-result-specialism-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminChangeSpecialismResultClear)]
        public async Task<IActionResult> AdminChangeSpecialismResultClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminChangeSpecialismResultViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminChangeSpecialismResult, new { registrationPathwayId, assessmentId });
        }


        #endregion

        #region Review Change Pathway Result
        [HttpGet]
        [Route("admin/change-assessment-result-core-review-changes", Name = RouteConstants.AdminChangePathwayResultReviewChanges)]
        public async Task<IActionResult> AdminChangePathwayResultReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangePathwayResultViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminChangePathwayResultReviewChangesViewModel viewModel = _loader.CreateAdminChangePathwayResultReviewChanges(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/change-assessment-result-core-review-changes", Name = RouteConstants.SubmitAdminChangePathwayResultReviewChanges)]
        public async Task<IActionResult> AdminChangePathwayResultReviewChangesAsync(AdminChangePathwayResultReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessChangePathwayResultReviewChangesAsync(model);
            if (!success)
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminChangePathwayResultReviewChanges.Notification_Message_Asessment_Result_Changed);
            await _cacheService.SetAsync<NotificationBannerModel>(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }


        #endregion

        [HttpGet]
        [Route("admin/change-assessment-result-specialism-review-changes", Name = RouteConstants.AdminChangeSpecialismResultReviewChanges)]
        public async Task<IActionResult> AdminChangeSpecialismResultReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminChangeSpecialismResultViewModel>(CacheKey);

            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminChangeSpecialismResultReviewChangesViewModel viewModel = _loader.CreateAdminChangeSpecialismResultReviewChanges(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/change-assessment-result-specialism-review-changes", Name = RouteConstants.SubmitAdminChangeSpecialismResultReviewChanges)]
        public async Task<IActionResult> AdminChangeSpecialismResultReviewChangesAsync(AdminChangeSpecialismResultReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessChangeSpecialismResultReviewChangesAsync(model);
            if (!success)
            {
                return RedirectToAction(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminChangeSpecialismResultReviewChanges.Notification_Message_Asessment_Result_Updated);
            await _cacheService.SetAsync<NotificationBannerModel>(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/request-replacement-document/{registrationPathwayId}", Name = RouteConstants.AdminRequestReplacementDocument)]
        public async Task<IActionResult> AdminRequestReplacementDocumentAsync(int registrationPathwayId)
        {
            var viewModel = await _loader.GetAdminLearnerRecordAsync<AdminRequestReplacementDocumentViewModel>(registrationPathwayId);

            if (viewModel == null || !viewModel.IsCertificateRerequestEligible)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/request-replacement-document", Name = RouteConstants.SubmitAdminRequestReplacementDocument)]
        public async Task<IActionResult> AdminRequestReplacementDocumentAsync(AdminRequestReplacementDocumentViewModel model)
        {
            var isSuccess = await _loader.CreateReplacementDocumentPrintingRequestAsync(model);

            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new AdminNotificationBannerModel(AdminRequestReplacementDocument.Success_Header_Replacement_Document_Requested);
            await _cacheService.SetAsync<NotificationBannerModel>(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToAction(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }
    }
}