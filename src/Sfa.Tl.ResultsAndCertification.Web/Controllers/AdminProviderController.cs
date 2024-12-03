using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminProviderController : Controller
    {
        private readonly IProviderLoader _providerLoader;
        private readonly IAdminProviderLoader _adminProviderLoader;
        private readonly ICacheService _cacheService;

        private string CacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);
        private string NotificationCacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardInformationCacheKey);

        public AdminProviderController(
            IProviderLoader providerLoader,
            IAdminProviderLoader adminProviderLoader,
            ICacheService cacheService)
        {
            _providerLoader = providerLoader;
            _adminProviderLoader = adminProviderLoader;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("admin/find-provider-clear", Name = RouteConstants.AdminFindProviderClear)]
        public async Task<IActionResult> AdminFindProviderClearAsync()
        {
            await _cacheService.RemoveAsync<AdminFindProviderViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminFindProvider);
        }

        [HttpGet]
        [Route("admin/find-provider", Name = RouteConstants.AdminFindProvider)]
        public async Task<IActionResult> AdminFindProviderAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminFindProviderViewModel>(CacheKey);
            return viewModel == null ? View(new AdminFindProviderViewModel()) : View(viewModel);
        }

        [HttpPost]
        [Route("admin/find-provider", Name = RouteConstants.AdminSubmitFindProvider)]
        public async Task<IActionResult> AdminFindProviderAsync(AdminFindProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            (bool found, int providerId) = await GetProviderId(viewModel.Search);
            if (!found)
            {
                ModelState.AddModelError("Search", AdminFindProvider.ProviderName_NotValid_Validation_Message);
                return View(viewModel);
            }

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminProviderDetails, new { providerId });
        }

        [HttpGet]
        [Route("admin/provider-details/{providerId}", Name = RouteConstants.AdminProviderDetails)]
        public async Task<IActionResult> AdminProviderDetailsAsync(int providerId)
        {
            AdminProviderDetailsViewModel viewModel = await _adminProviderLoader.GetProviderDetailsViewModel(providerId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(NotificationCacheKey);

            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/edit-provider/{providerId}", Name = RouteConstants.AdminEditProvider)]
        public async Task<IActionResult> AdminEditProviderAsync(int providerId)
        {
            AdminEditProviderViewModel viewModel = await _adminProviderLoader.GetEditProviderViewModel(providerId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/edit-provider", Name = RouteConstants.SubmitAdminEditProvider)]
        public async Task<IActionResult> AdminEditProviderAsync(AdminEditProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            UpdateProviderResponse updateResponse = await _adminProviderLoader.SubmitUpdateProviderRequest(viewModel);

            if (!updateResponse.IsRequestValid)
            {
                if (updateResponse.DuplicatedUkprnFound)
                    ModelState.AddModelError("Ukprn", AdminEditProvider.Validation_Message_Ukprn_Duplicated);

                if (updateResponse.DuplicatedNameFound)
                    ModelState.AddModelError("Name", AdminEditProvider.Validation_Message_Name_Duplicated);

                if (updateResponse.DuplicatedDisplayNameFound)
                    ModelState.AddModelError("DisplayName", AdminEditProvider.Validation_Message_DisplayName_Duplicated);

                return View(viewModel);
            }

            if (!updateResponse.Success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminEditProvider.Notification_Message_Provider_Updated);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminProviderDetails, new { providerId = viewModel.ProviderId });
        }

        private async Task<(bool found, int providerId)> GetProviderId(string providerName)
        {
            var notFoundResult = (false, 0);

            if (string.IsNullOrWhiteSpace(providerName))
            {
                return notFoundResult;
            }

            IEnumerable<ProviderLookupData> providerData = await _providerLoader.GetProviderLookupDataAsync(providerName, isExactMatch: true);
            if (!providerData.IsNullOrEmpty() && providerData.Count() == 1)
            {
                return (true, providerData.Single().Id);
            }

            return notFoundResult;
        }

        [HttpGet]
        [Route("admin/add-provider", Name = RouteConstants.AdminAddProvider)]
        public IActionResult AdminAddProviderAsync()
        {
            return View(new AdminAddProviderViewModel());
        }

        [HttpPost]
        [Route("admin/add-provider", Name = RouteConstants.SubmitAdminAddProvider)]
        public async Task<IActionResult> AdminAddProviderAsync(AdminAddProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            AddProviderResponse addResponse = await _adminProviderLoader.SubmitAddProviderRequest(viewModel);

            if (!addResponse.IsRequestValid)
            {
                if (addResponse.DuplicatedUkprnFound)
                    ModelState.AddModelError("Ukprn", AdminAddProvider.Validation_Message_Ukprn_Duplicated);

                if (addResponse.DuplicatedNameFound)
                    ModelState.AddModelError("Name", AdminAddProvider.Validation_Message_Name_Duplicated);

                if (addResponse.DuplicatedDisplayNameFound)
                    ModelState.AddModelError("DisplayName", AdminAddProvider.Validation_Message_DisplayName_Duplicated);

                return View(viewModel);
            }

            if (!addResponse.Success)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var notificationBanner = new AdminNotificationBannerModel(AdminAddProvider.Notification_Message_Provider_Added);
            await _cacheService.SetAsync<NotificationBannerModel>(NotificationCacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AdminProviderDetails, new { providerId = addResponse.ProviderId });
        }
    }
}