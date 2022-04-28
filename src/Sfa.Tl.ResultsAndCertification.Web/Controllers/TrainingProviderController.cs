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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

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
        [Route("manage-learner-maths-level/{profileId}", Name = RouteConstants.AddMathsStatus)]
        public async Task<IActionResult> AddMathsStatusAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-learner-maths-level/{profileId}", Name = RouteConstants.SubmitAddMathsStatus)]
        public async Task<IActionResult> AddMathsStatusAsync(AddMathsStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var isSuccess = await _trainingProviderLoader.UpdateLearnerSubjectAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { HeaderMessage = LearnerDetailsContent.Success_Header_Maths_Status_Added, Message = LearnerDetailsContent.Success_Message_Maths_Status_Added, DisplayMessageBody = true, IsRawHtml = true };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("manage-learner-english-level/{profileId}", Name = RouteConstants.AddEnglishStatus)]
        public async Task<IActionResult> AddEnglishStatusAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<AddEnglishStatusViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-learner-english-level/{profileId}", Name = RouteConstants.SubmitAddEnglishStatus)]
        public async Task<IActionResult> AddEnglishStatusAsync(AddEnglishStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var isSuccess = await _trainingProviderLoader.UpdateLearnerSubjectAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { HeaderMessage = LearnerDetailsContent.Success_Header_English_Status_Added, Message = LearnerDetailsContent.Success_Message_English_Status_Added, DisplayMessageBody = true, IsRawHtml = true };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("has-learner-completed-industry-placement/{isChangeMode:bool?}", Name = RouteConstants.AddIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync(bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false ||
                (cacheModel?.LearnerRecord?.HasLrsEnglishAndMaths == false))
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
            // DELETE
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
                //await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AddLearnerRecordConfirmation), new LearnerRecordConfirmationViewModel { Uln = response.Uln, Name = response.Name }, CacheExpiryTime.XSmall);  DELETE
                return RedirectToRoute("DELETE");
            }
            else
            {
                _logger.LogWarning(LogEvent.AddLearnerRecordFailed, $"Unable to add learner record for UniqueLearnerNumber: {cacheModel.Uln}. Method: SubmitLearnerRecordCheckAndSubmitAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        #region Update-Learner

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
        [Route("learner-record-page/{profileId}", Name = RouteConstants.LearnerRecordDetails)]
        public async Task<IActionResult> LearnerRecordDetailsAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel1>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsLearnerRegistered)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner is not registerd or learner record not added. Method: LearnerRecordDetailsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
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

        #endregion

        #region Guidance

        [HttpGet]
        [Route("provider-guidance", Name = RouteConstants.ProviderGuidance)]
        public IActionResult ProviderGuidance()
        {           
            return View();
        }

        [HttpGet]
        [Route("provider-timeline", Name = RouteConstants.ProviderTimeline)]
        public IActionResult ProviderTimeline()
        {
            return View();
        }

        #endregion
    }
}