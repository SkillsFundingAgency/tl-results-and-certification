using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

        private string ReregisterCacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.ReregisterCacheKey); }
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
        [Route("change-registration-learners-name/{profileId}", Name = RouteConstants.ChangeRegistrationLearnersName)]
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
        [Route("change-registration-learners-name", Name = RouteConstants.SubmitChangeRegistrationLearnersName)]
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
        [Route("change-registration-learners-date-of-birth/{profileId}", Name = RouteConstants.ChangeRegistrationDateofBirth)]
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
        [Route("change-registration-learners-date-of-birth", Name = RouteConstants.SubmitChangeRegistrationDateofBirth)]
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
        [Route("change-registration-provider/{profileId}/{isback:bool?}", Name = RouteConstants.ChangeRegistrationProvider)]
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
        [Route("change-registration-provider", Name = RouteConstants.SubmitChangeRegistrationProvider)]
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
        [Route("change-registration-provider-not-offering-core", Name = RouteConstants.ChangeRegistrationProviderNotOfferingSameCore)]
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
        [Route("registration-specialism-change-restriction/{profileId}", Name = RouteConstants.ChangeSpecialismRestriction)]
        public IActionResult ChangeSpecialismRestriction(int profileId)
        {
            var viewModel = new ChangeSpecialismRestrictionViewModel
            {
                ProfileId = profileId
            };
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
        public async Task<IActionResult> ChangeSpecialismsAsync(int profileId)
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
        public async Task<IActionResult> ChangeSpecialismsAsync(ChangeSpecialismViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = await _registrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(User.GetUkPrn(), viewModel.ProfileId);
                if (model == null)
                    return RedirectToRoute(RouteConstants.PageNotFound);

                viewModel.SpecialismCodes = model.SpecialismCodes;
                return View(viewModel);
            }

            viewModel.PathwaySpecialisms?.Specialisms?.ToList().ForEach(x => { x.IsSelected = (x.Code == viewModel.SelectedSpecialismCode); });
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
        [Route("amend-active-registration/{profileId}/{changeStatusId:int?}", Name = RouteConstants.AmendActiveRegistration)]
        public async Task<IActionResult> AmendActiveRegistrationAsync(int profileId, int? changeStatusId)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: AmendActiveRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = new AmendActiveRegistrationViewModel { ProfileId = registrationDetails.ProfileId, ChangeStatusId = changeStatusId };
            viewModel.SetChangeStatus();
            return View(viewModel);
        }

        [HttpPost]
        [Route("amend-active-registration", Name = RouteConstants.SubmitAmendActiveRegistration)]
        public async Task<IActionResult> AmendActiveRegistrationAsync(AmendActiveRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ChangeStatus == RegistrationChangeStatus.Withdrawn || model.ChangeStatus == RegistrationChangeStatus.Delete)
            {
                var registrationAssessmentDetails = await _registrationLoader.GetRegistrationAssessmentAsync(User.GetUkPrn(), model.ProfileId, RegistrationPathwayStatus.Active);
                if (registrationAssessmentDetails == null)
                    return RedirectToRoute(RouteConstants.PageNotFound);

                if (model.ChangeStatus == RegistrationChangeStatus.Withdrawn)
                {
                    if (registrationAssessmentDetails.HasAnyOutstandingPathwayPrsActivities)
                    {
                        var cannotBeWithdrawnViewModel = new RegistrationCannotBeWithdrawnViewModel { ProfileId = registrationAssessmentDetails.ProfileId };
                        await _cacheService.SetAsync(CacheKey, cannotBeWithdrawnViewModel, CacheExpiryTime.XSmall);
                        return RedirectToRoute(RouteConstants.RegistrationCannotBeWithdrawn);
                    }
                    return RedirectToRoute(RouteConstants.WithdrawRegistration, new { profileId = model.ProfileId, withdrawBackLinkOptionId = (int)WithdrawBackLinkOptions.AmendActiveRegistrationPage });
                }

                if (model.ChangeStatus == RegistrationChangeStatus.Delete)
                {
                    if (registrationAssessmentDetails.IsCoreResultExist || registrationAssessmentDetails.IsIndustryPlacementExist)
                    {
                        var cannotBeDeletedViewModel = new RegistrationCannotBeDeletedViewModel { ProfileId = registrationAssessmentDetails.ProfileId };
                        await _cacheService.SetAsync(string.Concat(CacheKey, Constants.RegistrationCannotBeDeletedViewModel), cannotBeDeletedViewModel, CacheExpiryTime.XSmall);
                        return RedirectToRoute(RouteConstants.RegistrationCannotBeDeleted);
                    }

                    return RedirectToRoute(RouteConstants.DeleteRegistration, new { profileId = model.ProfileId });
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("withdraw-registration/{profileId}/{withdrawBackLinkOptionId:int?}", Name = RouteConstants.WithdrawRegistration)]
        public async Task<IActionResult> WithdrawRegistrationAsync(int profileId, int? withdrawBackLinkOptionId)
        {
            var registrationAssessmentDetails = await _registrationLoader.GetRegistrationAssessmentAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (registrationAssessmentDetails == null || registrationAssessmentDetails.PathwayStatus != RegistrationPathwayStatus.Active || registrationAssessmentDetails.HasAnyOutstandingPathwayPrsActivities)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: WithdrawRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new WithdrawRegistrationViewModel { ProfileId = registrationAssessmentDetails.ProfileId, Uln = registrationAssessmentDetails.Uln, WithdrawBackLinkOptionId = withdrawBackLinkOptionId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("withdraw-registration", Name = RouteConstants.SubmitWithdrawRegistration)]
        public async Task<IActionResult> WithdrawRegistrationAsync(WithdrawRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.CanWithdraw.Value)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }
            else
            {
                var registrationAssessmentDetails = await _registrationLoader.GetRegistrationAssessmentAsync(User.GetUkPrn(), model.ProfileId, RegistrationPathwayStatus.Active);
                if (registrationAssessmentDetails == null || registrationAssessmentDetails.HasAnyOutstandingPathwayPrsActivities) // TODO: Can this check be moved to Api
                    return RedirectToRoute(RouteConstants.PageNotFound);

                var response = await _registrationLoader.WithdrawRegistrationAsync(User.GetUkPrn(), model);

                if (!response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.WithdrawRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.WithdrawRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("registration-withdrawn-confirmation", Name = RouteConstants.WithdrawRegistrationConfirmation)]
        public async Task<IActionResult> WithdrawConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<WithdrawRegistrationResponse>(string.Concat(CacheKey, Constants.WithdrawRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read WithdrawRegistrationConfirmationViewModel from redis cache in withdraw registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("amend-withdrawn-registration/{profileId}/{changeStatusId:int?}", Name = RouteConstants.AmendWithdrawRegistration)]
        public async Task<IActionResult> AmendWithdrawRegistrationAsync(int profileId, int? changeStatusId)
        {
            await _cacheService.RemoveAsync<ReregisterViewModel>(ReregisterCacheKey);
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: AmendWithdrawRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var viewModel = new AmendWithdrawRegistrationViewModel { ProfileId = registrationDetails.ProfileId, ChangeStatusId = changeStatusId };
            viewModel.SetChangeStatus();
            return View(viewModel);
        }

        [HttpPost]
        [Route("amend-withdrawn-registration", Name = RouteConstants.SubmitAmendWithdrawRegistration)]
        public IActionResult AmendWithdrawRegistrationAsync(AmendWithdrawRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.ChangeStatus == RegistrationChangeStatus.Rejoin)
            {
                return RedirectToRoute(RouteConstants.RejoinRegistration, new { profileId = model.ProfileId });
            }
            else if (model.ChangeStatus == RegistrationChangeStatus.Reregister)
            {
                return RedirectToRoute(RouteConstants.ReregisterProvider, new { profileId = model.ProfileId });
            }

            return View(model);
        }

        [HttpGet]
        [Route("reactivate-registration-same-course/{profileId}/{isFromCoreDenialPage:bool?}/{isChangeMode:bool?}", Name = RouteConstants.RejoinRegistration)]
        public async Task<IActionResult> RejoinRegistrationAsync(int profileId, bool isFromCoreDenialPage, bool isChangeMode)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: RejoinRegistrationAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new RejoinRegistrationViewModel { ProfileId = registrationDetails.ProfileId, Uln = registrationDetails.Uln, IsFromCoreDenialPage = isFromCoreDenialPage, IsChangeMode = isChangeMode };
            return View(viewModel);
        }

        [HttpPost]
        [Route("reactivate-registration-same-course", Name = RouteConstants.SubmitRejoinRegistration)]
        public async Task<IActionResult> RejoinRegistrationAsync(RejoinRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.CanRejoin.Value)
            {
                return RedirectToRoute(model.BackLink.RouteName, model.BackLink.RouteAttributes);
            }
            else
            {
                var response = await _registrationLoader.RejoinRegistrationAsync(User.GetUkPrn(), model);

                if (!response.IsSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.RejoinRegistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RejoinRegistrationConfirmation);
            }
        }

        [HttpGet]
        [Route("registration-reactivated-confirmation", Name = RouteConstants.RejoinRegistrationConfirmation)]
        public async Task<IActionResult> RejoinConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RejoinRegistrationResponse>(string.Concat(CacheKey, Constants.RejoinRegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read RejoinRegistrationConfirmationViewModel from redis cache in Rejoin registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("register-learner-new-course-select-provider/{profileId}/{isChangeMode:bool?}/{isFromConfirmation:bool?}", Name = RouteConstants.ReregisterProvider)]
        public async Task<IActionResult> ReregisterProviderAsync(int profileId, bool isChangeMode, bool isFromConfirmation)
        {
            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterProviderAsync({profileId}), Ukprn: {User.GetUkPrn()}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            var registeredProviders = await GetAoRegisteredProviders();
            var viewModel = cacheModel?.ReregisterProvider == null ? new ReregisterProviderViewModel() : cacheModel.ReregisterProvider;
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;

            viewModel.ProfileId = profileId;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForProvider;
            viewModel.IsFromConfirmation = isFromConfirmation;

            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-select-provider", Name = RouteConstants.SubmitReregisterProvider)]
        public async Task<IActionResult> ReregisterProviderAsync(ReregisterProviderViewModel model)
        {
            var registeredProviderViewModel = await GetAoRegisteredProviders();

            if (!ModelState.IsValid)
            {
                model.ProvidersSelectList = registeredProviderViewModel.ProvidersSelectList;
                return View(model);
            }

            model.SelectedProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text;

            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel?.ReregisterProvider != null)
            {
                if (cacheModel.ReregisterProvider.SelectedProviderUkprn != model.SelectedProviderUkprn)
                {
                    cacheModel.ReregisterCore = null;
                    cacheModel.SpecialismQuestion = null;
                    cacheModel.ReregisterSpecialisms = null;
                }
                cacheModel.ReregisterProvider = model;
            }
            else
                cacheModel = new ReregisterViewModel { ReregisterProvider = model };

            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);

            return model.IsChangeMode ?
                RedirectToRoute(RouteConstants.ReregisterCore, new { profileId = model.ProfileId, isChangeMode = "true" }) :
                RedirectToRoute(RouteConstants.ReregisterCore, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("register-learner-new-course-select-core/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.ReregisterCore)]
        public async Task<IActionResult> ReregisterCoreAsync(int profileId, bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (cacheModel?.ReregisterProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterCoreAsync({profileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var providerCores = await GetRegisteredProviderCores(cacheModel.ReregisterProvider.SelectedProviderUkprn.ToLong());
            var viewModel = cacheModel?.ReregisterCore == null ? new ReregisterCoreViewModel() : cacheModel.ReregisterCore;
            viewModel.ProfileId = profileId;
            viewModel.CoreSelectList = providerCores.CoreSelectList;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForCore;
            viewModel.IsChangeModeFromProvider = cacheModel.ReregisterProvider.IsChangeMode;
            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-select-core", Name = RouteConstants.SubmitReregisterCore)]
        public async Task<IActionResult> ReregisterCoreAsync(ReregisterCoreViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (cacheModel?.ReregisterProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var coreViewModel = await GetRegisteredProviderCores(cacheModel.ReregisterProvider.SelectedProviderUkprn.ToLong());

            if (!ModelState.IsValid)
            {
                model.CoreSelectList = coreViewModel.CoreSelectList;
                return View(model);
            }

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), model.ProfileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: Post - ReregisterCoreAsync({model.ProfileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (cacheModel?.ReregisterCore?.SelectedCoreCode != model.SelectedCoreCode)
            {
                cacheModel.SpecialismQuestion = null;
                cacheModel.ReregisterSpecialisms = null;
            }

            model.CoreCodeAtTheTimeOfWithdrawn = registrationDetails.PathwayLarId;
            model.SelectedCoreDisplayName = coreViewModel?.CoreSelectList?.FirstOrDefault(p => p.Value == model.SelectedCoreCode)?.Text;
            cacheModel.ReregisterCore = model;

            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);
            var routeValues = model.IsChangeMode ? new RouteValueDictionary { { Constants.ProfileId, model.ProfileId }, { Constants.IsChangeMode, "true" } } : new RouteValueDictionary { { Constants.ProfileId, model.ProfileId } };
            return RedirectToRoute(model.IsValidCore ? RouteConstants.ReregisterSpecialismQuestion : RouteConstants.ReregisterCannotSelectSameCore, routeValues);
        }

        [HttpGet]
        [Route("cannot-select-same-core/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.ReregisterCannotSelectSameCore)]
        public async Task<IActionResult> ReregisterCannotSelectSameCoreAsync(int profileId, bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (cacheModel?.ReregisterCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterCannotSelectSameCoreAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new ReregisterCannotSelectSameCoreViewModel { ProfileId = profileId, IsChangeMode = isChangeMode };
            return View(viewModel);
        }

        [HttpGet]
        [Route("register-learner-new-course-has-learner-decided-specialism/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.ReregisterSpecialismQuestion)]
        public async Task<IActionResult> ReregisterSpecialismQuestionAsync(int profileId, bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel == null || cacheModel.ReregisterCore == null || !cacheModel.ReregisterCore.IsValidCore)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterSpecialismQuestionAsync({profileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = cacheModel?.SpecialismQuestion == null ? new ReregisterSpecialismQuestionViewModel() : cacheModel.SpecialismQuestion;
            viewModel.ProfileId = profileId;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForSpecialismQuestion;
            viewModel.IsChangeModeFromCore = cacheModel.ReregisterCore.IsChangeMode;

            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-has-learner-decided-specialism", Name = RouteConstants.SubmitReregisterSpecialismQuestion)]
        public async Task<IActionResult> ReregisterSpecialismQuestionAsync(ReregisterSpecialismQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (cacheModel == null || cacheModel.ReregisterCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!model.HasLearnerDecidedSpecialism.Value)
                cacheModel.ReregisterSpecialisms = null;

            cacheModel.SpecialismQuestion = model;
            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);

            if (model.IsChangeMode)
                if (model.HasLearnerDecidedSpecialism.Value)
                    return RedirectToRoute(RouteConstants.ReregisterSpecialisms, new { model.ProfileId, isChangeMode = "true" });
                else
                    return RedirectToRoute(RouteConstants.ReregisterCheckAndSubmit, new { model.ProfileId });

            return RedirectToRoute(model.HasLearnerDecidedSpecialism.Value ?
                RouteConstants.ReregisterSpecialisms : RouteConstants.ReregisterAcademicYear,
                new { model.ProfileId });
        }

        [HttpGet]
        [Route("register-learner-new-course-select-specialism/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.ReregisterSpecialisms)]
        public async Task<IActionResult> ReregisterSpecialismsAsync(int profileId, bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel?.ReregisterCore == null || cacheModel?.SpecialismQuestion == null ||
                (!isChangeMode && cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. Method: ReregisterSpecialismQuestionAsync({profileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = cacheModel?.ReregisterSpecialisms == null ? new ReregisterSpecialismViewModel { PathwaySpecialisms = await GetPathwaySpecialismsByCoreCode(cacheModel.ReregisterCore.SelectedCoreCode) } : cacheModel.ReregisterSpecialisms;
            viewModel.ProfileId = profileId;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForSelectSpecialism;
            viewModel.IsChangeModeFromSpecialismQuestion = cacheModel.SpecialismQuestion.IsChangeMode;

            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-select-specialism", Name = RouteConstants.SubmitReregisterSpecialisms)]
        public async Task<IActionResult> ReregisterSpecialismsAsync(ReregisterSpecialismViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel?.SpecialismQuestion == null ||
                (!model.IsChangeMode && cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false))
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.IsChangeMode && cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism.Value == false)
                cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism = true;

            model.PathwaySpecialisms?.Specialisms?.ToList().ForEach(x => { x.IsSelected = x.Code == model.SelectedSpecialismCode; });
            var pathwaySpecialisms = await GetPathwaySpecialismsByCoreCode(cacheModel.ReregisterCore.SelectedCoreCode);
            model.PathwaySpecialisms.SpecialismsLookup = pathwaySpecialisms?.SpecialismsLookup;
            cacheModel.ReregisterSpecialisms = model;
            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);

            return RedirectToRoute(model.IsChangeMode ? RouteConstants.ReregisterCheckAndSubmit : RouteConstants.ReregisterAcademicYear, new { model.ProfileId });
        }

        [HttpGet]
        [Route("register-learner-new-course-select-academic-year/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.ReregisterAcademicYear)]
        public async Task<IActionResult> ReregisterAcademicYearAsync(int profileId, bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            if (cacheModel == null || cacheModel.SpecialismQuestion == null ||
                (cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism == true && cacheModel.ReregisterSpecialisms == null))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. " +
                    $"Method: ReregisterAcademicYearAsync({profileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var hasSpecialismsSelected = cacheModel.ReregisterSpecialisms != null;
            ReregisterAcademicYearViewModel viewModel;
            if (cacheModel.ReregisterAcademicYear == null)
            {
                viewModel = new ReregisterAcademicYearViewModel { ProfileId = profileId, HasSpecialismsSelected = hasSpecialismsSelected, AcademicYears = await _registrationLoader.GetCurrentAcademicYearsAsync() };
            }
            else
            {
                cacheModel.ReregisterAcademicYear.HasSpecialismsSelected = hasSpecialismsSelected;
                viewModel = cacheModel.ReregisterAcademicYear;
            }
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-select-academic-year", Name = RouteConstants.SubmitReregisterAcademicYear)]
        public async Task<IActionResult> ReregisterAcademicYearAsync(ReregisterAcademicYearViewModel viewModel)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);
            viewModel.AcademicYears = await _registrationLoader.GetCurrentAcademicYearsAsync();

            if (cacheModel == null || cacheModel.SpecialismQuestion == null ||
                (cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism == true && cacheModel.ReregisterSpecialisms == null) ||
                !viewModel.IsValidAcademicYear)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.HasSpecialismsSelected = cacheModel.ReregisterSpecialisms != null;
            cacheModel.ReregisterAcademicYear = viewModel;
            await _cacheService.SetAsync(ReregisterCacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.ReregisterCheckAndSubmit, new { viewModel.ProfileId });
        }

        [HttpGet]
        [Route("register-learner-new-course-check-and-submit/{profileId}", Name = RouteConstants.ReregisterCheckAndSubmit)]
        public async Task<IActionResult> ReregisterCheckAndSubmitAsync(int profileId)
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            var viewModel = new ReregisterCheckAndSubmitViewModel { ReregisterModel = cacheModel };

            if (!viewModel.IsCheckAndSubmitPageValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registrationDetails = await _registrationLoader.GetRegistrationDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);
            if (registrationDetails == null || registrationDetails.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found with Status: {RegistrationPathwayStatus.Withdrawn}. " +
                    $"Method: ReregisterCheckAndSubmitAsync({profileId}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.Uln = registrationDetails.Uln;
            await _cacheService.SetAsync(ReregisterCacheKey, viewModel.ResetChangeMode());
            return View(viewModel);
        }

        [HttpPost]
        [Route("register-learner-new-course-check-and-submit", Name = RouteConstants.SubmitReregisterCheckAndSubmit)]
        public async Task<IActionResult> ReregisterCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<ReregisterViewModel>(ReregisterCacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var response = await _registrationLoader.ReregistrationAsync(User.GetUkPrn(), cacheModel);

            if (response == null || response.IsSelectedCoreSameAsWithdrawn || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.RemoveAsync<ReregisterViewModel>(ReregisterCacheKey);
            await _cacheService.SetAsync(string.Concat(ReregisterCacheKey, Constants.ReregistrationConfirmationViewModel), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ReregistrationConfirmation);
        }

        [HttpGet]
        [Route("new-course-registration-confirmation", Name = RouteConstants.ReregistrationConfirmation)]
        public async Task<IActionResult> ReregistrationConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ReregistrationResponse>(string.Concat(ReregisterCacheKey, Constants.ReregistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read ReregistrationConfirmationViewModel from redis cache re-registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("registration-cannot-be-deleted", Name = RouteConstants.RegistrationCannotBeDeleted)]
        public async Task<IActionResult> RegistrationCannotBeDeletedAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RegistrationCannotBeDeletedViewModel>(string.Concat(CacheKey, Constants.RegistrationCannotBeDeletedViewModel));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound,
                    $"Unable to read RegistrationCannotBeDeletedViewModel from cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("registration-cannot-be-withdrawn", Name = RouteConstants.RegistrationCannotBeWithdrawn)]
        public async Task<IActionResult> RegistrationCannotBeWithdrawnAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RegistrationCannotBeWithdrawnViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound,
                    $"Unable to read RegistrationCannotBeWithdrawnViewModel from cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        private async Task<SelectProviderViewModel> GetAoRegisteredProviders()
        {
            return await _registrationLoader.GetRegisteredTqAoProviderDetailsAsync(User.GetUkPrn());
        }

        private async Task<SelectCoreViewModel> GetRegisteredProviderCores(long providerUkprn)
        {
            return await _registrationLoader.GetRegisteredProviderPathwayDetailsAsync(User.GetUkPrn(), providerUkprn);
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
            viewModel.SelectedSpecialismCode = coreSpecialisms?.Specialisms?.FirstOrDefault(x => viewModel.SpecialismCodes.All(vm => x.Code.Split(Constants.PipeSeperator).Any(x => x.Equals(vm, System.StringComparison.InvariantCultureIgnoreCase)))).Code;

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

        private async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsByCoreCode(string coreCode)
        {
            return await _registrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(User.GetUkPrn(), coreCode);
        }
    }
}