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
            => AdminOpenPathwayPostResultClearAsync<AdminOpenPathwayRommViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenPathwayRomm);

        [HttpGet]
        [Route("admin/open-romm-core", Name = RouteConstants.AdminOpenPathwayRomm)]
        public Task<IActionResult> AdminOpenPathwayRommAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenPathwayRommAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayRommAsync));

        [HttpPost]
        [Route("admin/open-romm-core", Name = RouteConstants.SubmitAdminOpenPathwayRomm)]
        public Task<IActionResult> AdminOpenPathwayRommAsync(AdminOpenPathwayRommViewModel model)
            => AdminOpenPathwayPostResultAsync(model, model.RegistrationPathwayId, model.DoYouWantToOpenRomm, RouteConstants.AdminOpenPathwayRommReviewChanges);

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
            => AdminOpenPathwayPostResultClearAsync<AdminOpenSpecialismRommViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenSpecialismRomm);

        [HttpGet]
        [Route("admin/open-romm-specialism", Name = RouteConstants.AdminOpenSpecialismRomm)]
        public Task<IActionResult> AdminOpenSpecialismRommAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenSpecialismRommAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenSpecialismRommAsync));

        [HttpPost]
        [Route("admin/open-romm-specialism", Name = RouteConstants.SubmitAdminOpenSpecialismRomm)]
        public Task<IActionResult> AdminOpenSpecialismRommAsync(AdminOpenSpecialismRommViewModel model)
            => AdminOpenPathwayPostResultAsync(model, model.RegistrationPathwayId, model.DoYouWantToOpenRomm, RouteConstants.AdminOpenSpecialismRommReviewChanges);

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

        #region Add ROMM outcome Pathway

        [HttpGet]
        [Route("admin/add-romm-outcome-core-clear", Name = RouteConstants.AdminAddCoreRommOutcomeClear)]
        public async Task<IActionResult> AdminAddCoreRommOutcomeClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddCoreRommOutcome, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/add-romm-outcome-core", Name = RouteConstants.AdminAddCoreRommOutcome)]
        public async Task<IActionResult> AdminAddCoreRommOutcomeAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddCoreRommOutcomeViewModel viewModel = await _loader.GetAdminAddPathwayRommOutcomeAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminAddCoreRommOutcomeAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

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
            return RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId }); //Todo re-direct to follow-up page.
        }

        #endregion

        #region Add ROMM outcome Specialism

        [HttpGet]
        [Route("admin/add-romm-outcome-specialism-clear", Name = RouteConstants.AdminAddSpecialismRommOutcomeClear)]
        public async Task<IActionResult> AdminAddSpecialismRommOutcomeClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAddSpecialismRommOutcome, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/add-romm-outcome-specialism", Name = RouteConstants.AdminAddSpecialismRommOutcome)]
        public async Task<IActionResult> AdminAddSpecialismRommOutcomeAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminAddSpecialismRommOutcomeViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminAddSpecialismRommOutcomeViewModel viewModel = await _loader.GetAdminAddSpecialismRommOutcomeAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminAddSpecialismRommOutcomeAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

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
            return RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId }); //Todo re-direct to follow-up page.
        }

        #endregion

        #region Open Appeal Pathway

        [HttpGet]
        [Route("admin/open-appeal-core-clear", Name = RouteConstants.AdminOpenPathwayAppealClear)]
        public Task<IActionResult> AdminOpenPathwayAppealClearAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultClearAsync<AdminOpenPathwayAppealViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenPathwayAppeal);

        [HttpGet]
        [Route("admin/open-appeal-core", Name = RouteConstants.AdminOpenPathwayAppeal)]
        public Task<IActionResult> AdminOpenPathwayAppealAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenPathwayAppealAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayAppealAsync));

        #endregion

        #region Open Appeal Specialism

        [HttpGet]
        [Route("admin/open-appeal-specialism-clear", Name = RouteConstants.AdminOpenSpecialismAppealClear)]
        public Task<IActionResult> AdminOpenSpecialismAppealClearAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultClearAsync<AdminOpenSpecialismAppealViewModel>(registrationPathwayId, assessmentId, RouteConstants.AdminOpenSpecialismAppeal);

        [HttpGet]
        [Route("admin/open-appeal-specialism", Name = RouteConstants.AdminOpenSpecialismAppeal)]
        public Task<IActionResult> AdminOpenSpecialismAppealAsync(int registrationPathwayId, int assessmentId)
            => AdminOpenPathwayPostResultAsync(registrationPathwayId, assessmentId, () => _loader.GetAdminOpenSpecialismAppealAsync(registrationPathwayId, assessmentId), nameof(AdminPostResultsController.AdminOpenPathwayAppealAsync));

        #endregion

        private async Task<IActionResult> AdminOpenPathwayPostResultClearAsync<T>(int registrationPathwayId, int assessmentId, string toRoute)
        {
            await _cacheService.RemoveAsync<T>(CacheKey);
            return RedirectToRoute(toRoute, new { registrationPathwayId, assessmentId });
        }

        private async Task<IActionResult> AdminOpenPathwayPostResultAsync<T>(int registrationPathwayId, int assessmentId, Func<Task<T>> loaderFunc, string methodName)
        {
            var cachedModel = await _cacheService.GetAsync<T>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            T viewModel = await loaderFunc();
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: {methodName}({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> AdminOpenPathwayPostResultAsync<T>(T model, int registrationPathwayId, bool? doYouWantToOpen, string toRoute)
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
            return RedirectToRoute(toRoute);
        }
    }
}