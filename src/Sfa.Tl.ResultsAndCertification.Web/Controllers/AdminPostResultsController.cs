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

        [HttpGet]
        [Route("admin/open-romm-core-clear", Name = RouteConstants.AdminOpenPathwayRommClear)]
        public async Task<IActionResult> AdminOpenPathwayRommClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminOpenPathwayRomm, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/open-romm-core", Name = RouteConstants.AdminOpenPathwayRomm)]
        public async Task<IActionResult> AdminOpenPathwayRommAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminOpenPathwayRommViewModel viewModel = await _loader.GetAdminOpenPathwayRommAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminOpenPathwayRommAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/open-romm-core", Name = RouteConstants.SubmitAdminOpenPathwayRomm)]
        public async Task<IActionResult> AdminOpenPathwayRommAsync(AdminOpenPathwayRommViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool noSelected = model.DoYouWantToOpenRomm.HasValue && model.DoYouWantToOpenRomm.Value == false;
            if (noSelected)
            {
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminOpenPathwayRommReviewChanges);
        }

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

        [HttpGet]
        [Route("admin/open-romm-specialism-clear", Name = RouteConstants.AdminOpenSpecialismRommClear)]
        public async Task<IActionResult> AdminOpenSpecialismRommClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminOpenSpecialismRommViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminOpenSpecialismRomm, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/open-romm-specialism", Name = RouteConstants.AdminOpenSpecialismRomm)]
        public async Task<IActionResult> AdminOpenSpecialismRommAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminOpenSpecialismRommViewModel viewModel = await _loader.GetAdminOpenSpecialismRommAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminOpenSpecialismRommAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/open-romm-specialism", Name = RouteConstants.SubmitAdminOpenSpecialismRomm)]
        public async Task<IActionResult> AdminOpenSpecialismRommAsync(AdminOpenSpecialismRommViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool noSelected = model.DoYouWantToOpenRomm.HasValue && model.DoYouWantToOpenRomm.Value == false;
            if (noSelected)
            {
                return RedirectToRoute(nameof(RouteConstants.AdminLearnerRecord), new { pathwayId = model.RegistrationPathwayId });
            }

            await _cacheService.SetAsync(CacheKey, model);
            return RedirectToRoute(RouteConstants.AdminOpenSpecialismRommReviewChanges);
        }

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
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeCoreClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.PathwayAssessmentId })  :
             RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId }); //Todo re-direct to follow-up page.
        }

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
            return !hasGradeChanged ? RedirectToRoute(RouteConstants.AdminAddRommOutcomeChangeGradeSpecialismClear, new { registrationPathwayId = model.RegistrationPathwayId, assessmentId = model.SpecialismAssessmentId }):
                RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId }); //Todo re-direct to follow-up page.
        }

        #region change romm
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
            return RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId });
        }

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
            return RedirectToRoute(RouteConstants.AdminLearnerRecord, new { pathwayId = model.RegistrationPathwayId });
        }



        #endregion

    }
}