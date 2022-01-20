﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual.ResultDetailsViewModelNew;
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
        [Route("results-learner-search/{populateUln:bool?}", Name = RouteConstants.SearchResults)]
        public async Task<IActionResult> SearchResultsAsync(bool populateUln)
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(Constants.ResultsSearchCriteria);
            var viewModel = new SearchResultsViewModel { SearchUln = !string.IsNullOrWhiteSpace(defaultValue) && populateUln ? defaultValue : null };

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
                if (searchResult.IsWithdrawn)
                {
                    await _cacheService.SetAsync(Constants.ResultsSearchCriteria, model.SearchUln);
                    return RedirectToRoute(RouteConstants.ResultWithdrawnDetails, new { profileId = searchResult.RegistrationProfileId });
                }

                return RedirectToRoute(RouteConstants.ResultDetails, new { profileId = searchResult.RegistrationProfileId });
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
            var viewModel = await _resultLoader.GetResultWithdrawnViewModelAsync(User.GetUkPrn(), profileId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result withdrawn details found. Method: GetResultWithdrawnViewModelAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Withdrawn}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("no-assessment-entries", Name = RouteConstants.ResultNoAssessmentEntry)]
        public async Task<IActionResult> ResultNoAssessmentEntryAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ResultNoAssessmentEntryViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read ResultNoAssessmentEntryViewModel from redis cache in assessment no assessment entries page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
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

            if (!viewModel.IsPathwayAssessmentEntryRegistered)
            {
                await _cacheService.SetAsync(Constants.ResultsSearchCriteria, viewModel.Uln.ToString());
                await _cacheService.SetAsync(CacheKey, _resultLoader.GetResultNoAssessmentEntryViewModel(viewModel));
                return RedirectToRoute(RouteConstants.ResultNoAssessmentEntry);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("learners-results-new/{profileId}", Name = "ResDetails")]
        public async Task<IActionResult> ResultDetailsNewAsync(int profileId)
        {
            var viewModel = await _resultLoader.GetResultDetailsAsync(User.GetUkPrn(), profileId, RegistrationPathwayStatus.Active);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No result details found. Method: GetResultDetailsAsync({User.GetUkPrn()}, {profileId}, {RegistrationPathwayStatus.Active}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!viewModel.IsPathwayAssessmentEntryRegistered)
            {
                await _cacheService.SetAsync(Constants.ResultsSearchCriteria, viewModel.Uln.ToString());
                await _cacheService.SetAsync(CacheKey, _resultLoader.GetResultNoAssessmentEntryViewModel(viewModel));
                return RedirectToRoute(RouteConstants.ResultNoAssessmentEntry);
            }

            var vm = new ResultDetailsViewModelNew
            {
                Firstname = "John",
                Lastname = "Smith",
                Uln = 5647382910,
                DateofBirth = System.DateTime.Today,
                ProviderName = "Barnsley College",
                ProviderUkprn = 100656,
                TlevelTitle = "Design, Surveying and Planning for Construction",

                // Core
                CoreComponentDisplayName = "Design, Surveying and Planning (123456)",
                CoreComponentExams = new List<ComponentExamViewModel>
                {
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = null, UpdatedBy = null, AppealEndDate = System.DateTime.Today.AddDays(10) },
                    new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "B", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 3", AppealEndDate = System.DateTime.Today.AddDays(10) },
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2021", Grade = "B", PrsStatus = PrsStatus.BeingAppealed, LastUpdated = "5 June 2021", UpdatedBy = "User 2", AppealEndDate = System.DateTime.Today.AddDays(10) },
                    new ComponentExamViewModel { AssessmentSeries = "Summer 2021", Grade = "A", PrsStatus = PrsStatus.Final, LastUpdated = "4 June 2021", UpdatedBy = "User 1", AppealEndDate = System.DateTime.Today.AddDays(10) },
                    new ComponentExamViewModel { AssessmentSeries = "Autumn 2020", Grade = "D", PrsStatus = null, LastUpdated = "34 June 2021", UpdatedBy = "User 1", AppealEndDate = System.DateTime.Today.AddDays(-365), AssessmentId = 1 }
                },

                // Specialisms
                SpecialismComponents = new List<SpecialismComponentViewModel>
                {
                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Plumbing",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = null, UpdatedBy = null, AppealEndDate = System.DateTime.Today.AddDays(10) },
                            new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 1",AppealEndDate = System.DateTime.Today.AddDays(10), AssessmentId = 1 }
                        }
                    },

                    new SpecialismComponentViewModel
                    {
                        SpecialismComponentDisplayName = "Heating",
                        SpecialismComponentExams = new List<ComponentExamViewModel>
                        {
                            new ComponentExamViewModel { AssessmentSeries = "Autumn 2022", Grade = null, PrsStatus = null, LastUpdated = "7 June 2021", UpdatedBy = "User 2", AppealEndDate = System.DateTime.Today.AddDays(10) },
                            new ComponentExamViewModel { AssessmentSeries = "Summer 2022", Grade = "Merit", PrsStatus = null, LastUpdated = "6 June 2021", UpdatedBy = "User 1", AppealEndDate = System.DateTime.Today.AddDays(10) }
                        }
                    }
                }
            };

            return View(vm);
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
            if (string.IsNullOrWhiteSpace(model?.SelectedGradeCode))
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
            var viewModel = await _resultLoader.GetManageCoreResultAsync(User.GetUkPrn(), profileId, assessmentId, isChangeMode: true);

            if (viewModel == null || !viewModel.IsValid)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No details found. Method: GetManageCoreResultViewModelAsync({User.GetUkPrn()}, {profileId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("change-core-result", Name = RouteConstants.SubmitChangeCoreResult)]
        public async Task<IActionResult> ChangeCoreResultAsync(ManageCoreResultViewModel model)
        {
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

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.ChangeResultConfirmationViewModel), new ResultConfirmationViewModel { Uln = response.Uln, ProfileId = response.ProfileId }, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.ChangeResultConfirmation);
        }

        [HttpGet]
        [Route("result-change-confirmation", Name = RouteConstants.ChangeResultConfirmation)]
        public async Task<IActionResult> ChangeResultConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ResultConfirmationViewModel>(string.Concat(CacheKey, Constants.ChangeResultConfirmationViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read ResultConfirmationViewModel from redis cache in change result confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
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