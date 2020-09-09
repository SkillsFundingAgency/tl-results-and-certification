using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
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

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.RegistrationCacheKey); }
        }

        public ManageRegistrationController(
            IRegistrationLoader registrationLoader,
            ICacheService cacheService,
            ILogger<ManageRegistrationController> logger)
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
        public async Task<IActionResult> ChangeLearnersNameAsync(ChangeLearnersNameViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var response = await _registrationLoader.ProcessProfileNameChangeAsync(User.GetUkPrn(), viewModel);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("change-learners-date-of-birth/{profileId}", Name = RouteConstants.ChangeRegistrationDateofBirth)]
        public async Task<IActionResult> ChangeDateofBirthAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeDateofBirthViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeDateofBirthAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("change-learners-date-of-birth", Name = RouteConstants.SubmitChangeRegistrationDateofBirth)]
        public async Task<IActionResult> ChangeDateofBirthAsync(ChangeDateofBirthViewModel viewModel)
        {
            await Task.Run(() => true);
            if (!IsValidDateofBirth(viewModel))
                return View(viewModel);

            return RedirectToRoute(RouteConstants.PageNotFound);
        }

        [HttpGet]
        [Route("registration-details-change-confirmation", Name = RouteConstants.ChangeRegistrationConfirmation)]
        public async Task<IActionResult> ChangeConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ManageRegistrationResponse>(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read ChangeRegistrationConfirmationViewModel from redis cache in change registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("change-provider/{profileId}", Name = RouteConstants.ChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeProviderViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeProviderAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
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

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { profileId = model.ProfileId });

            if (response.IsCoreNotSupported)
                return RedirectToRoute(RouteConstants.CannotChangeRegistrationProvider);

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("cannot-change-provider", Name = RouteConstants.CannotChangeRegistrationProvider)]
        public IActionResult CannotChangeProviderAsync()
        {
            return View();
        }

        [HttpGet]
        [Route("change-core/{profileId}", Name = RouteConstants.ChangeRegistrationCore)]
        public async Task<IActionResult> ChangeCoreAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeCoreViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeCoreAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("change-registration-learner-decided-specialism-question/{profileId}", Name = RouteConstants.ChangeRegistrationSpecialismQuestion)]
        public async Task<IActionResult> ChangeRegistrationSpecialismQuestionAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismQuestionViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeRegistrationSpecialismQuestionAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-registration-learner-decided-specialism-question", Name = RouteConstants.SubmitChangeRegistrationSpecialismQuestion)]
        public async Task<IActionResult> ChangeRegistrationSpecialismQuestionAsync(ChangeSpecialismQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.HasLearnerDecidedSpecialism.Value)
            {
                return RedirectToRoute(RouteConstants.ChangeRegistrationSpecialisms, new { profileId = model.ProfileId });
            }
            else
            {
                var response = await _registrationLoader.ProcessProviderChangesAsync(User.GetUkPrn(), new ChangeProviderViewModel());

                if (!response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
            }
        }

        private async Task<SelectProviderViewModel> GetAoRegisteredProviders()
        {
            return await _registrationLoader.GetRegisteredTqAoProviderDetailsAsync(User.GetUkPrn());
        }

        private bool IsValidDateofBirth(ChangeDateofBirthViewModel model)
        {
            var validationerrors = model.DateofBirth.ValidateDate("Date of birth");
            
            if (validationerrors?.Count == 0)
                return true;

            foreach (var error in validationerrors)
                ModelState.AddModelError(error.Key, error.Value);

            return false;
        }
    }
}