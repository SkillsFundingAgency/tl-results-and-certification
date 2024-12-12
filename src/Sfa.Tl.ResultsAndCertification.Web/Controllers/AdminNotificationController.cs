using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification.Validator;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminNotificationController : Controller
    {
        private readonly IAdminNotificationLoader _loader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminNotificationController> _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminNotificationCacheKey);

        private string NotificationCacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminNotificationInformationCacheKey);

        public AdminNotificationController(
            IAdminNotificationLoader loader,
            ICacheService cacheService,
            ILogger<AdminNotificationController> logger)
        {
            _loader = loader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/find-notification-clear", Name = RouteConstants.AdminFindNotificationClear)]
        public async Task<IActionResult> AdminFindNotificationClearAsync()
        {
            await _cacheService.RemoveAsync<AdminFindNotificationViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminFindNotification);
        }

        [HttpGet]
        [Route("admin/find-notification/{pageNumber:int?}", Name = RouteConstants.AdminFindNotification)]
        public async Task<IActionResult> AdminFindNotificationAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminFindNotificationViewModel>(CacheKey);
            if (viewModel == null)
            {
                AdminFindNotificationCriteriaViewModel criteria = _loader.LoadFilters();
                viewModel = await _loader.SearchNotificationAsync(criteria);

                await _cacheService.SetAsync(CacheKey, viewModel);
                return View(viewModel);
            }

            AdminFindNotificationCriteriaViewModel searchCriteria = viewModel.SearchCriteria;
            searchCriteria.PageNumber = pageNumber;

            viewModel = await _loader.SearchNotificationAsync(searchCriteria);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/find-notification-apply-filters", Name = RouteConstants.SubmitAdminFindNotificationApplyFilters)]
        public async Task<IActionResult> AdminFindNotificationApplyFiltersAsync(AdminFindNotificationCriteriaViewModel searchCriteriaViewModel)
        {
            var viewModel = await _cacheService.GetAsync<AdminFindNotificationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminFindNotificationViewModel cache data found. Method: {RouteConstants.SubmitAdminFindNotificationApplyFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = searchCriteriaViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminFindNotification, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpPost]
        [Route("admin/find-notification-clear-filters", Name = RouteConstants.SubmitAdminFindNotificationClearFilters)]
        public async Task<IActionResult> AdminFindNotificationClearFiltersAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminFindNotificationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminFindNotificationViewModel cache data found. Method: {RouteConstants.SubmitAdminFindNotificationClearFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = _loader.LoadFilters();

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminFindNotification, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpGet]
        [Route("admin/notification-details/{notificationId}", Name = RouteConstants.AdminNotificationDetails)]
        public async Task<IActionResult> AdminNotificationDetailsAsync(int notificationId)
        {
            AdminNotificationDetailsViewModel viewModel = await _loader.GetNotificationDetailsViewModel(notificationId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/edit-notification", Name = RouteConstants.AdminEditNotification)]
        public async Task<IActionResult> AdminEditNotificationAsync(int notificationId)
        {
            AdminEditNotificationViewModel viewModel = await _loader.GetEditNotificationViewModel(notificationId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/edit-notification", Name = RouteConstants.SubmitAdminEditNotification)]
        public async Task<IActionResult> AdminEditNotificationAsync(AdminEditNotificationViewModel viewModel)
        {
            ValidationResult validationReult = ValidateNotificationBaseViewModel(viewModel);
            validationReult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool success = await _loader.SubmitUpdateNotificationRequest(viewModel);

            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminEditNotification.Message_Notification_Updated);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminNotificationDetails, new { notificationId = viewModel.NotificationId });
        }

        [HttpGet]
        [Route("admin/add-notification", Name = RouteConstants.AdminAddNotification)]
        public IActionResult AdminAddNotificationAsync()
        {
            return View(new AdminAddNotificationViewModel());
        }

        [HttpPost]
        [Route("admin/add-notification", Name = RouteConstants.SubmitAdminAddNotification)]
        public async Task<IActionResult> AdminAddNotificationAsync(AdminAddNotificationViewModel viewModel)
        {
            ValidationResult validationReult = ValidateNotificationBaseViewModel(viewModel);
            validationReult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            AddNotificationResponse response = await _loader.SubmitAddNotificationRequest(viewModel);

            if (!response.Success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminAddNotification.Message_Notification_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminNotificationDetails, new { notificationId = response.NotificationId });
        }

        private static ValidationResult ValidateNotificationBaseViewModel(AdminNotificationBaseViewModel viewModel)
        {
            AdminNotificationBaseViewModelValidator validator = new();
            return validator.Validate(viewModel);
        }
    }
}