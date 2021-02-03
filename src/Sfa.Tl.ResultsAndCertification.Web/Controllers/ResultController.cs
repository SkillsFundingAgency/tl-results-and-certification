using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Threading.Tasks;
using ResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireResultsEditorAccess)]
    public class ResultController : Controller
    {
        private readonly IResultLoader _resultLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.ResultCacheKey); }
        }

        public ResultController(IResultLoader resultLoader, ICacheService cacheService, ILogger<ResultController> logger)
        {
            _resultLoader = resultLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("results", Name = RouteConstants.ResultsDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-results-file/{requestErrorTypeId:int?}", Name = RouteConstants.UploadResultsFile)]
        public IActionResult UploadResultsFile(int? requestErrorTypeId)
        {
            var model = new UploadResultsRequestViewModel { RequestErrorTypeId = requestErrorTypeId };
            model.SetAnyModelErrors(ModelState);
            return View(model);
        }

        [HttpPost]
        [Route("upload-results-file", Name = RouteConstants.SubmitUploadResultsFile)]
        public async Task<IActionResult> UploadResultsFileAsync(UploadResultsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _resultLoader.ProcessBulkResultsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ResultsUploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.ResultsUploadSuccessful);
            }            

            if (response.ShowProblemWithServicePage)
                return RedirectToRoute(RouteConstants.ProblemWithResultsUpload);

            var unsuccessfulViewModel = new UploadUnsuccessfulViewModel
            {
                BlobUniqueReference = response.BlobUniqueReference,
                FileSize = response.ErrorFileSize,
                FileType = FileType.Csv.ToString().ToUpperInvariant()
            };

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel), unsuccessfulViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ResultsUploadUnsuccessful);
        }

        [HttpGet]
        [Route("results-upload-confirmation", Name = RouteConstants.ResultsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(string.Concat(CacheKey, Constants.ResultsUploadSuccessfulViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed, $"Unable to read upload successful result response from redis cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("results-upload-unsuccessful", Name = RouteConstants.ResultsUploadUnsuccessful)]
        public async Task<IActionResult> UploadUnsuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful results response from redis cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("results-file-upload-service-problem", Name = RouteConstants.ProblemWithResultsUpload)]
        public IActionResult ProblemWithResultsUpload()
        {
            return View();
        }

        [HttpGet]
        [Route("download-result-errors", Name = RouteConstants.DownloadResultErrors)]
        public async Task<IActionResult> DownloadResultErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _resultLoader.GetResultValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download result validation errors. Method: GetResultValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = ResultContent.UploadUnsuccessful.Result_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadResultErrorsFailed, $"Not a valid guid to read file.Method: DownloadResultErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("results-learner-search", Name = RouteConstants.SearchResults)]
        public async Task<IActionResult> SearchResultsAsync()
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(Constants.ResultsSearchCriteria);
            var viewModel = new SearchResultsViewModel { SearchUln = defaultValue };
            return View(viewModel);
        }

        [HttpPost]
        [Route("results-learner-search", Name = RouteConstants.SubmitSearchResults)]
        public async Task<IActionResult> SearchResultsAsync(SearchResultsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var searchResult = await _resultLoader.FindUlnResultsAsync(User.GetUkPrn(), model.SearchUln.ToLong());

            if (searchResult?.IsAllowed == true)
            {
                return RedirectToRoute(searchResult.IsWithdrawn ? RouteConstants.ResultWithdrawnDetails : RouteConstants.ResultDetails, new { profileId = searchResult.RegistrationProfileId });
            }
            else
            {
                await _cacheService.SetAsync(Constants.ResultsSearchCriteria, model.SearchUln);

                var ulnResultsNotfoundModel = new UlnResultsNotFoundViewModel { Uln = model.SearchUln.ToString() };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.SearchResultsUlnNotFound), ulnResultsNotfoundModel, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.SearchResultsNotFound);
            }
        }

        [HttpGet]
        [Route("search-for-learner-results-ULN-not-found", Name = RouteConstants.SearchResultsNotFound)]
        public async Task<IActionResult> SearchResultsNotFoundAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UlnResultsNotFoundViewModel>(string.Concat(CacheKey, Constants.SearchResultsUlnNotFound));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read SearchResultsUlnNotFound from redis cache in search results not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("learners-results-withdrawn-learner/{profileId}", Name = RouteConstants.ResultWithdrawnDetails)]
        public async Task<IActionResult> ResultWithdrawnDetailsAsync(int profileId)
        {
            var viewModel = await _resultLoader.GetResultDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Withdrawn);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result withdrawn details found. Method: GetResultDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Withdrawn}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("learners-results/{profileId}", Name = RouteConstants.ResultDetails)]
        public async Task<IActionResult> ResultDetailsAsync(int profileId)
        {
            var viewModel = await _resultLoader.GetResultDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result details found. Method: GetResultDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Active}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("select-core-result/{profileId}/{assessmentId}", Name = RouteConstants.AddCoreResult)]
        public async Task<IActionResult> AddCoreResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageCoreResultViewModelAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: false);
            
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetAddCoreResultViewModelAsync({User.GetUkPrn()}, {profileId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }            

            return View(viewModel);
        }

        [HttpPost]
        [Route("select-core-result", Name = RouteConstants.SubmitAddCoreResult)]
        public async Task<IActionResult> AddCoreResultAsync(ManageCoreResultViewModel model)
        {
            if (string.IsNullOrEmpty(model?.SelectedGradeCode))
                return RedirectToRoute(RouteConstants.ResultDetails, new { profileId = model.ProfileId });

            var response = await _resultLoader.AddCoreResultAsync(User.GetUkPrn(), model);

            if (response == null || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ResultConfirmationViewModel), new ResultConfirmationViewModel { Uln = response.Uln, ProfileId = response.ProfileId }, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.AddResultConfirmation);
        }

        [HttpGet]
        [Route("result-added-confirmation", Name = RouteConstants.AddResultConfirmation)]
        public async Task<IActionResult> AddResultConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ResultConfirmationViewModel>(string.Concat(CacheKey, Constants.ResultConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read ResultConfirmationViewModel from redis cache in add result confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("change-core-result/{profileId}/{assessmentId}", Name = RouteConstants.ChangeCoreResult)]
        public async Task<IActionResult> ChangeCoreResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageCoreResultViewModelAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: true);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetAddCoreResultViewModelAsync({User.GetUkPrn()}, {profileId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }
    }
}