using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Threading.Tasks;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentLoader _assessmentLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AssessmentCacheKey); }
        }

        public AssessmentController(IAssessmentLoader assessmentLoader, ICacheService cacheService, ILogger<AssessmentController> logger)
        {
            _assessmentLoader = assessmentLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("assessment-entries", Name = RouteConstants.AssessmentDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-assessment-entries-file/{requestErrorTypeId:int?}", Name = RouteConstants.UploadAssessmentsFile)]
        public IActionResult UploadAssessmentsFile(int? requestErrorTypeId)
        {
            var model = new UploadAssessmentsRequestViewModel { RequestErrorTypeId = requestErrorTypeId };
            model.SetAnyModelErrors(ModelState);
            return View(model);
        }

        [HttpPost]
        [Route("upload-assessment-entries-file", Name = RouteConstants.SubmitUploadAssessmentsFile)]
        public async Task<IActionResult> UploadAssessmentsFileAsync(UploadAssessmentsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _assessmentLoader.ProcessBulkAssessmentsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AssessmentsUploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AssessmentsUploadSuccessful);
            }

            if (response.ShowProblemWithServicePage)
                return RedirectToRoute(RouteConstants.ProblemWithAssessmentsUpload);

            var unsuccessfulViewModel = new UploadUnsuccessfulViewModel 
            { 
                BlobUniqueReference = response.BlobUniqueReference, 
                FileSize = response.ErrorFileSize, 
                FileType = FileType.Csv.ToString().ToUpperInvariant() 
            };

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel), unsuccessfulViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.AssessmentsUploadUnsuccessful);
        }

        [HttpGet]
        [Route("assessment-entries-upload-confirmation", Name = RouteConstants.AssessmentsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(string.Concat(CacheKey, Constants.AssessmentsUploadSuccessfulViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed, $"Unable to read upload successful assessment response from redis cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("assessment-entries-upload-unsuccessful", Name = RouteConstants.AssessmentsUploadUnsuccessful)]
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
        [Route("assessment-entries-file-upload-service-problem", Name = RouteConstants.ProblemWithAssessmentsUpload)]
        public IActionResult ProblemWithAssessmentsUpload()
        {
            return View();
        }

        [HttpGet]
        [Route("download-assessment-errors", Name = RouteConstants.DownloadAssessmentErrors)]
        public async Task<IActionResult> DownloadAssessmentErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _assessmentLoader.GetAssessmentValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download assessment validation errors. Method: GetAssessmentValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = AssessmentContent.UploadUnsuccessful.Assessment_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadAssesssmentErrorsFailed, $"Not a valid guid to read file.Method: DownloadAssessmentErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("assessment-entries-learner-search/{populateUln:bool?}", Name = RouteConstants.SearchAssessments)]
        public async Task<IActionResult> SearchAssessmentsAsync(bool populateUln)
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(Constants.AssessmentsSearchCriteria);
            var viewModel = new SearchAssessmentsViewModel { SearchUln = populateUln ? defaultValue : null };
            return View(viewModel);
        }

        [HttpPost]
        [Route("assessment-entries-learner-search", Name = RouteConstants.SubmitSearchAssessments)]
        public async Task<IActionResult> SearchAssessmentsAsync(SearchAssessmentsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var searchResult = await _assessmentLoader.FindUlnAssessmentsAsync(User.GetUkPrn(), model.SearchUln.ToLong());

            if (searchResult?.IsAllowed == true)
            {
                if(searchResult.IsWithdrawn)
                    await _cacheService.SetAsync(Constants.AssessmentsSearchCriteria, model.SearchUln);

                return RedirectToRoute(searchResult.IsWithdrawn ? RouteConstants.AssessmentWithdrawnDetails : RouteConstants.AssessmentDetails, new { profileId = searchResult.RegistrationProfileId });
            }
            else
            {
                await _cacheService.SetAsync(Constants.AssessmentsSearchCriteria, model.SearchUln);

                var ulnAssessmentsNotfoundModel = new UlnAssessmentsNotFoundViewModel { Uln = model.SearchUln.ToString() };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.SearchAssessmentsUlnNotFound), ulnAssessmentsNotfoundModel, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.SearchAssessmentsNotFound);
            }
        }

        [HttpGet]
        [Route("assessment-entries-uln-not-found", Name = RouteConstants.SearchAssessmentsNotFound)]
        public async Task<IActionResult> SearchAssessmentsNotFoundAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UlnAssessmentsNotFoundViewModel>(string.Concat(CacheKey, Constants.SearchAssessmentsUlnNotFound));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read SearchAssessmentsUlnNotFound from redis cache in search assessments not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("assessment-entries-learner-withdrawn/{profileId}", Name = RouteConstants.AssessmentWithdrawnDetails)]
        public async Task<IActionResult> AssessmentWithdrawnDetailsAsync(int profileId)
        {
            var viewModel = await _assessmentLoader.GetAssessmentDetailsAsync<AssessmentUlnWithdrawnViewModel>(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No assessment withdrawn details found. Method: GetAssessmentDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Withdrawn}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("assessment-entry-learner-details/{profileId}", Name = RouteConstants.AssessmentDetails)]
        public async Task<IActionResult> AssessmentDetailsAsync(int profileId)
        {
            var viewModel = await _assessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No assessment details found. Method: GetAssessmentDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Active}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("assessment-entry-add-core/{profileId}", Name = RouteConstants.AddCoreAssessmentEntry)]
        public async Task<IActionResult> AddCoreAssessmentEntryAsync(int profileId)
        {
            var viewModel = await _assessmentLoader.GetAddAssessmentEntryAsync<AddAssessmentEntryViewModel>(User.GetUkPrn(), profileId, ComponentType.Core, null);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No assessment series available or Learner not found. Method: GetAddAssessmentEntryAsync({User.GetUkPrn()}, {profileId}, {ComponentType.Core}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("assessment-entry-add-core/{profileId}", Name = RouteConstants.EntrySeries)]
        public async Task<IActionResult> AddCoreAssessmentEntryAsync(AddAssessmentEntryViewModel model)
        {
            var assessmentEntryDetails = await _assessmentLoader.GetAddAssessmentEntryAsync<AddAssessmentEntryViewModel>(User.GetUkPrn(), model.ProfileId, ComponentType.Core, null);
            if (!ModelState.IsValid)
                return View(assessmentEntryDetails);

            if (!model.IsOpted.Value)
                return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });

            model.ComponentType = ComponentType.Core;
            var response = await _assessmentLoader.AddAssessmentEntryAsync(User.GetUkPrn(), model);

            if (!response.IsSuccess)
            {
                _logger.LogWarning(LogEvent.AddCoreAssessmentEntryFailed, $"Unable to add core assessment for ProfileId: {model.ProfileId}. Method: AddAssessmentEntryAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }

            var notificationBanner = new NotificationBannerModel { Message = assessmentEntryDetails.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });
        }        

        [HttpGet]
        [Route("assessment-entry-remove-core/{assessmentId}", Name = RouteConstants.RemoveCoreAssessmentEntry)]
        public async Task<IActionResult> RemoveCoreAssessmentEntryAsync(int assessmentId)
        {
            var viewModel = await _assessmentLoader.GetActiveAssessmentEntryDetailsAsync(User.GetUkPrn(), assessmentId, ComponentType.Core);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No valid assessment entry available. Method: GetActiveAssessmentEntryDetailsAsync({User.GetUkPrn()}, {assessmentId}, {ComponentType.Core}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("assessment-entry-remove-core/{assessmentId}", Name = RouteConstants.SubmitRemoveCoreAssessmentEntry)]
        public async Task<IActionResult> RemoveCoreAssessmentEntryAsync(AssessmentEntryDetailsViewModel model)
        {
            var assessmentEntryDetails = await _assessmentLoader.GetActiveAssessmentEntryDetailsAsync(User.GetUkPrn(), model.AssessmentId, ComponentType.Core);
            if (!ModelState.IsValid)
                return View(assessmentEntryDetails);

            if (!model.CanRemoveAssessmentEntry.Value)
                return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });

            model.ComponentType = ComponentType.Core;
            var isSuccess = await _assessmentLoader.RemoveAssessmentEntryAsync(User.GetUkPrn(), model);

            if (!isSuccess)
            {
                _logger.LogWarning(LogEvent.AddCoreAssessmentEntryFailed, $"Unable to remove core assessment for ProfileId: {model.ProfileId} and AssessmentId: {model.AssessmentId}. Method: RemoveAssessmentEntryAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }

            var notificationBanner = new NotificationBannerModel { Message = assessmentEntryDetails.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });
        }

        [HttpGet]
        [Route("assessment-entry-add-specialisms/{profileId}/{specialismLarId}", Name = RouteConstants.AddSpecialismAssessmentEntry)]
        public async Task<IActionResult> AddSpecialismAssessmentEntryAsync(int profileId, string specialismLarId)
        {
            var viewModel = await _assessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(User.GetUkPrn(), profileId, ComponentType.Specialism, specialismLarId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No assessment series available for specialisms or Learner not found. Method: GetAddAssessmentEntryAsync({User.GetUkPrn()}, {profileId}, {ComponentType.Specialism}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SpecialismLarId = specialismLarId;

            if (!viewModel.IsValidToAdd)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Not a valid specialism to add assessment entry. Method: GetAddAssessmentEntryAsync({User.GetUkPrn()}, {profileId}, {ComponentType.Specialism}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("assessment-entry-add-specialisms/{profileId}/{specialismId}", Name = RouteConstants.SubmitAddSpecialismAssessmentEntry)]
        public async Task<IActionResult> AddSpecialismAssessmentEntryAsync(AddSpecialismAssessmentEntryViewModel model)
        {
            var assessmentEntryDetails = await _assessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(User.GetUkPrn(), model.ProfileId, ComponentType.Specialism, model.SpecialismLarId);
            assessmentEntryDetails.SpecialismLarId = model.SpecialismLarId;

            if (!ModelState.IsValid)
                return View(assessmentEntryDetails);

            if (!model.IsOpted.Value)
                return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });

            if (!assessmentEntryDetails.IsValidToAdd)
            {
                _logger.LogWarning(LogEvent.NotValidData, $"Not a valid specialism to add assessment entry. Method: AddSpecialismAssessmentEntryAsync({User.GetUkPrn()}, {model.ProfileId}, {ComponentType.Specialism}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var response = await _assessmentLoader.AddSpecialismAssessmentEntryAsync(User.GetUkPrn(), assessmentEntryDetails);

            if (!response.IsSuccess)
            {
                _logger.LogWarning(LogEvent.AddSpecialismAssessmentEntryFailed, $"Unable to add specialism assessment entry for ProfileId: {model.ProfileId}. Method: AddSpecialismAssessmentEntryAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }

            var notificationBanner = new NotificationBannerModel { Message = assessmentEntryDetails.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.AssessmentDetails, new { model.ProfileId });
        }
    }
}
