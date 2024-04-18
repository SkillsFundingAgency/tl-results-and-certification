using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminPostResultsController : Controller
    {
        private readonly IAdminPostResultsLoader _loader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminPostResultsCacheKey);

        public AdminPostResultsController(
            IAdminPostResultsLoader loader,
            ICacheService cacheService,
            ILogger<AdminPostResultsController> logger)
        {
            _loader = loader;
            _cacheService = cacheService;
            _logger = logger;
        }

        #region Open ROMM Pathway

        [HttpGet]
        [Route("admin/open-romm-core-clear", Name = RouteConstants.AdminOpenPathwayRommClear)]
        public Task<IActionResult> AdminOpenPathwayRommClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminOpenPathwayRommViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenPathwayRomm);

        [HttpGet]
        [Route("admin/open-romm-core", Name = RouteConstants.AdminOpenPathwayRomm)]
        public Task<IActionResult> AdminOpenPathwayRommAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenPathwayRommAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayRommAsync));

        [HttpPost]
        [Route("admin/open-romm-core", Name = RouteConstants.SubmitAdminOpenPathwayRomm)]
        public Task<IActionResult> AdminOpenPathwayRommAsync(AdminOpenPathwayRommViewModel model)
            => PostAdminPostResultAsync(model, model.RegistrationPathwayId, model.PathwayAssessmentId, model.DoYouWantToOpenRomm, RouteConstants.AdminOpenPathwayRommReviewChanges);

        [HttpGet]
        [Route("admin/review-changes-romm-core", Name = RouteConstants.AdminOpenPathwayRommReviewChanges)]
        public async Task<IActionResult> AdminOpenPathwayRommReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminOpenPathwayRommReviewChangesViewModel viewModel = _loader.GetAdminOpenPathwayRommReviewChangesAsync(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-romm-core", Name = RouteConstants.SubmitAdminOpenPathwayRommReviewChanges)]
        public async Task<IActionResult> AdminOpenPathwayRommReviewChangesAsync(AdminOpenPathwayRommReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminOpenPathwayRommAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminOpenPathwayRommReviewChanges.Notification_Message_A_Romm_Has_Been_Opened);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        #region Open ROMM Specialism

        [HttpGet]
        [Route("admin/open-romm-specialism-clear", Name = RouteConstants.AdminOpenSpecialismRommClear)]
        public Task<IActionResult> AdminOpenSpecialismRommClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminOpenSpecialismRommViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenSpecialismRomm);

        [HttpGet]
        [Route("admin/open-romm-specialism", Name = RouteConstants.AdminOpenSpecialismRomm)]
        public Task<IActionResult> AdminOpenSpecialismRommAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenSpecialismRommAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenSpecialismRommAsync));

        [HttpPost]
        [Route("admin/open-romm-specialism", Name = RouteConstants.SubmitAdminOpenSpecialismRomm)]
        public Task<IActionResult> AdminOpenSpecialismRommAsync(AdminOpenSpecialismRommViewModel model)
            => PostAdminPostResultAsync(model, model.RegistrationPathwayId, model.SpecialismAssessmentId, model.DoYouWantToOpenRomm, RouteConstants.AdminOpenSpecialismRommReviewChanges);

        [HttpGet]
        [Route("admin/review-changes-romm-specialism", Name = RouteConstants.AdminOpenSpecialismRommReviewChanges)]
        public async Task<IActionResult> AdminOpenSpecialismRommReviewChangesAsync()
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey);
            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminOpenSpecialismRommReviewChangesViewModel viewModel = _loader.GetAdminOpenSpecialismRommReviewChangesAsync(cachedModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-romm-specialism", Name = RouteConstants.SubmitAdminOpenSpecialismRommReviewChanges)]
        public async Task<IActionResult> AdminOpenSpecialismRommReviewChangesAsync(AdminOpenSpecialismRommReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminOpenSpecialismRommAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminOpenSpecialismRommReviewChanges.Notification_Message_A_Romm_Has_Been_Opened);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        #region Add ROMM Outcome Pathway

        [HttpGet]
        [Route("admin/add-romm-outcome-core-clear", Name = RouteConstants.AdminAddCoreRommOutcomeClear)]
        public Task<IActionResult> AdminAddCoreRommOutcomeClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminAddCoreRommOutcomeViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminAddCoreRommOutcome);

        [HttpGet]
        [Route("admin/add-romm-outcome-core", Name = RouteConstants.AdminAddCoreRommOutcome)]
        public Task<IActionResult> AdminAddCoreRommOutcomeAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminAddPathwayRommOutcomeAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminAddCoreRommOutcomeAsync));

        [HttpPost]
        [Route("admin/add-romm-outcome-core", Name = RouteConstants.SubmitAddCoreRommOutcome)]
        public async Task<IActionResult> AdminAddCoreRommOutcomeAsync(AdminAddCoreRommOutcomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool hasGradeChanged = model.WhatIsRommOutcome.HasValue && model.WhatIsRommOutcome.Value == false;
            await _cacheService.SetAsync(CacheKey, model);
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeCoreClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.PathwayAssessmentId }) :
             RedirectToRoute(RouteConstants.AdminReviewChangesRommOutcomeCore, new { isSameGrade = true });
        }

        #endregion

        #region Add ROMM Outcome Specialism

        [HttpGet]
        [Route("admin/add-romm-outcome-specialism-clear", Name = RouteConstants.AdminAddSpecialismRommOutcomeClear)]
        public Task<IActionResult> AdminAddSpecialismRommOutcomeClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminAddSpecialismRommOutcomeViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminAddSpecialismRommOutcome);

        [HttpGet]
        [Route("admin/add-romm-outcome-specialism", Name = RouteConstants.AdminAddSpecialismRommOutcome)]
        public Task<IActionResult> AdminAddSpecialismRommOutcomeAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminAddSpecialismRommOutcomeAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminAddSpecialismRommOutcomeAsync));

        [HttpPost]
        [Route("admin/add-romm-outcome-specialism", Name = RouteConstants.SubmitAddSpecialismRommOutcome)]
        public async Task<IActionResult> AdminAddSpecialismRommOutcomeAsync(AdminAddSpecialismRommOutcomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool hasGradeChanged = model.WhatIsRommOutcome.HasValue && model.WhatIsRommOutcome.Value == false;


            await _cacheService.SetAsync(CacheKey, model);
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeSpecialismClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.SpecialismAssessmentId }) :
                RedirectToRoute(RouteConstants.AdminReviewChangesRommOutcomeSpecialism, new { isSameGrade = true });
        }

        #endregion

        #region Change ROMM Grade Pathway

        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-core-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddRommOutcomeChangeGradeCoreClear)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeCoreClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeCore, new { registrationPathwayId, assessmentId });
        }


        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddRommOutcomeChangeGradeCore)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeCoreAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddRommOutcomeChangeGradeCoreViewModel viewModel = await _loader.GetAdminAddRommOutcomeChangeGradeCoreAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AddResultCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-romm-outcome-change-grade-core", Name = RouteConstants.SubmitAdminAddRommOutcomeChangeGradeCore)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeCoreAsync(AdminAddRommOutcomeChangeGradeCoreViewModel model)
        {
            await _loader.LoadAdminAddRommOutcomeChangeGradeCoreGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewChangesRommOutcomeCore);
        }

        #endregion

        #region Change ROMM Grade Specialism

        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-specialism-clear/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddRommOutcomeChangeGradeSpecialismClear)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeSpecialismClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeSpecialism, new { registrationPathwayId, assessmentId });
        }


        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminAddRommOutcomeChangeGradeSpecialism)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeSpecialismAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddRommOutcomeChangeGradeSpecialismViewModel viewModel = await _loader.GetAdminAddRommOutcomeChangeGradeSpecialismAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AddResultCoreAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-romm-outcome-change-grade-specialism", Name = RouteConstants.SubmitAdminAddRommOutcomeChangeGradeSpecialism)]
        public async Task<IActionResult> AdminAddRommOutcomeChangeGradeSpecialismAsync(AdminAddRommOutcomeChangeGradeSpecialismViewModel model)
        {
            await _loader.LoadAdminAddRommOutcomeChangeGradeSpecialismGrades(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminReviewChangesRommOutcomeSpecialism);
        }

        #endregion

        #region Open Appeal Pathway

        [HttpGet]
        [Route("admin/open-appeal-core-clear", Name = RouteConstants.AdminOpenPathwayAppealClear)]
        public Task<IActionResult> AdminOpenPathwayAppealClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminOpenPathwayAppealViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenPathwayAppeal);

        [HttpGet]
        [Route("admin/open-appeal-core", Name = RouteConstants.AdminOpenPathwayAppeal)]
        public Task<IActionResult> AdminOpenPathwayAppealAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenPathwayAppealAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayAppealAsync));

        [HttpPost]
        [Route("admin/open-appeal-core", Name = RouteConstants.SubmitAdminOpenPathwayAppeal)]
        public Task<IActionResult> AdminOpenPathwayAppealAsync(AdminOpenPathwayAppealViewModel model)
            => PostAdminPostResultAsync(model, model.RegistrationPathwayId, model.PathwayAssessmentId, model.DoYouWantToOpenAppeal, RouteConstants.AdminOpenPathwayAppealReviewChanges);

        [HttpGet]
        [Route("admin/review-changes-appeal-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminOpenPathwayAppealReviewChanges)]
        public async Task<IActionResult> AdminReviewCoreAppealChangesAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey);
            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminAppealCoreReviewChangesViewModel viewModel = await _loader.GetAdminAppealCoreReviewChangesAsync(registrationPathwayId, assessmentId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-appeal-core/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.SubmitAdminOpenPathwayAppealReviewChanges)]
        public async Task<IActionResult> AdminReviewCoreAppealChangesAsync(AdminAppealCoreReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminOpenCoreAppealAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminReviewCoreAppealChanges.Notification_Message_Core_Appeal_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        #region Open Appeal Specialism

        [HttpGet]
        [Route("admin/open-appeal-specialism-clear", Name = RouteConstants.AdminOpenSpecialismAppealClear)]
        public Task<IActionResult> AdminOpenSpecialismAppealClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminOpenSpecialismAppealViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenSpecialismAppeal);

        [HttpGet]
        [Route("admin/open-appeal-specialism", Name = RouteConstants.AdminOpenSpecialismAppeal)]
        public Task<IActionResult> AdminOpenSpecialismAppealAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenSpecialismAppealAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayAppealAsync));

        [HttpPost]
        [Route("admin/open-appeal-specialism", Name = RouteConstants.SubmitAdminOpenSpecialismAppeal)]
        public Task<IActionResult> AdminOpenSpecialismAppealAsync(AdminOpenSpecialismAppealViewModel model)
            => PostAdminPostResultAsync(model, model.RegistrationPathwayId, model.SpecialismAssessmentId, model.DoYouWantToOpenAppeal, RouteConstants.AdminOpenSpecialismAppealReviewChanges);

        [HttpGet]
        [Route("admin/review-changes-appeal-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.AdminOpenSpecialismAppealReviewChanges)]
        public async Task<IActionResult> AdminReviewSpecialismAppealChangesAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
            if (cachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminAppealSpecialismReviewChangesViewModel viewModel = await _loader.GetAdminAppealSpecialismReviewChangesAsync(registrationPathwayId, assessmentId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/review-changes-appeal-specialism/{registrationPathwayId}/{assessmentId}", Name = RouteConstants.SubmitAdminOpenSpecialismAppealReviewChanges)]
        public async Task<IActionResult> AdminReviewSpecialismAppealChangesAsync(AdminAppealSpecialismReviewChangesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminOpenSpecialismAppealAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminReviewCoreAppealChanges.Notification_Message_Core_Appeal_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        #endregion

        private async Task<IActionResult> GetAdminPostResultClearAsync<T>(int registrationPathwayId, int assessmentId, string toRoute)
        {
            await _cacheService.RemoveAsync<T>(CacheKey);
            return RedirectToRoute(toRoute, new { registrationPathwayId, assessmentId });
        }

        private async Task<IActionResult> GetAdminPostResultAsync<T>(int registrationPathwayId, int assessmentId, Func<Task<T>> loaderFunc, string methodName)
        {
            var cachedModel = await _cacheService.GetAsync<T>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            T viewModel = await loaderFunc();
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result details found. Method: {methodName}({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        private async Task<IActionResult> PostAdminPostResultAsync<T>(T model, int registrationPathwayId, int assessmentId, bool? doYouWantToOpen, string toRoute)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool noSelected = doYouWantToOpen.HasValue && doYouWantToOpen.Value == false;
            if (noSelected)
            {
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = registrationPathwayId });
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(toRoute, new { registrationPathwayId, assessmentId });
        }

        #region Review Change romm
        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-review-changes-core", Name = RouteConstants.AdminReviewChangesRommOutcomeCore)]
        public async Task<IActionResult> AdminReviewChangesRommOutcomeCoreAsync(bool isSameGrade)
        {
            var changeGradeCachedModel = await _cacheService.GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            var addRommcachedModel = await _cacheService.GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey);
            if (changeGradeCachedModel == null && addRommcachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminReviewChangesRommOutcomeCoreViewModel viewModel = isSameGrade ? _loader.GetAdminReviewChangesRommOutcomeCoreAsync(addRommcachedModel) : _loader.GetAdminReviewChangesRommOutcomeCoreAsync(changeGradeCachedModel);
            viewModel.IsSameGrade = isSameGrade;
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-romm-outcome-change-grade-review-changes-core", Name = RouteConstants.SubmitAdminReviewChangesRommOutcomeCore)]
        public async Task<IActionResult> AdminReviewChangesRommOutcomeCoreAsync(AdminReviewChangesRommOutcomeCoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminReviewChangesRommOutcomeCoreAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminReviewChangesRommOutcomeCore.Notification_Message_Romm_Outcome_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }

        [HttpGet]
        [Route("admin/add-romm-outcome-change-grade-review-changes-specialism", Name = RouteConstants.AdminReviewChangesRommOutcomeSpecialism)]
        public async Task<IActionResult> AdminReviewChangesRommOutcomeSpecialismAsync(bool isSameGrade = false)
        {
            var changeGradeCachedModel = await _cacheService.GetAsync<AdminAddRommOutcomeChangeGradeSpecialismViewModel>(CacheKey);
            var addRommcachedModel = await _cacheService.GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey);

            if (changeGradeCachedModel == null && addRommcachedModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            AdminReviewChangesRommOutcomeSpecialismViewModel viewModel = isSameGrade ? _loader.GetAdminReviewChangesRommOutcomeSpecialismAsync(addRommcachedModel) : _loader.GetAdminReviewChangesRommOutcomeSpecialismAsync(changeGradeCachedModel);
            viewModel.IsSameGrade = isSameGrade;
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/add-romm-outcome-change-grade-review-changes-specialism", Name = RouteConstants.SubmitAdminReviewChangesRommOutcomeSpecialism)]
        public async Task<IActionResult> AdminReviewChangesRommOutcomeSpecialismAsync(AdminReviewChangesRommOutcomeSpecialismViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool success = await _loader.ProcessAdminReviewChangesRommOutcomeSpecialismAsync(model);
            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            string adminDashboardCacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

            var notificationBanner = new AdminNotificationBannerModel(AdminReviewChangesRommOutcomeSpecialism.Notification_Message_Romm_Outcome_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(adminDashboardCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
        }


        #endregion

        #region Add Appeal Outcome Pathway

        [HttpGet]
        [Route("admin/add-appeal-outcome-core-clear", Name = RouteConstants.AdminAddCoreAppealOutcomeClear)]
        public Task<IActionResult> AdminAddCoreAppealOutcomeClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminAddCoreAppealOutcomeViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminAddCoreAppealOutcome);

        [HttpGet]
        [Route("admin/add-appeal-outcome-core", Name = RouteConstants.AdminAddCoreAppealOutcome)]
        public Task<IActionResult> AdminAddCoreAppealOutcomeAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminAddPathwayAppealOutcomeAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminAddCoreRommOutcomeAsync));

        [HttpPost]
        [Route("admin/add-appeal-outcome-core", Name = RouteConstants.SubmitAddCoreAppealOutcome)]
        public async Task<IActionResult> AdminAddCoreAppealOutcomeAsync(AdminAddCoreAppealOutcomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool hasGradeChanged = model.WhatIsAppealOutcome.HasValue && model.WhatIsAppealOutcome.Value == false;
            await _cacheService.SetAsync(CacheKey, model);
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddAppealOutcomeChangeGradeCoreClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.PathwayAssessmentId }) :
             RedirectToRoute(RouteConstants.AdminReviewChangesAppealOutcomeCore, new { isSameGrade = true });
        }

        #endregion

        #region Add Appeal Outcome Specialism

        [HttpGet]
        [Route("admin/add-appeal-outcome-specialism-clear", Name = RouteConstants.AdminAddSpecialismAppealOutcomeClear)]
        public Task<IActionResult> AdminAddSpecialismAppealOutcomeClearAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultClearAsync<AdminAddSpecialismAppealOutcomeViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminAddSpecialismAppealOutcome);

        [HttpGet]
        [Route("admin/add-appeal-outcome-specialism", Name = RouteConstants.AdminAddSpecialismAppealOutcome)]
        public Task<IActionResult> AdminAddSpecialismAppealOutcomeAsync(int registrationPathwayId, int assessmentId)
            => GetAdminPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminAddSpecialismAppealOutcomeAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminAddSpecialismRommOutcomeAsync));

        [HttpPost]
        [Route("admin/add-appeal-outcome-specialism", Name = RouteConstants.SubmitAddSpecialismAppealOutcome)]
        public async Task<IActionResult> AdminAddSpecialismAppealOutcomeAsync(AdminAddSpecialismAppealOutcomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool hasGradeChanged = model.WhatIsAppealOutcome.HasValue && model.WhatIsAppealOutcome.Value == false;


            await _cacheService.SetAsync(CacheKey, model);
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddAppealOutcomeChangeGradeSpecialismClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.SpecialismAssessmentId }) :
                RedirectToRoute(RouteConstants.AdminReviewChangesAppealOutcomeSpecialism, new { isSameGrade = true });
        }

        #endregion
    }
}