﻿using Microsoft.AspNetCore.Authorization;
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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Linq;
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
                    FileDownloadName = ResultContent.UploadUnsuccessful.Result_Error_Report_File_Name
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadResultErrorsFailed, $"Not a valid guid to read file.Method: DownloadResultErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("results-learner-withdrawn/{profileId}", Name = RouteConstants.ResultWithdrawnDetails)]
        public async Task<IActionResult> ResultWithdrawnDetailsAsync(int profileId)
        {
            var viewModel = await _resultLoader.GetResultWithdrawnViewModelAsync(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result withdrawn details found. Method: GetResultWithdrawnViewModelAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Withdrawn}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("learner-results/{profileId}", Name = RouteConstants.ResultDetails)]
        public async Task<IActionResult> ResultDetailsAsync(int profileId)
        {
            var viewModel = await _resultLoader.GetResultDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result details found. Method: GetResultDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Active}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("select-core-result/{profileId}/{assessmentId}", Name = RouteConstants.AddCoreResult)]
        public async Task<IActionResult> AddCoreResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageCoreResultAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: false);

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
            if (!ModelState.IsValid)
            {
                var resultsViewModel = await _resultLoader.GetManageCoreResultAsync(User.GetUkPrn(), model.ProfileId, model.AssessmentId, isChangeMode: false);
                return View(resultsViewModel);
            }

            var response = await _resultLoader.AddCoreResultAsync(User.GetUkPrn(), model);

            if (response == null || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.ResultDetails, new { model.ProfileId });
        }

        [HttpGet]
        [Route("change-core-result/{profileId}/{assessmentId}", Name = RouteConstants.ChangeCoreResult)]
        public async Task<IActionResult> ChangeCoreResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageCoreResultAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: true);

            if (viewModel == null || !viewModel.IsValid)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetManageCoreResultViewModelAsync({User.GetUkPrn()}, {profileId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-core-result/{profileId}/{assessmentId}", Name = RouteConstants.SubmitChangeCoreResult)]
        public async Task<IActionResult> ChangeCoreResultAsync(ManageCoreResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var resultsViewModel = await _resultLoader.GetManageCoreResultAsync(User.GetUkPrn(), model.ProfileId, model.AssessmentId, isChangeMode: true);
                return View(resultsViewModel);
            }

            var isResultChanged = await _resultLoader.IsCoreResultChangedAsync(User.GetUkPrn(), model);
            if (!isResultChanged.HasValue)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"ChangeCoreResult request data-mismatch. Method:IsCoreResultChanged({User.GetUkPrn()}, {model}), ProfileId: {model.ProfileId}, ResultId: {model.ResultId}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (isResultChanged == false)
                return RedirectToRoute(RouteConstants.ResultDetails, new { profileId = model.ProfileId });

            var response = await _resultLoader.ChangeCoreResultAsync(User.GetUkPrn(), model);

            if (response == null || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.ResultDetails, new { model.ProfileId });
        }

        [HttpGet]
        [Route("select-specialism-result/{profileId}/{assessmentId}", Name = RouteConstants.AddSpecialismResult)]
        public async Task<IActionResult> AddSpecialismResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageSpecialismResultAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: false);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetManageSpecialismResultViewModelAsync({User.GetUkPrn()}, {profileId}, {assessmentId}, {false}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("select-specialism-result/{profileId}/{assessmentId}", Name = RouteConstants.SubmitAddSpecialismResult)]
        public async Task<IActionResult> AddSpecialismResultAsync(ManageSpecialismResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var resultsViewModel = await _resultLoader.GetManageSpecialismResultAsync(User.GetUkPrn(), model.ProfileId, model.AssessmentId, isChangeMode: false);
                return View(resultsViewModel);
            }

            var response = await _resultLoader.AddSpecialismResultAsync(User.GetUkPrn(), model);

            if (response == null || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.ResultDetails, new { model.ProfileId });
        }

        [HttpGet]
        [Route("change-specialism-result/{profileId}/{assessmentId}", Name = RouteConstants.ChangeSpecialismResult)]
        public async Task<IActionResult> ChangeSpecialismResultAsync(int profileId, int assessmentId)
        {
            var viewModel = await _resultLoader.GetManageSpecialismResultAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: true);

            if (viewModel == null || !viewModel.IsValid)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetManageSpecialismResultAsync({User.GetUkPrn()}, {profileId}, {assessmentId}, {true}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-specialism-result/{profileId}/{assessmentId}", Name = RouteConstants.SubmitChangeSpecialismResult)]
        public async Task<IActionResult> ChangeSpecialismResultAsync(ManageSpecialismResultViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var resultsViewModel = await _resultLoader.GetManageSpecialismResultAsync(User.GetUkPrn(), model.ProfileId, model.AssessmentId, isChangeMode: true);
                return View(resultsViewModel);
            }

            var isResultChanged = await _resultLoader.IsSpecialismResultChangedAsync(User.GetUkPrn(), model);
            if (!isResultChanged.HasValue)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"ChangeSpecialismResult request data-mismatch. Method:IsSpecialismResultChanged({User.GetUkPrn()}, {model}), ProfileId: {model.ProfileId}, ResultId: {model.ResultId}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (isResultChanged == false)
                return RedirectToRoute(RouteConstants.ResultDetails, new { profileId = model.ProfileId });

            var response = await _resultLoader.ChangeSpecialismResultAsync(User.GetUkPrn(), model);

            if (response == null || !response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.ResultDetails, new { model.ProfileId });
        }

        [HttpGet]
        [Route("results-generating-download", Name = RouteConstants.ResultsGeneratingDownload)]
        public IActionResult ResultsGeneratingDownload()
        {
            return View();
        }

        [HttpPost]
        [Route("results-generating-download", Name = RouteConstants.SubmitResultsGeneratingDownload)]
        public async Task<IActionResult> SubmitResultsGeneratingDownloadAsync()
        {
            var exportResponse = await _resultLoader.GenerateResultsExportAsync(User.GetUkPrn(), User.GetUserEmail());
            if (exportResponse == null || exportResponse.Any(r => r.ComponentType == ComponentType.NotSpecified))
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (exportResponse.All(x => !x.IsDataFound))
            {
                _logger.LogWarning(LogEvent.NoDataFound,
                    $"There are no results entries found for the Data export. Method: GenerateResultsExportAsync({User.GetUkPrn()}, {User.GetUserEmail()})");

                return RedirectToRoute(RouteConstants.ResultsNoRecordsFound);
            }

            var resultsDownloadViewModel = new ResultsDownloadViewModel();

            foreach (var response in exportResponse.Where(r => r.IsDataFound))
            {
                var downloadViewModel = new DownloadLinkViewModel
                {
                    BlobUniqueReference = response.BlobUniqueReference,
                    FileSize = response.FileSize,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                };

                switch (response.ComponentType)
                {
                    case ComponentType.Core:
                        resultsDownloadViewModel.CoreResultsDownloadLinkViewModel = downloadViewModel;
                        break;
                    case ComponentType.Specialism:
                        resultsDownloadViewModel.SpecialismResultsDownloadLinkViewModel = downloadViewModel;
                        break;
                }
            }

            await _cacheService.SetAsync(CacheKey, resultsDownloadViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ResultsDownloadData);
        }

        [HttpGet]
        [Route("results-download-data", Name = RouteConstants.ResultsDownloadData)]
        public async Task<IActionResult> ResultsDownloadDataAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<ResultsDownloadViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read DataExportResponse from redis cache in results download page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("download-results-data/{id}/{componentType}", Name = RouteConstants.ResultsDownloadDataLink)]
        public async Task<IActionResult> ResultsDownloadDataLinkAsync(string id, ComponentType componentType)
        {
            if (id.IsGuid())
            {
                var fileStream = await _resultLoader.GetResultsDataFileAsync(User.GetUkPrn(), id.ToGuid(), componentType);
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download result data. Method: GetResultsDataFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id}, componentType = {componentType})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = componentType == ComponentType.Core
                                       ? ResultContent.ResultsDownloadData.Core_Results_Download_FileName
                                       : ResultContent.ResultsDownloadData.Specialism_Results_Download_FileName
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DocumentDownloadFailed, $"Not a valid guid to read file.Method: ResultsDownloadDataLinkAsync(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("results-no-records-found", Name = RouteConstants.ResultsNoRecordsFound)]
        public IActionResult ResultsNoRecordsFound()
        {
            return View(new ResultsNoRecordsFoundViewModel());
        }
    }
}