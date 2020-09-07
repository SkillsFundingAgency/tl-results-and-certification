using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class ManageRegistrationController : Controller
    {
        private readonly IRegistrationLoader _registrationLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        public ManageRegistrationController(
            IRegistrationLoader registrationLoader, 
            ICacheService cacheService, 
            ILogger<RegistrationController> logger)
        {
            _registrationLoader = registrationLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("change-learners-name/{profileId}", Name = RouteConstants.ChangeRegistrationLearnersName)]
        public async Task<IActionResult> ChangeLearnersNameAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeLearnersNameViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeLearnersNameAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-learners-name", Name = RouteConstants.SubmitChangeRegistrationLearnersName)]
        public async Task<IActionResult> ChangeLearnersNameAsync(ChangeLearnersNameViewModel vm)
        {
            await Task.Run(() => true);
            if (!ModelState.IsValid)
                return View(vm);
            else
                return RedirectToRoute(RouteConstants.ChangeRegistrationLearnersName, new { vm.ProfileId });
        }

        [HttpGet]
        [Route("change-provider/{profileId}", Name = RouteConstants.ChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeProviderViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeRegistrationProviderAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var registeredProviders = await GetAoRegisteredProviders();
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
            viewModel.SelectedProviderUkprn = viewModel.SelectedProviderUkprn;
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-provider", Name = RouteConstants.SubmitChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(ChangeProviderViewModel model)
        {
            if (model == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registeredProviderViewModel = await GetAoRegisteredProviders();

            if (!ModelState.IsValid)
            {
                model.ProvidersSelectList = registeredProviderViewModel.ProvidersSelectList;
                return View(model);
            }

            var response = await _registrationLoader.ProcessProviderChangesAsync(User.GetUkPrn(), model);

            return RedirectToRoute(RouteConstants.RegistrationDetails);
        }
        private async Task<SelectProviderViewModel> GetAoRegisteredProviders()
        {
            return await _registrationLoader.GetRegisteredTqAoProviderDetailsAsync(User.GetUkPrn());
        }
    }
}