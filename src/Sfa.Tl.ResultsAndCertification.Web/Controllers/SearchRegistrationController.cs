using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class SearchRegistrationController : Controller
    {
        private readonly ISearchRegistrationLoader _loader;
        private readonly IProviderLoader _providerLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<SearchRegistrationController> _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.SearchRegistrationCacheKey);

        public SearchRegistrationController(ISearchRegistrationLoader loader, IProviderLoader providerLoader, ICacheService cacheService, ILogger<SearchRegistrationController> logger)
        {
            _loader = loader;
            _providerLoader = providerLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("search-learner-clear", Name = RouteConstants.SearchRegistrationClear)]
        public async Task<IActionResult> SearchRegistrationClearAsync(SearchRegistrationType type)
        {
            await _cacheService.RemoveAsync<SearchRegistrationViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.SearchRegistration, new { type });
        }

        [HttpGet]
        [Route("search-learner", Name = RouteConstants.SearchRegistration)]
        public async Task<IActionResult> SearchRegistrationAsync(SearchRegistrationType type, int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<SearchRegistrationViewModel>(CacheKey);
            if (viewModel == null)
            {
                viewModel = await _loader.CreateSearchRegistration(type);

                await _cacheService.SetAsync(CacheKey, viewModel);
                return View(viewModel);
            }

            var searchCriteria = viewModel.Criteria;

            if (!searchCriteria.IsSearchKeyApplied && !searchCriteria.AreFiltersApplied)
            {
                viewModel.ClearRegistrationDetailsList();
                return View(viewModel);
            }

            searchCriteria.PageNumber = pageNumber;

            SearchRegistrationDetailsListViewModel registrationDetailsListViewModel = await _loader.GetSearchRegistrationDetailsListAsync(User.GetUkPrn(), type, searchCriteria);
            viewModel.SetRegistrationDetailsList(registrationDetailsListViewModel);

            viewModel.DetailsList = registrationDetailsListViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("search-learner-search-key", Name = RouteConstants.SubmitSearchRegistrationSearchKey)]
        public Task<IActionResult> SearchRegistrationSearchKeyAsync(SearchRegistrationCriteriaViewModel searchCriteriaViewModel)
            => RunAsync(RouteConstants.SubmitSearchRegistrationSearchKey, p => p.SetSearchKey(searchCriteriaViewModel.SearchKey));

        [HttpPost]
        [Route("search-learner-clear-key", Name = RouteConstants.SubmitSearchRegistrationClearKey)]
        public Task<IActionResult> SearchRegistrationClearKeyAsync()
            => RunAsync(RouteConstants.SubmitSearchRegistrationClearKey, p => p.ClearSearchKey());

        [HttpPost]
        [Route("search-learner-filters", Name = RouteConstants.SubmitSearchRegistrationFilters)]
        public async Task<IActionResult> SearchRegistrationFiltersAsync(SearchRegistrationFiltersViewModel filtersViewModel)
        {
            int? providerId = await GetFilterProviderId(filtersViewModel.Search);
            filtersViewModel.SelectedProviderId = providerId;

            return await RunAsync(RouteConstants.SubmitSearchRegistrationFilters, p => p.SetFilters(filtersViewModel));
        }

        [HttpPost]
        [Route("search-learner-clear-filters", Name = RouteConstants.SubmitSearchRegistrationClearFilters)]
        public Task<IActionResult> SearchRegistrationClearFiltersAsync()
            => RunAsync(RouteConstants.SubmitSearchRegistrationClearFilters, p => p.ClearFilters());

        private async Task<IActionResult> RunAsync(string endpoint, Action<SearchRegistrationViewModel> action)
        {
            var viewModel = await _cacheService.GetAsync<SearchRegistrationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No SearchRegistrationViewModel cache data found. Method: {endpoint}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            action(viewModel);

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchRegistration, new { type = viewModel.SearchType, pageNumber = viewModel.Criteria.PageNumber });
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
    }
}