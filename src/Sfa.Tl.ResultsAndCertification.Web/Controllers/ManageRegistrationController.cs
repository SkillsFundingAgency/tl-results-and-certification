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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Linq;
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
            if (!IsValidDateofBirth(viewModel))
                return View(viewModel);

            var response = await _registrationLoader.ProcessDateofBirthChangeAsync(User.GetUkPrn(), viewModel);

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
        [Route("change-provider/{profileId}/{isback:bool?}", Name = RouteConstants.ChangeRegistrationProvider)]
        public async Task<IActionResult> ChangeProviderAsync(int profileId, bool isback = false)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeProviderViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeProviderAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var registeredProviders = await GetAoRegisteredProviders();
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
            
            if (isback)
                viewModel.SelectedProviderUkprn = TempData.Get<string>(Constants.ChangeRegistrationCoreNotSupportedProviderUkprn) ?? viewModel.SelectedProviderUkprn;
            
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
            {
                TempData.Set(Constants.ChangeRegistrationCoreNotSupportedProviderUkprn, model.SelectedProviderUkprn);
                var providerDetailsModel = new ChangeProviderCoreNotSupportedViewModel { ProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel), providerDetailsModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ChangeRegistrationCoreQuestion, new { profileId = model.ProfileId });
            }

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("change-core-and-provider/{profileId}", Name = RouteConstants.ChangeRegistrationCoreQuestion)]
        public async Task<IActionResult> ChangeCoreQuestionAsync(int profileId)
        {
            var cacheViewModel = await _cacheService.GetAndRemoveAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeProviderCoreNotSupportedViewModel from redis cache in ChangeCoreQuestion page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = await GetChangeCoreQuestionDetailsAsync(profileId, cacheViewModel);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration change core question details found. Method: ChangeCoreQuestionAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }        

        [HttpPost]
        [Route("change-core-and-provider", Name = RouteConstants.SubmitChangeCoreQuestion)]
        public async Task<IActionResult> ChangeCoreQuestionAsync(ChangeCoreQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheViewModel = new ChangeProviderCoreNotSupportedViewModel { ProfileId = model.ProfileId, ProviderDisplayName = model.ProviderDisplayName, CoreDisplayName = model.CoreDisplayName, CanChangeCore = model.CanChangeCore };
            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel), cacheViewModel);
            return RedirectToRoute(model.CanChangeCore == true ? RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw : RouteConstants.ChangeRegistrationProviderNotOfferingSameCore); 
        }

        [HttpGet]
        [Route("change-registration-provider-and-core-need-to-withdraw", Name = RouteConstants.ChangeRegistrationProviderAndCoreNeedToWithdraw)]
        public async Task<IActionResult> ChangeProviderAndCoreNeedToWithdrawAsync()
        {
            var cacheViewModel = await _cacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeCoreProviderDetailsViewModel from redis cache in ChangeProviderAndCoreNeedToWithdraw page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new ChangeProviderAndCoreNeedToWithdrawViewModel { ProfileId = cacheViewModel.ProfileId };
            return View(viewModel);
        }

        [HttpGet]
        [Route("provider-not-offering-same-core", Name = RouteConstants.ChangeRegistrationProviderNotOfferingSameCore)]
        public async Task<IActionResult> ChangeProviderNotOfferingSameCoreAsync()
        {
            var cacheViewModel = await _cacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(string.Concat(CacheKey, Constants.ChangeRegistrationProviderCoreNotSupportedViewModel));
            if (cacheViewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ChangeCoreProviderDetailsViewModel from redis cache in ChangeProviderNotOfferingSameCore page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new ChangeProviderNotOfferingSameCoreViewModel { ProfileId = cacheViewModel.ProfileId, ProviderDisplayName = cacheViewModel.ProviderDisplayName, CoreDisplayName = cacheViewModel.CoreDisplayName };
            return View(viewModel);
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

            if (model.HasLearnerDecidedSpecialism.HasValue && model.HasLearnerDecidedSpecialism.Value)
            {
                return RedirectToRoute(RouteConstants.ChangeRegistrationSpecialisms, new { profileId = model.ProfileId });
            }
            else
            {
                var response = await _registrationLoader.ProcessSpecialismQuestionChangeAsync(User.GetUkPrn(), model);

                if (response == null || !response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("change-registration-select-specialism/{profileId}", Name = RouteConstants.ChangeRegistrationSpecialisms)]
        public async Task<IActionResult> ChangeRegistrationSpecialismsAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeRegistrationSpecialismsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.PathwaySpecialisms = await GetPathwaySpecialismsAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-registration-select-specialism", Name = RouteConstants.SubmitChangeRegistrationSpecialisms)]
        public async Task<IActionResult> ChangeRegistrationSpecialismsAsync(ChangeSpecialismViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(User.GetUkPrn(), viewModel.ProfileId);
                if (viewModel == null)
                    return RedirectToRoute(RouteConstants.PageNotFound);

                viewModel.SpecialismCodes = model.SpecialismCodes;
                return View(viewModel);
            }

            var response = await _registrationLoader.ProcessSpecialismChangeAsync(User.GetUkPrn(), viewModel);
            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeRegistrationConfirmationViewModel), response as ManageRegistrationResponse, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeRegistrationConfirmation);
        }

        [HttpGet]
        [Route("academic-year-cannot-change/{profileId}", Name = RouteConstants.ChangeAcademicYear)]
        public async Task<IActionResult> ChangeAcademicYearAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: ChangeAcademicYearAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("amend-active-registration/{profileId}", Name = RouteConstants.AmendActiveRegistration)]
        public async Task<IActionResult> AmendActiveRegistrationAsync(int profileId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsByProfileIdAsync(User.GetUkPrn(), profileId);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: AmendActiveRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = new AmendActiveRegistrationViewModel { ProfileId = registrationDetails.ProfileId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("amend-active-registration", Name = RouteConstants.SubmitAmendActiveRegistration)]
        public IActionResult AmendActiveRegistrationAsync(AmendActiveRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return View(model);
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

        private async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsAsync(ChangeSpecialismViewModel viewModel)
        {
            var coreSpecialisms = await _registrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(User.GetUkPrn(), viewModel.CoreCode);
            
            // Update IsSelected flag.
            coreSpecialisms.Specialisms.ToList().ForEach(x => { x.IsSelected = viewModel.SpecialismCodes.Contains(x.Code); });
            
            return coreSpecialisms;
        }

        private async Task<ChangeCoreQuestionViewModel> GetChangeCoreQuestionDetailsAsync(int profileId, ChangeProviderCoreNotSupportedViewModel providerViewModel)
        {
            var coreQuestionDetails = await _registrationLoader.GetRegistrationChangeCoreQuestionDetailsAsync(User.GetUkPrn(), profileId);

            if (coreQuestionDetails != null)
            {
                coreQuestionDetails.ProviderDisplayName = providerViewModel?.ProviderDisplayName;
                coreQuestionDetails.CanChangeCore = providerViewModel.CanChangeCore;
            }
            return coreQuestionDetails;
        }
    }
}