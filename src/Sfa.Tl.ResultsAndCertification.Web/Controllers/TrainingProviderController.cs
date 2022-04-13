using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class TrainingProviderController : Controller
    {
        private readonly ITrainingProviderLoader _trainingProviderLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TrainingProviderCacheKey); } }

        public TrainingProviderController(ITrainingProviderLoader trainingProviderLoader, ICacheService cacheService, ILogger<TrainingProviderController> logger)
        {
            _trainingProviderLoader = trainingProviderLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("add-learner-record-ULN-already-added/{profileId}", Name = RouteConstants.EnterUniqueLearnerNumberAddedAlready)]
        public async Task<IActionResult> EnterUniqueLearnerNumberAddedAlreadyAsync(int profileId)
        {
            // TODO: Delete
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddLearnerRecordViewModel from redis cache in Uln already added page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(new LearnerRecordAddedAlreadyViewModel { ProfileId = profileId, Uln = cacheModel.Uln?.EnterUln?.ToString(), LearnerName = cacheModel?.LearnerRecord?.Name });
        }

        [HttpGet]
        [Route("add-learner-record-ULN-not-registered", Name = RouteConstants.EnterUniqueLearnerNumberNotFound)]
        public async Task<IActionResult> EnterUniqueLearnerNumberNotFoundAsync()
        {
            // TODO: Delete
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddLearnerRecordViewModel from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(new LearnerRecordNotFoundViewModel { Uln = cacheModel.Uln?.EnterUln?.ToString() });
        }

        [HttpGet]
        [Route("add-learner-record-english-and-maths-achievement/{isChangeMode:bool?}", Name = RouteConstants.AddEnglishAndMathsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsQuestionAsync(bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false || cacheModel?.LearnerRecord?.HasLrsEnglishAndMaths == true)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.EnglishAndMathsQuestion == null ? new EnglishAndMathsQuestionViewModel() : cacheModel.EnglishAndMathsQuestion;
            viewModel.LearnerName = cacheModel.LearnerRecord.Name;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-english-and-maths-achievement", Name = RouteConstants.SubmitAddEnglishAndMathsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsQuestionAsync(EnglishAndMathsQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.EnglishAndMathsQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(model.IsChangeMode ? RouteConstants.AddLearnerRecordCheckAndSubmit : RouteConstants.AddIndustryPlacementQuestion);
        }

        [HttpGet]
        [Route("add-learner-record-english-and-maths-achievement-lrs", Name = RouteConstants.AddEnglishAndMathsLrsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsLrsQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false || cacheModel?.LearnerRecord?.HasLrsEnglishAndMaths == false || cacheModel?.LearnerRecord?.IsSendConfirmationRequired == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.EnglishAndMathsLrsQuestion == null ? new EnglishAndMathsLrsQuestionViewModel() : cacheModel.EnglishAndMathsLrsQuestion;
            viewModel.LearnerName = cacheModel.LearnerRecord.Name;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-english-and-maths-achievement-lrs", Name = RouteConstants.SubmitAddEnglishAndMathsLrsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsLrsQuestionAsync(EnglishAndMathsLrsQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.EnglishAndMathsLrsQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            var response = await _trainingProviderLoader.AddLearnerRecordAsync(User.GetUkPrn(), cacheModel);

            if (response.IsSuccess)
            {
                if (cacheModel.Uln.IsNavigatedFromSearchLearnerRecordNotAdded)
                    await _cacheService.RemoveAsync<SearchLearnerRecordViewModel>(CacheKey);

                await _cacheService.RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AddEnglishAndMathsSendDataConfirmation), new LearnerRecordConfirmationViewModel { Uln = response.Uln, Name = response.Name }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AddEnglishAndMathsSendDataConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.AddEnglishAndMathsSendDataEmailFailed, $"Unable to send email for English and maths send data for UniqueLearnerNumber: {cacheModel.Uln}. Method: AddEnglishAndMathsLrsQuestionAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("add-learner-record-english-and-maths-data-confirmation", Name = RouteConstants.AddEnglishAndMathsSendDataConfirmation)]
        public async Task<IActionResult> AddEnglishAndMathsSendDataConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<LearnerRecordConfirmationViewModel>(string.Concat(CacheKey, Constants.AddEnglishAndMathsSendDataConfirmation));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read LearnerRecordConfirmationViewModel from redis cache in add english and maths send data confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("has-learner-completed-industry-placement/{isChangeMode:bool?}", Name = RouteConstants.AddIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync(bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false ||
                (cacheModel?.LearnerRecord?.HasLrsEnglishAndMaths == false && cacheModel?.EnglishAndMathsQuestion == null))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.IndustryPlacementQuestion == null ? new IndustryPlacementQuestionViewModel() : cacheModel.IndustryPlacementQuestion;
            viewModel.LearnerName = cacheModel.LearnerRecord.Name;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;

            if (cacheModel?.LearnerRecord.HasLrsEnglishAndMaths == true)
                viewModel.IsBackLinkToEnterUln = true;

            return View(viewModel);
        }

        [HttpPost]
        [Route("has-learner-completed-industry-placement", Name = RouteConstants.SubmitIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync(IndustryPlacementQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.IndustryPlacementQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-learner-record-check-and-submit", Name = RouteConstants.AddLearnerRecordCheckAndSubmit)]
        public async Task<IActionResult> AddLearnerRecordCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            var viewModel = new CheckAndSubmitViewModel { LearnerRecordModel = cacheModel };

            if (!viewModel.IsCheckAndSubmitPageValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            await _cacheService.SetAsync(CacheKey, viewModel.ResetChangeMode());
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-check-and-submit", Name = RouteConstants.SubmitLearnerRecordCheckAndSubmit)]
        public async Task<IActionResult> SubmitLearnerRecordCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var response = await _trainingProviderLoader.AddLearnerRecordAsync(User.GetUkPrn(), cacheModel);

            if (response.IsSuccess)
            {
                if (cacheModel.Uln.IsNavigatedFromSearchLearnerRecordNotAdded)
                    await _cacheService.RemoveAsync<SearchLearnerRecordViewModel>(CacheKey);

                await _cacheService.RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AddLearnerRecordConfirmation), new LearnerRecordConfirmationViewModel { Uln = response.Uln, Name = response.Name }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.LearnerRecordAddedConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.AddLearnerRecordFailed, $"Unable to add learner record for UniqueLearnerNumber: {cacheModel.Uln}. Method: SubmitLearnerRecordCheckAndSubmitAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("learner-record-added-confirmation", Name = RouteConstants.LearnerRecordAddedConfirmation)]
        public async Task<IActionResult> AddLearnerRecordConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<LearnerRecordConfirmationViewModel>(string.Concat(CacheKey, Constants.AddLearnerRecordConfirmation));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read LearnerRecordConfirmationViewModel from redis cache in add learner record confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("add-learner-record-cancel", Name = RouteConstants.AddLearnerRecordCancel)]
        public async Task<IActionResult> AddLearnerRecordCancelAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new LearnerRecordCancelViewModel { LearnerName = cacheModel.LearnerRecord.Name, CancelLearnerRecord = true };
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-cancel", Name = RouteConstants.SubmitLearnerRecordCancel)]
        public async Task<IActionResult> AddLearnerRecordCancelAsync(LearnerRecordCancelViewModel viewModel)
        {
            if (viewModel.CancelLearnerRecord)
            {
                await _cacheService.RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.ManageLearnerRecordsDashboard);
            }
            else
                return RedirectToRoute(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }

        #region Update-Learner

        // TODO: Remove
        [HttpGet]
        [Route("update-learner-record", Name = RouteConstants.UpdateLearnerRecord)]
        public async Task<IActionResult> UpdateLearnerRecordAsync()
        {
            await _cacheService.RemoveAsync<SearchLearnerRecordViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.SearchLearnerRecord);
        }

        [HttpGet]
        [Route("search-learner-record-unique-learner-number", Name = RouteConstants.SearchLearnerRecord)]
        public async Task<IActionResult> SearchLearnerRecordAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<SearchLearnerRecordViewModel>(CacheKey);
            var viewModel = cacheModel ?? new SearchLearnerRecordViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("search-learner-record-unique-learner-number", Name = RouteConstants.SubmitSearchLearnerRecord)]
        public async Task<IActionResult> SearchLearnerRecordAsync(SearchLearnerRecordViewModel model)
        {
            // Note: Please note this is intrim search page, we have another stories coming up will replace this method. 
            if (!ModelState.IsValid)
                return View(model);

            var learnerRecord = await _trainingProviderLoader.FindLearnerRecordAsync(User.GetUkPrn(), model.SearchUln.ToLong());
            if (learnerRecord == null || !learnerRecord.IsLearnerRegistered)
            {
                model.IsLearnerRegistered = learnerRecord?.IsLearnerRegistered ?? false;

                await _cacheService.SetAsync(CacheKey, model);
                return RedirectToRoute(RouteConstants.SearchLearnerRecordNotFound);
            }
            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = learnerRecord.ProfileId });
        }

        [HttpGet]
        [Route("search-learner-record-ULN-not-registered", Name = RouteConstants.SearchLearnerRecordNotFound)]
        public async Task<IActionResult> SearchLearnerRecordNotFoundAsync()
        {
            var cacheModel = await _cacheService.GetAsync<SearchLearnerRecordViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IsLearnerRegistered)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read SearchLearnerRecordViewModel from redis cache or IsLearnerRegistered is true in search learner record not registered page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(new SearchLearnerRecordNotFoundViewModel { Uln = cacheModel.SearchUln?.ToString() });
        }

        [HttpGet]
        [Route("search-learner-record-ULN-not-added", Name = RouteConstants.SearchLearnerRecordNotAdded)]
        public async Task<IActionResult> SearchLearnerRecordNotAddedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<SearchLearnerRecordViewModel>(CacheKey);

            if (cacheModel == null || cacheModel.IsLearnerRecordAdded)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read SearchLearnerRecordViewModel from redis cache or IsLearnerRecord already added in search learner record not added page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(new LearnerRecordNotAddedViewModel { Uln = cacheModel.SearchUln.ToString() });
        }

        [HttpGet]
        [Route("learner-record-page/{profileId}", Name = RouteConstants.LearnerRecordDetails)]
        public async Task<IActionResult> LearnerRecordDetailsAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsLearnerRegistered) // TODO: check if IsLearnerRegistered can be deleted as well?
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner is not registerd or learner record not added. Method: LearnerRecordDetailsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("query-english-and-maths-status/{profileId}", Name = RouteConstants.QueryEnglishAndMathsStatus)]
        public async Task<IActionResult> QueryEnglishAndMathsStatusAsync(int profileId)
        {
            var learnerDetails = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(User.GetUkPrn(), profileId);
            if (learnerDetails == null || !learnerDetails.HasLrsEnglishAndMaths)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner details are found or no LRS data available. Method: GetLearnerRecordDetailsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = new QueryEnglishAndMathsViewModel { ProfileId = learnerDetails.ProfileId, Name = learnerDetails.Name };
            return View(viewModel);
        }

        [HttpGet]
        [Route("update-learner-record-industry-placement-status/{profileId}/{pathwayId}", Name = RouteConstants.UpdateIndustryPlacementQuestion)]
        public async Task<IActionResult> UpdateIndustryPlacementQuestionAsync(int profileId, int pathwayId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateIndustryPlacementQuestionViewModel>(User.GetUkPrn(), profileId, pathwayId);
            if (viewModel == null || !viewModel.IsLearnerRecordAdded)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner record not added. Method: UpdateIndustryPlacementAsync({User.GetUkPrn()}, {profileId}, {pathwayId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("update-learner-record-industry-placement-status", Name = RouteConstants.SubmitUpdateIndustryPlacementQuestion)]
        public async Task<IActionResult> UpdateIndustryPlacementQuestionAsync(UpdateIndustryPlacementQuestionViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var response = await _trainingProviderLoader.ProcessIndustryPlacementQuestionUpdateAsync(User.GetUkPrn(), viewModel);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.IndustryPlacementUpdatedConfirmation), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.IndustryPlacementUpdatedConfirmation);
        }

        [HttpGet]
        [Route("industry-placement-updated-confirmation", Name = RouteConstants.IndustryPlacementUpdatedConfirmation)]
        public async Task<IActionResult> IndustryPlacementUpdatedConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(string.Concat(CacheKey, Constants.IndustryPlacementUpdatedConfirmation));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read UpdateLearnerRecordResponseViewModel from redis cache in industry placement updated confirmation page. Method: IndustryPlacementUpdatedConfirmationAsync(), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("update-learner-record-english-and-maths-achievement/{profileId}", Name = RouteConstants.UpdateEnglisAndMathsAchievement)]
        public async Task<IActionResult> UpdateEnglisAndMathsAchievementAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateEnglishAndMathsQuestionViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null || !viewModel.IsLearnerRecordAdded || viewModel.HasLrsEnglishAndMaths || viewModel.EnglishAndMathsStatus == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner record not added or invalid record to show. Method: UpdateEnglisAndMathsAchievementAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("update-learner-record-english-and-maths-achievement", Name = RouteConstants.SubmitUpdateEnglisAndMathsAchievement)]
        public async Task<IActionResult> UpdateEnglisAndMathsAchievementAsync(UpdateEnglishAndMathsQuestionViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var response = await _trainingProviderLoader.ProcessEnglishAndMathsQuestionUpdateAsync(User.GetUkPrn(), viewModel);

            if (response == null)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!response.IsModified)
                return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { viewModel.ProfileId });

            if (!response.IsSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.EnglishAndMathsAchievementUpdatedConfirmation), response, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.EnglishAndMathsAchievementUpdatedConfirmation);
        }

        [HttpGet]
        [Route("english-and-maths-achievement-updated-confirmation", Name = RouteConstants.EnglishAndMathsAchievementUpdatedConfirmation)]
        public async Task<IActionResult> EnglishAndMathsAchievementUpdatedConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UpdateLearnerRecordResponseViewModel>(string.Concat(CacheKey, Constants.EnglishAndMathsAchievementUpdatedConfirmation));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read UpdateLearnerRecordResponseViewModel from redis cache in english and maths achievement updated confirmation page. Method: EnglishAndMathsAchievementUpdatedConfirmationAsync(), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        #endregion

        private async Task SyncCacheUln(EnterUlnViewModel model, FindLearnerRecord learnerRecord = null)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.Uln != null && cacheModel?.Uln?.EnterUln == model.EnterUln)
            {
                cacheModel.LearnerRecord = learnerRecord;
                cacheModel.Uln = model;
            }
            else
                cacheModel = new AddLearnerRecordViewModel { LearnerRecord = learnerRecord, Uln = model };

            await _cacheService.SetAsync(CacheKey, cacheModel);
        }
    }
}