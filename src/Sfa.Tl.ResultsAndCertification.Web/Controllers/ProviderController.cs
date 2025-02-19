﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireProviderEditorAccess)]
    public class ProviderController : Controller
    {
        private readonly IProviderLoader _providerLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.ProviderCacheKey);

        public ProviderController(IProviderLoader providerLoader, ICacheService cacheService, ILogger<ProviderController> logger)
        {
            _providerLoader = providerLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("your-providers", Name = RouteConstants.YourProviders)]
        public async Task<IActionResult> YourProvidersAsync()
        {
            var viewModel = await _providerLoader.GetYourProvidersAsync(User.GetUkPrn());

            if (viewModel == null || viewModel.Providers == null || viewModel.Providers.Count == 0)
            {
                _logger.LogInformation(LogEvent.ProviderNotFound, $"No provideproviders found. Method: GetTqAoProviderDetailsAsync(Ukprn: {User.GetUkPrn()}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.FindProvider);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("find-provider/{isback:bool?}", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync(bool isback = false)
        {
            var yourProvidersExists = await _providerLoader.IsAnyProviderSetupCompletedAsync(User.GetUkPrn());
            var viewModel = new FindProviderViewModel { ShowViewProvidersLink = yourProvidersExists };

            viewModel.Search = isback ? TempData.Get<string>(Constants.FindProviderSearchCriteria) : null;
            return View(viewModel);
        }

        [HttpPost]
        [Route("find-provider", Name = RouteConstants.SubmitFindProvider)]
        public async Task<IActionResult> FindProviderAsync(FindProviderViewModel viewModel)
        {
            if (!await FindProviderViewModelValidated(viewModel))
            {
                return View(viewModel);
            }

            var hasAnyTlevelSetup = await _providerLoader.HasAnyTlevelSetupForProviderAsync(User.GetUkPrn(), viewModel.SelectedProviderId);
            return RedirectToRoute(hasAnyTlevelSetup ? RouteConstants.ProviderTlevels : RouteConstants.SelectProviderTlevels, new { providerId = viewModel.SelectedProviderId });
        }

        [HttpGet]
        [Route("select-providers-tlevels/{providerId}", Name = RouteConstants.SelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(int providerId)
        {
            return await GetSelectProviderTlevelsAsync(providerId, isAddTlevel: false);
        }

        [HttpGet]
        [Route("add-additional-tlevels/{providerId}", Name = RouteConstants.AddProviderTlevels)]
        public async Task<IActionResult> AddProviderTlevelsAsync(int providerId)
        {
            return await GetSelectProviderTlevelsAsync(providerId, isAddTlevel: true);
        }

        [HttpPost]
        [Route("add-additional-tlevels", Name = RouteConstants.SubmitAddProviderTlevels)]
        [Route("select-providers-tlevels", Name = RouteConstants.SubmitSelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(ProviderTlevelsViewModel viewModel)
        {
            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!ModelState.IsValid)
            {
                return await GetSelectProviderTlevelsAsync(viewModel.ProviderId, viewModel.IsAddTlevel);
            }

            var isSuccess = await _providerLoader.AddProviderTlevelsAsync(viewModel);
            if (isSuccess)
            {
                viewModel.Tlevels = viewModel.Tlevels.Where(x => x.IsSelected).ToList();
                await _cacheService.SetAsync(CacheKey, viewModel);
                return RedirectToRoute(RouteConstants.ProviderTlevelConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.ProviderTlevelNotAdded,
                    $"Unable to add provider T level. Method: AddProviderTlevelsAsync, Ukprn: {User.GetUkPrn()}, Provider: {viewModel.ProviderId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("submit-successful", Name = RouteConstants.ProviderTlevelConfirmation)]
        public async Task<IActionResult> ConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAsync<ProviderTlevelsViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed,
                    $"Unable to read provider T level add confirmation page temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("remove-tlevel/{id}/{navigation:bool?}", Name = RouteConstants.RemoveProviderTlevel)]
        public async Task<IActionResult> RemoveProviderTlevelAsync(int id, bool navigation)
        {
            var viewModel = await _providerLoader.GetTqProviderTlevelDetailsAsync(User.GetUkPrn(), id);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ProviderTlevelNotFound, $"No provider T level found. Method: GetTqProviderTlevelDetailsAsync({User.GetUkPrn()}, {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.ShowBackToProvidersLink = navigation;
            return View(viewModel);
        }

        [HttpPost]
        [Route("remove-tlevel", Name = RouteConstants.SubmitRemoveProviderTlevel)]
        public async Task<IActionResult> RemoveProviderTlevelAsync(ProviderTlevelDetailsViewModel viewModel)
        {
            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!ModelState.IsValid)
                return View(viewModel);

            if (viewModel.CanRemoveTlevel == false)
                return RedirectToRoute(RouteConstants.ProviderTlevels, new { providerId = viewModel.TlProviderId, navigation = !viewModel.ShowBackToProvidersLink });

            var isSuccess = await _providerLoader.RemoveTqProviderTlevelAsync(User.GetUkPrn(), viewModel.Id);

            if (isSuccess)
            {
                if (viewModel.ShowBackToProvidersLink)
                {
                    var providersViewModel = await _providerLoader.GetTqAoProviderDetailsAsync(User.GetUkPrn());
                    viewModel.ShowBackToProvidersLink = providersViewModel != null && providersViewModel.Count > 0;
                }

                await _cacheService.SetAsync(CacheKey, viewModel);
                return RedirectToRoute(RouteConstants.RemoveProviderTlevelConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.ProviderTlevelNotRemoved,
                    $"Unable to remove provider T level. Method: RemoveTqProviderTlevelAsync(Ukprn: {User.GetUkPrn()}, id: {viewModel.Id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("tlevel-removed-confirmation", Name = RouteConstants.RemoveProviderTlevelConfirmation)]
        public async Task<IActionResult> RemoveConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAsync<ProviderTlevelDetailsViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed,
                    $"Unable to read remove provider T level confirmation page temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("provider-tlevels/{providerId}/{navigation:bool?}", Name = RouteConstants.ProviderTlevels)]
        public async Task<IActionResult> ViewProviderTlevelsAsync(int providerId, bool navigation)
        {
            var viewModel = await _providerLoader.GetViewProviderTlevelViewModelAsync(User.GetUkPrn(), providerId);

            if (viewModel == null || viewModel.Tlevels == null)
            {
                _logger.LogWarning(LogEvent.ProviderNotFound,
                    $"No provider found. Method: GetViewProviderTlevelViewModelAsync(Ukprn: {User.GetUkPrn()}, ProviderId: {providerId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!viewModel.AnyTlevelsAvailable)
            {
                _logger.LogInformation(LogEvent.ProviderTlevelNotFound,
                    $"No provider T levels found. Method: GetViewProviderTlevelViewModelAsync(Ukprn: {User.GetUkPrn()}, ProviderId: {providerId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.SelectProviderTlevels, new { providerId });
            }

            viewModel.IsNavigatedFromFindProvider = navigation;
            viewModel.IsNavigatedFromYourProvider = !navigation;
            return View(viewModel);
        }

        private async Task<IActionResult> GetSelectProviderTlevelsAsync(int providerId, bool isAddTlevel)
        {
            var viewModel = await _providerLoader.GetSelectProviderTlevelsAsync(User.GetUkPrn(), providerId);

            if (viewModel == null || viewModel.Tlevels == null)
            {
                _logger.LogWarning(LogEvent.ProviderNotFound,
                    $"No provider found. Method: GetSelectProviderTlevelsAsync(Ukprn: {User.GetUkPrn()}, ProviderId: {providerId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            else if (viewModel.Tlevels.Count == 0)
            {
                _logger.LogInformation(LogEvent.ProviderTlevelNotFound,
                    $"No provider T levels found. Method: GetSelectProviderTlevelsAsync(Ukprn: {User.GetUkPrn()}, ProviderId: {providerId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ProviderTlevels, new { providerId, navigation = true });
            }

            viewModel.IsAddTlevel = isAddTlevel;

            if (!viewModel.IsAddTlevel)
            {
                TempData.Set(Constants.FindProviderSearchCriteria, viewModel.DisplayName);
            }
            return View("SelectProviderTlevels", viewModel);
        }

        private async Task<bool> FindProviderViewModelValidated(FindProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return false;

            if (viewModel.SelectedProviderId == 0)
            {
                var providerData = await _providerLoader.GetProviderLookupDataAsync(viewModel.Search, isExactMatch: true);
                if (providerData == null || providerData.Count() != 1)
                {
                    _logger.LogInformation(LogEvent.ProviderNotFound,
                    $"No provider found. Method: GetProviderLookupDataAsync(Search: {viewModel.Search}, isExactMatch: {true}), User: {User.GetUserEmail()}");
                    ModelState.AddModelError("Search", Web.Content.Provider.FindProvider.ProviderName_NotValid_Validation_Message);
                    return false;
                }

                viewModel.SelectedProviderId = providerData.First().Id;
            }

            return true;
        }
    }
}