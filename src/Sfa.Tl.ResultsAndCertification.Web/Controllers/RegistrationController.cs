using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
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
        [Route("add-registration-unique-learner", Name= RouteConstants.AddRegistration)]
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
            return RedirectToRoute(RouteConstants.AddRegistrationLearnersName);
        }

        [HttpGet]
        [Route("add-registration-learners-name", Name = RouteConstants.AddRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync()
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            
            if(cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(cacheModel?.LearnersName == null ? new LearnersNameViewModel() : cacheModel.LearnersName);
        }

        [HttpPost]
        [Route("add-registration-learners-name", Name = RouteConstants.SubmitRegistrationLearnersName)]
        public async Task<IActionResult> AddRegistrationLearnersNameAsync(LearnersNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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
            if (!IsValidDateOfBirth(model))
            {
                return View(model);
            }

            model.DateofBirth = DateTime.UtcNow;

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel == null)
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
            if (!ModelState.IsValid)
            {
                model = await GetAoRegisteredProviders();
                return View(model);
            }

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

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

            var providerCores = await GetRegisteredProviderCores(cacheModel.SelectProvider.SelectedProviderId.ToLong());
            var viewModel = cacheModel?.SelectCore == null ? new SelectCoreViewModel() : cacheModel.SelectCore;
            viewModel.CoreSelectList = providerCores.CoreSelectList;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-registration-core", Name = RouteConstants.SubmitRegistrationCore)]
        public async Task<IActionResult> AddRegistrationCoreAsync(SelectCoreViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel == null || cacheModel?.SelectProvider == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
            {
                model = await GetRegisteredProviderCores(cacheModel.SelectProvider.SelectedProviderId.ToLong());
                return View(model);
            }

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
            {
                return View(model);
            }

            var cacheModel = await _cacheService.GetAsync<RegistrationViewModel>(CacheKey);
            if (cacheModel == null || cacheModel?.SelectCore == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.SpecialismQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.AddRegistrationSpecialismQuestion);
        }

        private async Task<SelectProviderViewModel> GetAoRegisteredProviders()
        {
            return await _registrationLoader.GetRegisteredTqAoProviderDetailsAsync(User.GetUkPrn());
        }
        private async Task<SelectCoreViewModel> GetRegisteredProviderCores(long providerUkprn)
        {
            return await _registrationLoader.GetRegisteredProviderCoreDetailsAsync(User.GetUkPrn(), providerUkprn);
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

        private bool IsValidDateOfBirth(DateofBirthViewModel model)
        {
            // TODO:
            return true;
        }
    }
}