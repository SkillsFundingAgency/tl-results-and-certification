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
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner.Validator;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminBannerController : Controller
    {
        private readonly IAdminBannerLoader _adminBannerLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminBannerController> _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminBannerCacheKey);

        private string NotificationCacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminBannerInformationCacheKey);

        public AdminBannerController(
            IAdminBannerLoader adminBannerLoader,
            ICacheService cacheService,
            ILogger<AdminBannerController> logger)
        {
            _adminBannerLoader = adminBannerLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/find-banner/{pageNumber:int?}", Name = RouteConstants.AdminFindBanner)]
        public async Task<IActionResult> AdminFindBannerAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminFindBannerViewModel>(CacheKey);
            if (viewModel == null)
            {
                AdminFindBannerViewModel loadedViewModel = await _adminBannerLoader.SearchBannersAsync();

                await _cacheService.SetAsync(CacheKey, loadedViewModel);
                return View(loadedViewModel);
            }

            AdminFindBannerCriteriaViewModel searchCriteria = viewModel.SearchCriteriaViewModel ?? new AdminFindBannerCriteriaViewModel();
            searchCriteria.PageNumber = pageNumber;

            viewModel = await _adminBannerLoader.SearchBannersAsync(searchCriteria);
            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/find-banner-search-key", Name = RouteConstants.SubmitAdminFindBanner)]
        public async Task<IActionResult> AdminFindBannerSearchKeyAsync(AdminFindBannerCriteriaViewModel searchCriteriaViewModel)
        {
            var viewModel = await _cacheService.GetAsync<AdminFindBannerViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminFindBannerViewModel cache data found. Method: {RouteConstants.SubmitAdminFindBanner}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteriaViewModel = searchCriteriaViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminFindBanner, new { pageNumber = viewModel.SearchCriteriaViewModel.PageNumber });
        }

        [HttpGet]
        [Route("admin/banner-details/{bannerId}", Name = RouteConstants.AdminBannerDetails)]
        public async Task<IActionResult> AdminBannerDetailsAsync(int bannerId)
        {
            AdminBannerDetailsViewModel viewModel = await _adminBannerLoader.GetBannerDetailsViewModel(bannerId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/edit-banner", Name = RouteConstants.AdminEditBanner)]
        public async Task<IActionResult> AdminEditBannerAsync(int bannerId)
        {
            AdminEditBannerViewModel viewModel = await _adminBannerLoader.GetEditBannerViewModel(bannerId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/edit-banner", Name = RouteConstants.SubmitAdminEditBanner)]
        public async Task<IActionResult> AdminEditBannerAsync(AdminEditBannerViewModel viewModel)
        {
            ValidationResult validationReult = ValidateBannerBaseViewModel(viewModel);
            validationReult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool success = await _adminBannerLoader.SubmitUpdateBannerRequest(viewModel);

            if (!success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminEditBanner.Notification_Message_Banner_Updated);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminBannerDetails, new { bannerId = viewModel.BannerId });
        }

        [HttpGet]
        [Route("admin/add-banner", Name = RouteConstants.AdminAddBanner)]
        public IActionResult AdminAddBannerAsync()
        {
            return View(new AdminAddBannerViewModel());
        }

        [HttpPost]
        [Route("admin/add-banner", Name = RouteConstants.SubmitAdminAddBanner)]
        public async Task<IActionResult> AdminAddBannerAsync(AdminAddBannerViewModel viewModel)
        {
            ValidationResult validationReult = ValidateBannerBaseViewModel(viewModel);
            validationReult.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            AddBannerResponse response = await _adminBannerLoader.SubmitAddBannerRequest(viewModel);

            if (!response.Success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminAddBanner.Notification_Message_Banner_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminBannerDetails, new { bannerId = response.BannerId });
        }

        private static ValidationResult ValidateBannerBaseViewModel(AdminBannerBaseViewModel viewModel)
        {
            AdminBannerBaseViewModelValidator validator = new();
            return validator.Validate(viewModel);
        }
    }
}