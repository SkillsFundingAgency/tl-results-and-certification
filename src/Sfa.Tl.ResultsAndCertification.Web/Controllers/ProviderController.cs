using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        [HttpGet]
        [Route("your-providers", Name = RouteConstants.YourProviders)]
        public async Task<IActionResult> ViewAllAsync()
        {
            return await Task.Run(() => View());
        }

        [HttpGet]
        [Route("providers", Name = RouteConstants.Providers)]
        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync()
        {
            var yourProvidersExists  = await _providerLoader.IsAnyProviderSetupCompletedAsync(User.GetUkPrn());
            var viewModel = new FindProviderViewModel { ShowViewProvidersLink = yourProvidersExists };
            return View(viewModel);
        }

        [HttpPost]
        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync(FindProviderViewModel viewModel)
        {
            if (!await FindProviderViewModelValidated(viewModel))
            {
                return View(viewModel);
            }

            /* TODO 
             * Task - check if all tlevels are set, then redirect to providerTlevels
             */
            var isAllTlevelsSetupDone = false;
            if (isAllTlevelsSetupDone)
            {
                return RedirectToRoute(RouteConstants.ProviderTlevels, new { providerId = viewModel.SelectedProviderId });
            }

            return RedirectToRoute(RouteConstants.SelectProviderTlevels, new { providerId = viewModel.SelectedProviderId });
        }

        [HttpGet]
        [Route("search-provider/{name}", Name = RouteConstants.ProviderNameLookup)]
        public async Task<JsonResult> GetProviderLookupDataAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Json(string.Empty);

            var providersData = await _providerLoader.GetProviderLookupDataAsync(name, isExactMatch: false);
            return Json(providersData);
        }

        [HttpGet]
        [Route("select-providers-tlevels/{providerId}", Name = RouteConstants.SelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(int providerId)
        {
            return await GetSelectProviderTlevelsAsync(providerId);
        }

        [HttpPost]
        [Route("select-providers-tlevels", Name = RouteConstants.SubmitSelectProviderTlevels)]
        public async Task<IActionResult> SelectProviderTlevelsAsync(ProviderTlevelsViewModel viewModel)
        {
            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!ModelState.IsValid)
            {
                return await GetSelectProviderTlevelsAsync(viewModel.ProviderId);
            }

            var isSuccess = await _providerLoader.AddProviderTlevelsAsync(viewModel);
            if (isSuccess)
            {
                viewModel.Tlevels = viewModel.Tlevels.Where(x => x.IsSelected).ToList();
                TempData[Constants.ProviderTlevelsViewModel] = JsonConvert.SerializeObject(viewModel);
                return RedirectToRoute(RouteConstants.ProviderTlevelConfirmation);
            }
            else
            {
                return RedirectToRoute("error/500");
            }
        }

        [HttpGet]
        [Route("submit-successful", Name = RouteConstants.ProviderTlevelConfirmation)]
        public IActionResult ConfirmationAsync()
        {
            if (TempData[Constants.ProviderTlevelsViewModel] == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = JsonConvert.DeserializeObject<ProviderTlevelsViewModel>(TempData[Constants.ProviderTlevelsViewModel] as string);
            return View(viewModel);
        }

        [HttpGet]
        [Route("provider-tlevels/{providerId}", Name = RouteConstants.ProviderTlevels)]
        public async Task<IActionResult> ViewProviderTlevelsAsync(int providerId)
        {
            var viewModel = await _providerLoader.GetViewProviderTlevelViewModelAsync(User.GetUkPrn(), providerId);

            /* TODO:
             * Task: viewModel should know the if no more tlevelsetup can be done --> show or hide the button 'Add more Tlevels'
             * Task: viewModel should track the previous page so that correct button at the bottom will be shown. 
             */

            // Task -> Bookmark or no Tlevels then redirect  (check with Gurmukh this redirection seesmsto be wrong)
            var tlevelsExists = true;
            if (!tlevelsExists)
            {
                return RedirectToRoute(RouteConstants.YourProviders);
            }
            
            //return View(viewModel);

            return await GetSelectProviderTlevelsAsync(providerId);
        }

        private async Task<IActionResult> GetSelectProviderTlevelsAsync(int providerId)
        {
            var viewModel = await _providerLoader.GetSelectProviderTlevelsAsync(User.GetUkPrn(), providerId);

            if (viewModel == null || viewModel.Tlevels == null || viewModel.Tlevels.Count == 0)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
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
                    ModelState.AddModelError("Search", Web.Content.Provider.FindProvider.ProviderName_NotValid_Validation_Message);
                    return false;
                }

                viewModel.SelectedProviderId = providerData.First().Id;
            }

            return true;
        }
    }
}