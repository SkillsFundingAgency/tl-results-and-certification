using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireProviderEditorAccess)]
    public class ProviderController : Controller
    {
        private readonly IProviderLoader _providerLoader;
        private readonly ILogger _logger;

        public ProviderController(IProviderLoader providerLoader, ILogger<ProviderController> logger)
        {
            _providerLoader = providerLoader;
            _logger = logger;
        }

        [Route("providers", Name = RouteConstants.Providers)]
        public async Task<IActionResult> IndexAsync()
        {
            var IsAnyProviderSetupCompleted = await _providerLoader.IsAnyProviderSetupCompletedAsync(User.GetUkPrn());

            if (IsAnyProviderSetupCompleted)
                return RedirectToRoute(RouteConstants.YourProviders); // TODO: redirect to AddProvider.

            return RedirectToRoute(RouteConstants.FindProvider);
        }

        [Route("your-providers", Name = RouteConstants.YourProviders)]
        public async Task<IActionResult> ViewAllAsync()
        {
            return await Task.Run(() => View());
        }

        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public IActionResult FindProviderAsync()
        {
            var viewModel = new FindProviderViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync(FindProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // TODO: Below is for the next story.
            var result = await Task.Run(() => true);
            return RedirectToRoute(RouteConstants.YourProviders);
        }

        [Route("search-provider/{name}", Name = RouteConstants.ProviderNameLookup)]
        public async Task<JsonResult> FindProviderNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Json(string.Empty);

            var providerNames = await _providerLoader.FindProviderNameAsync(name);

            // Temp code to test ajax call. 
            var providers = new List<ProviderLookupViewModel>();
            var i = 0;
            foreach (var p in providerNames)
                providers.Add(new ProviderLookupViewModel { Id = ++i, DisplayName = p }); ;

            return Json(providers);
        }

        [Route("select-providers-tlevels/{providerId}", Name = RouteConstants.SelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(int providerId)
        {
            return await GetSelectProviderTlevelsAsync(providerId);
        }

        [HttpPost]
        [Route("select-providers-tlevels", Name = RouteConstants.SubmitSelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(ProviderTlevelsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return await GetSelectProviderTlevelsAsync(viewModel.ProviderId);
            }
            return RedirectToRoute(RouteConstants.PageNotFound);
        }

        private async Task<IActionResult> GetSelectProviderTlevelsAsync(int providerId)
        {
            var viewModel = await _providerLoader.GetSelectProviderTlevelsAsync(User.GetUkPrn(), providerId);

            if (viewModel == null || viewModel.Tlevels == null || viewModel.Tlevels.Count == 0)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }
    }
}