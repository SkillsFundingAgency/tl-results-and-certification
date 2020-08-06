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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Linq;
using System.Threading.Tasks;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationLoader _registrationLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.RegistrationCacheKey); }
        }

        public RegistrationController(IRegistrationLoader registrationLoader, ICacheService cacheService, ILogger<RegistrationController> logger)
        {
            _registrationLoader = registrationLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("registrations", Name = RouteConstants.RegistrationDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-registrations-file", Name = RouteConstants.UploadRegistrationsFile)]
        public IActionResult UploadRegistrationsFile()
        {
            return View(new UploadRegistrationsRequestViewModel());
        }

        [HttpPost]
        [Route("upload-registrations-file", Name = RouteConstants.SubmitUploadRegistrationsFile)]
        public async Task<IActionResult> UploadRegistrationsFileAsync(UploadRegistrationsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _registrationLoader.ProcessBulkRegistrationsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.RegistrationsUploadSuccessful);
            }
            else
            {
                if (response.ShowProblemWithServicePage)
                {
                    return RedirectToRoute(RouteConstants.ProblemWithRegistrationsUpload);
                }
                else
                {
                    var unsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = response.BlobUniqueReference, FileSize = response.ErrorFileSize, FileType = FileType.Csv.ToString().ToUpperInvariant() };
                    await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel), unsuccessfulViewModel, CacheExpiryTime.XSmall);
                    return RedirectToRoute(RouteConstants.RegistrationsUploadUnsuccessful);
                }
            }
        }

        [HttpGet]
        [Route("registrations-upload-successful", Name = RouteConstants.RegistrationsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadSuccessfulViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed,
                    $"Unable to read upload successful registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("registrations-upload-unsuccessful", Name = RouteConstants.RegistrationsUploadUnsuccessful)]
        public async Task<IActionResult> UploadUnsuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("problem-with-registrations-upload", Name = RouteConstants.ProblemWithRegistrationsUpload)]
        public IActionResult ProblemWithRegistrationsUpload()
        {
            return View();
        }

        [HttpGet]
        [Route("download-registration-errors", Name = RouteConstants.DownloadRegistrationErrors)]
        public async Task<IActionResult> DownloadRegistrationErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _registrationLoader.GetRegistrationValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download registration validation errors. Method: GetRegistrationValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = RegistrationContent.UploadUnsuccessful.Registrations_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadRegistrationErrorsFailed, $"Not a valid guid to read file.Method: DownloadRegistrationErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("add-registration-unique-learner", Name = RouteConstants.AddRegistration)]
        public async Task<IActionResult> AddRegistrationAsync()
        {
            await _cacheService.RemoveAsync<RegistrationViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AddRegistrationUln);
        }

        [HttpGet]
        [Route("add-registration-unique-learner-number/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationUln)]
        public async Task<IActionResult> AddRegistrationUlnAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            var viewModel = cacheModel?.Uln != null ? cacheModel.Uln : new UlnViewModel();

            viewModel.IsChangeMode = isChangeMode && (cacheModel?.IsChangeModeAllowed == true);
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-unique-learner-number", Name = RouteConstants.SubmitRegistrationUln)]
        public async Task<IActionResult> AddRegistrationUlnAsync(UlnViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await SyncCacheUln(model);

            var findUln = await _registrationLoader.FindUlnAsync(User.GetUkPrn(), model.Uln.ToLong());
            if (findUln != null && findUln.IsUlnRegisteredAlready)
            {
                findUln.IsChangeMode = model.IsChangeMode;
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UlnNotFoundViewModel), findUln, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.UlnCannotBeRegistered);
            }

            return model.IsChangeMode ? RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit) : RedirectToRoute(RouteConstants.AddRegistrationLearnersName);
        }

        [HttpGet]
        [Route("ULN-cannot-be-registered", Name = RouteConstants.UlnCannotBeRegistered)]
        public async Task<IActionResult> UlnCannotBeRegistered()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UlnNotFoundViewModel>(string.Concat(CacheKey, Constants.UlnNotFoundViewModel));
            return viewModel == null ? RedirectToRoute(RouteConstants.PageNotFound) : (IActionResult)View(viewModel);
        }

        [HttpGet]
        [Route("add-registration-learners-name/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.LearnersName == null ? new LearnersNameViewModel() : cacheModel.LearnersName;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-learners-name", Name = RouteConstants.SubmitRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync(LearnersNameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.LearnersName = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return model.IsChangeMode ? RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit) : RedirectToRoute(RouteConstants.AddRegistrationDateofBirth);
        }

        [HttpGet]
        [Route("add-registration-date-of-birth/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationDateofBirth)]
        public async Task<IActionResult> AddRegistrationDateofBirthAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.LearnersName == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.DateofBirth == null ? new DateofBirthViewModel() : cacheModel.DateofBirth;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-date-of-birth", Name = RouteConstants.SubmitRegistrationDateofBirth)]
        public async Task<IActionResult> AddRegistrationDateofBirthAsync(DateofBirthViewModel model)
        {
            if (!IsValidDateofBirth(model))
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.LearnersName == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.DateofBirth = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return model.IsChangeMode ? RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit) : RedirectToRoute(RouteConstants.AddRegistrationProvider);
        }

        [HttpGet]
        [Route("add-registration-provider/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationProvider)]
        public async Task<IActionResult> AddRegistrationProviderAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.DateofBirth == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registeredProviders = await GetAoRegisteredProviders();
            var viewModel = cacheModel?.SelectProvider == null ? new SelectProviderViewModel() : cacheModel.SelectProvider;
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForProvider;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-provider", Name = RouteConstants.SubmitRegistrationProvider)]
        public async Task<IActionResult> AddRegistrationProviderAsync(SelectProviderViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.DateofBirth == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registeredProviderViewModel = await GetAoRegisteredProviders();

            if (!ModelState.IsValid)
            {
                model.ProvidersSelectList = registeredProviderViewModel.ProvidersSelectList;
                return View(model);
            }

            if (cacheModel?.SelectProvider?.SelectedProviderUkprn != model.SelectedProviderUkprn)
            {
                cacheModel.SelectCore = null;
                cacheModel.SpecialismQuestion = null;
                cacheModel.SelectSpecialism = null;
            }

            model.SelectedProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text;
            cacheModel.SelectProvider = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return model.IsChangeMode ? RedirectToRoute(RouteConstants.AddRegistrationCore, new { isChangeMode = true }) : RedirectToRoute(RouteConstants.AddRegistrationCore);
        }

        [HttpGet]
        [Route("add-registration-core/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationCore)]
        public async Task<IActionResult> AddRegistrationCoreAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var providerCores = await GetRegisteredProviderCores(cacheModel.SelectProvider.SelectedProviderUkprn.ToLong());
            var viewModel = cacheModel?.SelectCore == null ? new SelectCoreViewModel() : cacheModel.SelectCore;
            viewModel.CoreSelectList = providerCores.CoreSelectList;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForCore;
            viewModel.IsChangeModeFromProvider = cacheModel.SelectProvider.IsChangeMode;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-core", Name = RouteConstants.SubmitRegistrationCore)]
        public async Task<IActionResult> AddRegistrationCoreAsync(SelectCoreViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.SelectProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var coreViewModel = await GetRegisteredProviderCores(cacheModel.SelectProvider.SelectedProviderUkprn.ToLong());
            if (!ModelState.IsValid)
            {
                model.CoreSelectList = coreViewModel.CoreSelectList;
                return View(model);
            }

            if (cacheModel?.SelectCore?.SelectedCoreCode != model.SelectedCoreCode)
            {
                cacheModel.SpecialismQuestion = null;
                cacheModel.SelectSpecialism = null;
            }

            model.SelectedCoreDisplayName = coreViewModel?.CoreSelectList?.FirstOrDefault(p => p.Value == model.SelectedCoreCode)?.Text;
            cacheModel.SelectCore = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return model.IsChangeMode ? RedirectToRoute(RouteConstants.AddRegistrationSpecialismQuestion, new { isChangeMode = true }) : RedirectToRoute(RouteConstants.AddRegistrationSpecialismQuestion);
        }

        [HttpGet]
        [Route("add-registration-learner-decided-specialism-question/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationSpecialismQuestion)]
        public async Task<IActionResult> AddRegistrationSpecialismQuestionAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialismQuestion == null ? new SpecialismQuestionViewModel() : cacheModel.SpecialismQuestion;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForSpecialismQuestion;
            viewModel.IsChangeModeFromCore = cacheModel.SelectCore.IsChangeMode;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-learner-decided-specialism-question", Name = RouteConstants.SubmitRegistrationSpecialismQuestion)]
        public async Task<IActionResult> AddRegistrationSpecialismQuestionAsync(SpecialismQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.SelectCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!model.HasLearnerDecidedSpecialism.Value)
            {
                cacheModel.SelectSpecialism = null;
            }

            cacheModel.SpecialismQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if(model.IsChangeMode)
            {
                return model.HasLearnerDecidedSpecialism.Value ? RedirectToRoute(RouteConstants.AddRegistrationSpecialism, new { isChangeMode = true }) : RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit);
            }
            else
            {
                return RedirectToRoute(model.HasLearnerDecidedSpecialism.Value ? RouteConstants.AddRegistrationSpecialism : RouteConstants.AddRegistrationAcademicYear);
            }
        }

        [HttpGet]
        [Route("add-registration-specialism/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationSpecialism)]
        public async Task<IActionResult> AddRegistrationSpecialismAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectCore == null || cacheModel?.SpecialismQuestion == null || (!isChangeMode && cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SelectSpecialism == null ? new SelectSpecialismViewModel { PathwaySpecialisms = await GetPathwaySpecialismsByCoreCode(cacheModel.SelectCore.SelectedCoreCode) } : cacheModel.SelectSpecialism;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowedForSelectSpecialism;
            viewModel.IsChangeModeFromSpecialismQuestion = cacheModel.SpecialismQuestion.IsChangeMode;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-specialism", Name = RouteConstants.SubmitRegistrationSpecialism)]
        public async Task<IActionResult> AddRegistrationSpecialismAsync(SelectSpecialismViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.SelectCore == null || cacheModel?.SpecialismQuestion == null || (!model.IsChangeMode && cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false))
                return RedirectToRoute(RouteConstants.PageNotFound);

            if(model.IsChangeMode && cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism.Value == false)
            {
                cacheModel.SpecialismQuestion.HasLearnerDecidedSpecialism = true;
            }

            cacheModel.SelectSpecialism = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(model.IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.AddRegistrationAcademicYear);
        }

        [HttpGet]
        [Route("add-registration-academic-year/{isChangeMode:bool?}", Name = RouteConstants.AddRegistrationAcademicYear)]
        public async Task<IActionResult> AddRegistrationAcademicYearAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SpecialismQuestion == null || (cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == true && cacheModel?.SelectSpecialism == null))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var hasSpecialismsSelected = cacheModel?.SelectSpecialism != null;

            SelectAcademicYearViewModel viewModel;

            if (cacheModel?.SelectAcademicYear == null)
            {
                viewModel = new SelectAcademicYearViewModel { HasSpecialismsSelected = hasSpecialismsSelected };
            }
            else
            {
                cacheModel.SelectAcademicYear.HasSpecialismsSelected = hasSpecialismsSelected;
                viewModel = cacheModel?.SelectAcademicYear;
            }
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-academic-year", Name = RouteConstants.SubmitRegistrationAcademicYear)]
        public async Task<IActionResult> AddRegistrationAcademicYearAsync(SelectAcademicYearViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (model == null || !model.IsValidAcademicYear || cacheModel?.SpecialismQuestion == null || (cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == true && cacheModel?.SelectSpecialism == null))
                return RedirectToRoute(RouteConstants.PageNotFound);

            model.HasSpecialismsSelected = cacheModel?.SelectSpecialism != null;
            cacheModel.SelectAcademicYear = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-registration-check-and-submit", Name = RouteConstants.AddRegistrationCheckAndSubmit)]
        public async Task<IActionResult> AddRegistrationCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            var viewModel = new CheckAndSubmitViewModel { RegistrationModel = cacheModel };

            if (!viewModel.IsCheckAndSubmitPageValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            await _cacheService.SetAsync(CacheKey, viewModel.ResetChangeMode());
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-check-and-submit", Name = RouteConstants.SubmitRegistrationCheckAndSubmit)]
        public async Task<IActionResult> SubmitRegistrationCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var isSuccess = await _registrationLoader.AddRegistrationAsync(User.GetUkPrn(), cacheModel);

            if (isSuccess)
            {
                await _cacheService.RemoveAsync<RegistrationViewModel>(CacheKey);
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.RegistrationConfirmationViewModel), new RegistrationConfirmationViewModel { UniqueLearnerNumber = cacheModel.Uln.Uln }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AddRegistrationConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.ManualRegistrationProcessFailed, $"Unable to add registration for UniqueLearnerNumber = {cacheModel.Uln}. Method: SubmitRegistrationCheckAndSubmitAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("add-registration-confirmation", Name = RouteConstants.AddRegistrationConfirmation)]
        public async Task<IActionResult> AddRegistrationConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RegistrationConfirmationViewModel>(string.Concat(CacheKey, Constants.RegistrationConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read RegistrationConfirmationViewModel from temp data in add registration confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("search-for-registration", Name = RouteConstants.SearchRegistration)]
        public async Task<IActionResult> SearchRegistration()
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(Constants.RegistrationSearchCriteria);
            var viewModel = new SearchRegistrationViewModel { SearchUln = defaultValue };
            return View(viewModel);
        }

        [HttpPost]
        [Route("search-for-registration", Name = RouteConstants.SubmitSearchRegistration)]
        public async Task<IActionResult> SearchRegistrationAsync(SearchRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var searchResult = await _registrationLoader.FindUlnAsync(User.GetUkPrn(), model.SearchUln.ToLong());

            if (searchResult?.IsActive == true)
            {
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { profileId = searchResult.RegistrationProfileId });
            }
            else
            {
                await _cacheService.SetAsync(Constants.RegistrationSearchCriteria, model.SearchUln);

                var ulnNotfoundModel = new UlnNotFoundViewModel { Uln = model.SearchUln.ToString(), BackLinkRouteName = RouteConstants.SearchRegistration };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.SearchRegistrationUlnNotFound), ulnNotfoundModel, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.SearchRegistrationNotFound);
            }
        }

        [HttpGet]
        [Route("search-for-registration-ULN-not-found", Name = RouteConstants.SearchRegistrationNotFound)]
        public async Task<IActionResult> SearchRegistrationNotFound()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UlnNotFoundViewModel>(string.Concat(CacheKey, Constants.SearchRegistrationUlnNotFound));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read SearchRegistrationUlnNotFound from temp data in search registration not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("search-for-registration-registration-details/{profileId}", Name = RouteConstants.RegistrationDetails)]
        public async Task<IActionResult> RegistrationDetailsAsync(int profileId)
        {
            var viewModel = await _registrationLoader.GetRegistrationDetailsByProfileIdAsync(User.GetUkPrn(), profileId);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No registration details found. Method: GetRegistrationDetailsByProfileIdAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("cancel-registration/{profileId}", Name = RouteConstants.CancelRegistration)]
        public async Task<IActionResult> CancelRegistrationAsync(int profileId)
        {
            var ulnDetails = await _registrationLoader.GetRegistrationDetailsByProfileIdAsync(User.GetUkPrn(), profileId);
            if (ulnDetails == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new CancelRegistrationViewModel { ProfileId = ulnDetails.ProfileId, Uln = ulnDetails.Uln };
            return View(viewModel);
        }

        [HttpPost]
        [Route("cancel-registration", Name = RouteConstants.SubmitCancelRegistration)]
        public async Task<IActionResult> CancelRegistrationAsync(CancelRegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (!viewModel.CancelRegistration.Value)
                return RedirectToRoute(RouteConstants.RegistrationDetails, new { profileId = viewModel.ProfileId });

            var isSuccess = await _registrationLoader.DeleteRegistrationAsync(User.GetUkPrn(), viewModel.ProfileId);

            if (isSuccess)
            {
                await _cacheService.SetAsync(CacheKey, new RegistrationCancelledConfirmationViewModel { Uln = viewModel.Uln }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RegistrationCancelledConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.RegistrationNotDeleted, $"Unable to delete registration. Method: DeleteRegistrationAsync(Ukprn: {User.GetUkPrn()}, id: {viewModel.ProfileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("registration-cancelled-confirmation", Name = RouteConstants.RegistrationCancelledConfirmation)]
        public async Task<IActionResult> RegistrationCancelledConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<RegistrationCancelledConfirmationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed,
                    $"Unable to read cancel registration confirmation viewmodel from cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
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

        private async Task<PathwaySpecialismsViewModel> GetPathwaySpecialismsByCoreCode(string coreCode)
        {
            return await _registrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(User.GetUkPrn(), coreCode);
        }

        private async Task SyncCacheUln(UlnViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.Uln != null)
                cacheModel.Uln = model;
            else
                cacheModel = new RegistrationViewModel { Uln = model };

            await _cacheService.SetAsync(CacheKey, cacheModel);
        }

        private bool IsValidDateofBirth(DateofBirthViewModel model)
        {
            var dateofBirth = string.Concat(model.Day, "/", model.Month, "/", model.Year);
            var validationerrors = dateofBirth.ValidateDate("Date of birth");

            if (validationerrors?.Count == 0)
                return true;

            foreach (var error in validationerrors)
                ModelState.AddModelError(error.Key, error.Value);

            return false;
        }
    }
}