using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                TempData[Constants.UploadSuccessfulViewModel] = JsonConvert.SerializeObject(successfulViewModel);
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
                    TempData[Constants.UploadUnsuccessfulViewModel] = JsonConvert.SerializeObject(unsuccessfulViewModel);
                    return RedirectToRoute(RouteConstants.RegistrationsUploadUnsuccessful);
                }
            }
        }

        [HttpGet]
        [Route("registrations-upload-successful", Name = RouteConstants.RegistrationsUploadSuccessful)]
        public IActionResult UploadSuccessful()
        {
            if (TempData[Constants.UploadSuccessfulViewModel] == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed,
                    $"Unable to read upload successful registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = JsonConvert.DeserializeObject<UploadSuccessfulViewModel>(TempData[Constants.UploadSuccessfulViewModel] as string);
            return View(viewModel);
        }

        [HttpGet]
        [Route("registrations-upload-unsuccessful", Name = RouteConstants.RegistrationsUploadUnsuccessful)]
        public IActionResult UploadUnsuccessful()
        {
            if (TempData[Constants.UploadUnsuccessfulViewModel] == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = JsonConvert.DeserializeObject<UploadUnsuccessfulViewModel>(TempData[Constants.UploadUnsuccessfulViewModel] as string);
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
        [Route("add-registration-unique-learner-number", Name = RouteConstants.AddRegistrationUln)]
        public async Task<IActionResult> AddRegistrationUlnAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.Uln != null)
                return View(cacheModel.Uln);

            var model = new UlnViewModel();
            return View(model);
        }

        [HttpPost]
        [Route("add-registration-unique-learner-number", Name = RouteConstants.AddRegistrationUln)]
        public async Task<IActionResult> AddRegistrationUlnAsync(UlnViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await SyncCacheUln(model);

            var cannotBeRegistered = await _registrationLoader.IsUlnRegisteredAsync(User.GetUkPrn(), model.Uln);
            if (cannotBeRegistered != null && cannotBeRegistered.IsUlnRegisteredAlready)
            {
                TempData[Constants.UlnCanNotBeRegisteredViewModel] = JsonConvert.SerializeObject(cannotBeRegistered);
                return RedirectToRoute(RouteConstants.UlnCanNotBeRegistered);
            }

            return RedirectToRoute(RouteConstants.AddRegistrationLearnersName);
        }

        [HttpGet]
        [Route("ULN-cannot-be-registered", Name = RouteConstants.UlnCanNotBeRegistered)]
        public IActionResult UlnCannotBeRegistered()
        {
            if (TempData[Constants.UlnCanNotBeRegisteredViewModel] == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var model = JsonConvert.DeserializeObject<UlnCannotBeRegisteredViewModel>(TempData[Constants.UlnCanNotBeRegisteredViewModel] as string);
            return View(model);
        }

        [HttpGet]
        [Route("add-registration-learners-name", Name = RouteConstants.AddRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(cacheModel?.LearnersName == null ? new LearnersNameViewModel() : cacheModel.LearnersName);
        }

        [HttpPost]
        [Route("add-registration-learners-name", Name = RouteConstants.SubmitRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync(LearnersNameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.LearnersName = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddRegistrationDateofBirth);
        }

        [HttpGet]
        [Route("add-registration-date-of-birth", Name = RouteConstants.AddRegistrationDateofBirth)]
        public async Task<IActionResult> AddRegistrationDateofBirthAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.LearnersName == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(cacheModel?.DateofBirth == null ? new DateofBirthViewModel() : cacheModel.DateofBirth);
        }

        [HttpPost]
        [Route("add-registration-date-of-birth", Name = RouteConstants.SubmitRegistrationDateofBirth)]
        public async Task<IActionResult> AddRegistrationDateofBirthAsync(DateofBirthViewModel model)
        {
            if (!IsValidDateofBirth(model))
            {
                return View(model);
            }

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.LearnersName == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.DateofBirth = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddRegistrationProvider);
        }

        [HttpGet]
        [Route("add-registration-provider", Name = RouteConstants.AddRegistrationProvider)]
        public async Task<IActionResult> AddRegistrationProviderAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.DateofBirth == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var registeredProviders = await GetAoRegisteredProviders();
            var viewModel = cacheModel?.SelectProvider == null ? new SelectProviderViewModel() : cacheModel.SelectProvider;
            viewModel.ProvidersSelectList = registeredProviders.ProvidersSelectList;
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
                return View(registeredProviderViewModel);

            if (cacheModel?.SelectProvider?.SelectedProviderUkprn != model.SelectedProviderUkprn)
            {
                cacheModel.SelectCore = null;
                cacheModel.SpecialismQuestion = null;
                cacheModel.SelectSpecialism = null;
            }
            
            model.SelectedProviderDisplayName = registeredProviderViewModel?.ProvidersSelectList?.FirstOrDefault(p => p.Value == model.SelectedProviderUkprn)?.Text;
            cacheModel.SelectProvider = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationCore);
        }

        [HttpGet]
        [Route("add-registration-core", Name = RouteConstants.AddRegistrationCore)]
        public async Task<IActionResult> AddRegistrationCoreAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var providerCores = await GetRegisteredProviderCores(cacheModel.SelectProvider.SelectedProviderUkprn.ToLong());
            var viewModel = cacheModel?.SelectCore == null ? new SelectCoreViewModel() : cacheModel.SelectCore;
            viewModel.CoreSelectList = providerCores.CoreSelectList;
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
                return View(coreViewModel);

            if (cacheModel?.SelectCore?.SelectedCoreCode != model.SelectedCoreCode)
            {
                cacheModel.SpecialismQuestion = null;
                cacheModel.SelectSpecialism = null;
            }

            model.SelectedCoreDisplayName = coreViewModel?.CoreSelectList?.FirstOrDefault(p => p.Value == model.SelectedCoreCode)?.Text;
            cacheModel.SelectCore = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationSpecialismQuestion);
        }

        [HttpGet]
        [Route("add-registration-learner-decided-specialism-question", Name = RouteConstants.AddRegistrationSpecialismQuestion)]
        public async Task<IActionResult> AddRegistrationSpecialismQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialismQuestion == null ? new SpecialismQuestionViewModel() : cacheModel.SpecialismQuestion;
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
            return RedirectToRoute(model.HasLearnerDecidedSpecialism.Value ? RouteConstants.AddRegistrationSpecialism : RouteConstants.AddRegistrationAcademicYear);
        }

        [HttpGet]
        [Route("add-registration-specialism", Name = RouteConstants.AddRegistrationSpecialism)]
        public async Task<IActionResult> AddRegistrationSpecialismAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            if (cacheModel?.SelectCore == null || cacheModel?.SpecialismQuestion == null || cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SelectSpecialism == null ? new SelectSpecialismViewModel { PathwaySpecialisms = await GetPathwaySpecialismsByCoreCode(cacheModel.SelectCore.SelectedCoreCode) } : cacheModel.SelectSpecialism;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-specialism", Name = RouteConstants.SubmitRegistrationSpecialism)]
        public async Task<IActionResult> AddRegistrationSpecialismAsync(SelectSpecialismViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel?.SelectCore == null || cacheModel?.SpecialismQuestion == null || cacheModel?.SpecialismQuestion?.HasLearnerDecidedSpecialism == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.SelectSpecialism = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationAcademicYear);
        }

        [HttpGet]
        [Route("add-registration-academic-year", Name = RouteConstants.AddRegistrationAcademicYear)]
        public async Task<IActionResult> AddRegistrationAcademicYearAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            return View();
        }

        [HttpPost]
        [Route("add-registration-academic-year", Name = RouteConstants.SubmitRegistrationAcademicYear)]
        public async Task<IActionResult> AddRegistrationAcademicYearAsync(AcademicYearViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            cacheModel.AcademicYear = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-registration-check-and-submit", Name = RouteConstants.AddRegistrationCheckAndSubmit)]
        public async Task<IActionResult> AddRegistrationCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);

            var viewModel = new CheckAndSubmitViewModel { RegistrationModel = cacheModel };

            if(!viewModel.IsCheckAndSubmitPageValid)
            return RedirectToRoute(RouteConstants.PageNotFound);

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

            return RedirectToRoute(RouteConstants.PageNotFound);
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