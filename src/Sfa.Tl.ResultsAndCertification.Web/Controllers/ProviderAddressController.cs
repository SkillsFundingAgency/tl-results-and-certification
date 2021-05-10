using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class ProviderAddressController : Controller
    {
        private readonly IProviderAddressLoader _providerAddressLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.ProviderAddressCacheKey); } }

        public ProviderAddressController(IProviderAddressLoader providerAddressLoader, ICacheService cacheService, ILogger<ProviderAddressController> logger)
        {
            _providerAddressLoader = providerAddressLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("manage-postal-address", Name = RouteConstants.ManagePostalAddress)]
        public async Task<IActionResult> ManagePostalAddressAsync()
        {
            var isAlreadyAdded = await FindPostalAddress();
            if (isAlreadyAdded)
                return View(); //TODO: redirect to ShowAddressPage.

            await _cacheService.RemoveAsync<AddProviderAddressViewModel>(CacheKey);
            return View(new ManagePostalAddressViewModel());

            static async Task<bool> FindPostalAddress()
            {
                await Task.CompletedTask;
                return false;
            }
        }

        [HttpGet]
        [Route("add-postal-address-postcode/{showPostcode:bool?}", Name = RouteConstants.AddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync(bool showPostcode = true)
        {
            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            var viewModel = new AddAddressPostcodeViewModel { Postcode = showPostcode && cacheModel?.AddAddressPostcode != null ? cacheModel.AddAddressPostcode.Postcode : null };
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-postcode", Name = RouteConstants.SubmitAddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync(AddAddressPostcodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = new AddProviderAddressViewModel { AddAddressPostcode = model };
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddAddressSelect);
        }

        [HttpGet]
        [Route("add-address-manually/{isFromSelectAddress:bool?}", Name = RouteConstants.AddAddressManually)]
        public async Task<IActionResult> AddAddressManuallyAsync(bool isFromSelectAddress)
        {
            // Fresh start -> Clear all manual data.
            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            if (cacheModel != null)
            {
                cacheModel.Manual = null;
                await _cacheService.SetAsync(CacheKey, cacheModel);
            }
            else
            {
                cacheModel = new AddProviderAddressViewModel();
                await _cacheService.SetAsync(CacheKey, cacheModel);
            }

            if (isFromSelectAddress)
                return RedirectToRoute(RouteConstants.AddPostalAddressManual, new { isFromSelectAddress });
            else
                return RedirectToRoute(RouteConstants.AddPostalAddressManual);
        }

        [HttpGet]
        [Route("add-postal-address-manual/{isFromSelectAddress:bool?}", Name = RouteConstants.AddPostalAddressManual)]
        public async Task<IActionResult> AddPostalAddressManualAsync(bool isFromSelectAddress)
        {
            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel.Manual ?? new AddPostalAddressManualViewModel();
            viewModel.IsFromSelectAddress = isFromSelectAddress;
            cacheModel.Manual = viewModel;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-manual", Name = RouteConstants.SubmitAddPostalAddressManual)]
        public async Task<IActionResult> AddPostalAddressManualAsync(AddPostalAddressManualViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.Manual = model;
            cacheModel.IsManual = true;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddAddressCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-postal-address-check-and-submit", Name = RouteConstants.AddAddressCheckAndSubmit)]
        public async Task<IActionResult> AddAddressCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddProviderAddressViewModel from redis cache in address check and submit page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var model = new AddAddressCheckAndSubmitViewModel { ProviderAddress = cacheModel };
            if (!model.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(model);
        }

        [HttpPost]
        [Route("add-postal-address-check-and-submit", Name = RouteConstants.SubmitAddAddressCheckAndSubmit)]
        public async Task<IActionResult> SubmitAddAddressCheckAndSubmitAsync()
        {
            await Task.CompletedTask;
            return RedirectToRoute("DevInprogress");
        }

        [HttpGet]
        [Route("add-postal-address-select", Name = RouteConstants.AddAddressSelect)]
        public async Task<IActionResult> AddAddressSelectAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);
            if (cacheModel?.AddAddressPostcode == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddProviderAddressViewModel from redis cache in add address select page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = await _providerAddressLoader.GetAddressesByPostcodeAsync(cacheModel.AddAddressPostcode.Postcode);
            viewModel.Postcode = cacheModel.AddAddressPostcode.Postcode;
            
            cacheModel.AddAddressSelect = viewModel;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-select", Name = RouteConstants.SubmitAddAddressSelect)]
        public async Task<IActionResult> AddAddressSelectAsync(AddAddressSelectViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddProviderAddressViewModel>(CacheKey);

            if (cacheModel?.AddAddressPostcode == null || cacheModel?.AddAddressSelect == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.AddAddressSelect.SelectedAddressUdprn = model.SelectedAddressUdprn;
            cacheModel.AddAddressSelect.DepartmentName = model.DepartmentName;
            cacheModel.IsManual = false;

            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddAddressCheckAndSubmit);
        }

        [HttpGet]
        [Route("Dev-inprogress", Name = "DevInprogress")]
        public async Task<IActionResult> DevInprogress()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
