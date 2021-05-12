using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

            await _cacheService.RemoveAsync<AddAddressViewModel>(CacheKey);
            return View(new ManagePostalAddressViewModel());

            static async Task<bool> FindPostalAddress()
            {
                await Task.CompletedTask;
                return false;
            }
        }

        [HttpGet]
        [Route("add-address", Name = RouteConstants.AddAddress)]
        public async Task<IActionResult> AddAddressAsync()
        {
            await _cacheService.RemoveAsync<AddAddressViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AddAddressPostcode);
        }

        [HttpGet]
        [Route("add-postal-address-postcode/{showPostcode:bool?}", Name = RouteConstants.AddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync(bool showPostcode = true)
        {
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
            var viewModel = new AddAddressPostcodeViewModel { Postcode = showPostcode && cacheModel?.AddAddressPostcode != null ? cacheModel.AddAddressPostcode.Postcode : null };
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-postcode", Name = RouteConstants.SubmitAddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync(AddAddressPostcodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = new AddAddressViewModel { AddAddressPostcode = model };
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddAddressSelect);
        }

        [HttpGet]
        [Route("add-address-manually/{isFromSelectAddress:bool?}", Name = RouteConstants.AddAddressManually)]
        public async Task<IActionResult> AddAddressManuallyAsync(bool isFromSelectAddress)
        {
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
            
            if (cacheModel != null)
                cacheModel.AddAddressManual = null;
            else
                cacheModel = new AddAddressViewModel();
            
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddPostalAddressManual, isFromSelectAddress ? new { isFromSelectAddress } : null);
        }

        [HttpGet]
        [Route("add-postal-address-manual/{isFromSelectAddress:bool?}", Name = RouteConstants.AddPostalAddressManual)]
        public async Task<IActionResult> AddPostalAddressManualAsync(bool isFromSelectAddress)
        {
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel.AddAddressManual ?? new AddAddressManualViewModel();
            viewModel.IsFromSelectAddress = isFromSelectAddress;

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-manual", Name = RouteConstants.SubmitAddPostalAddressManual)]
        public async Task<IActionResult> AddPostalAddressManualAsync(AddAddressManualViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.AddAddressSelect = null; 
            cacheModel.AddAddressManual = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddAddressCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-postal-address-select", Name = RouteConstants.AddAddressSelect)]
        public async Task<IActionResult> AddAddressSelectAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
            if (cacheModel?.AddAddressPostcode == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddProviderAddressViewModel from redis cache in add address select page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var addresses = await _providerAddressLoader.GetAddressesByPostcodeAsync(cacheModel.AddAddressPostcode.Postcode);
            AddAddressSelectViewModel viewModel;

            if (cacheModel.AddAddressSelect != null)
            {
                viewModel = cacheModel.AddAddressSelect;
                viewModel.AddressSelectList = addresses.AddressSelectList;
            }
            else
            {
                addresses.Postcode = cacheModel.AddAddressPostcode.Postcode;
                viewModel = addresses;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-postal-address-select", Name = RouteConstants.SubmitAddAddressSelect)]
        public async Task<IActionResult> AddAddressSelectAsync(AddAddressSelectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var addressViewModel = await _providerAddressLoader.GetAddressesByPostcodeAsync(model.Postcode);
                model.AddressSelectList = addressViewModel.AddressSelectList;
                return View(model);
            }

            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);

            if (cacheModel?.AddAddressPostcode == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var selectedAddress = await _providerAddressLoader.GetAddressByUprnAsync(model.SelectedAddressUprn.Value);

            if (selectedAddress == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            model.SelectedAddress = selectedAddress;
            cacheModel.AddAddressSelect = model;
            cacheModel.AddAddressManual = null;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddAddressCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-postal-address-check-and-submit", Name = RouteConstants.AddAddressCheckAndSubmit)]
        public async Task<IActionResult> AddAddressCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);
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
            var cacheModel = await _cacheService.GetAsync<AddAddressViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var isSuccess = await _providerAddressLoader.AddAddressAsync(User.GetUkPrn(), cacheModel);

            if (isSuccess)
            {
                await _cacheService.RemoveAsync<AddAddressViewModel>(CacheKey);
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AddAddressConfirmation), true, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AddAddressConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.AddAddressFailed, $"Unable to add address for provider ukprn: {User.GetUkPrn()}. Method: SubmitAddAddressCheckAndSubmitAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("add-postal-address-confirmation", Name = RouteConstants.AddAddressConfirmation)]
        public async Task<IActionResult> AddAddressConfirmationAsync()
        {
            var isAddressAdded = await _cacheService.GetAndRemoveAsync<bool>(string.Concat(CacheKey, Constants.AddAddressConfirmation));

            if (!isAddressAdded)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read value from redis cache in add addressconfirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View();
        }
    }
}