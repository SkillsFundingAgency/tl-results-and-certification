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
        [Route("manage-learners/{academicYear}/{pageNumber:int?}", Name = RouteConstants.SearchLearnerDetails)]
        public async Task<IActionResult> SearchLearnerDetailsAsync(int academicYear, int? pageNumber)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            var searchFilters = (searchCriteria == null || searchCriteria.SearchLearnerFilters == null) 
                                ? await _trainingProviderLoader.GetSearchLearnerFiltersAsync(User.GetUkPrn()) 
                                : searchCriteria.SearchLearnerFilters;

            if (searchFilters == null)
                return RedirectToRoute(RouteConstants.PageNotFound);
                       

            if (searchCriteria == null)
                searchCriteria = new SearchCriteriaViewModel { AcademicYear = academicYear, PageNumber = pageNumber };
            else
            {
                searchCriteria.AcademicYear = academicYear;
                searchCriteria.PageNumber = pageNumber;
            }

            var learnersList = await _trainingProviderLoader.SearchLearnerDetailsAsync(User.GetUkPrn(), searchCriteria);

            if (searchCriteria.SearchLearnerFilters == null)
                searchCriteria.SearchLearnerFilters = searchFilters;

            var viewModel = new RegisteredLearnersViewModel
            {
                SearchCriteria = searchCriteria,
                SearchLearnerDetailsList = learnersList
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-learners/{academicYear}", Name = RouteConstants.SubmitSearchLearnerDetails)]
        public async Task<IActionResult> SearchLearnerDetailsAsync(SearchCriteriaViewModel viewModel)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            // populate if any filter are applied from cache
            if(searchCriteria != null)
                viewModel.SearchLearnerFilters = searchCriteria.SearchLearnerFilters;

            viewModel.IsSearchKeyApplied = true;
            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear = viewModel.AcademicYear });
        }

        [HttpGet]
        [Route("manage-learners-clearsearchkey/{academicYear}", Name = RouteConstants.SearchLearnerClearKey)]
        public async Task<IActionResult> SearchLearnerClearKeyAsync(int academicYear)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            if (searchCriteria != null)
            {
                searchCriteria.SearchKey = null;
                searchCriteria.IsSearchKeyApplied = false;
                await _cacheService.SetAsync(CacheKey, searchCriteria);
            }

            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear });
        }

        [HttpPost]
        [Route("manage-learners-applyfilters", Name = RouteConstants.SubmitSearchLearnerApplyFilters)]
        public async Task<IActionResult> SubmitSearchLearnerApplyFiltersAsync(SearchCriteriaViewModel viewModel)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            // populate searchKey and IsSerchKeyApplied from cache
            if (searchCriteria != null)
            {
                viewModel.SearchKey = searchCriteria.SearchKey;
                viewModel.IsSearchKeyApplied = searchCriteria.IsSearchKeyApplied;
            }

            viewModel.SearchLearnerFilters.IsApplyFiltersSelected = true;
            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear = viewModel.AcademicYear });
        }

        [HttpGet]
        [Route("manage-learners-clearfilters/{academicYear}", Name = RouteConstants.SearchLearnerClearFilters)]
        public async Task<IActionResult> SearchLearnerClearFiltersAsync(int academicYear)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            if(searchCriteria != null)
            {
                searchCriteria.SearchLearnerFilters = null;
                await _cacheService.SetAsync(CacheKey, searchCriteria);
            }
            
            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear });
        }

        //[HttpGet]
        //[Route("manage-registered-learners", Name = "ManageRegisteredLearners")]
        //public async Task<IActionResult> ManageRegisteredLearnersAsync()
        //{
        //    await _cacheService.RemoveAsync<SearchLearnerFiltersViewModel>(CacheKey);
        //    return RedirectToRoute(RouteConstants.GetRegisteredLearners);
        //}

        //[HttpGet]
        //[Route("manage-learners-new", Name = RouteConstants.GetRegisteredLearners)]
        //public async Task<IActionResult> GetRegisteredLearnersAsync()
        //{
        //    // 1. Build SearchCriteria
        //    //      1.1 CacheFound      --> GetLookupData + SelectedValues
        //    //      1.2 CacheNotFound   --> AskApi get FilterLookups and update Cache. 
        //    // 2. Request api 
        //    // 3. Build ViewModel 
        //    // 4. return View();

        //    // Step 1.2 from below. 
        //    var searchFilters = await _cacheService.GetAsync<SearchLearnerFiltersViewModel>(CacheKey);
        //    if (searchFilters == null)
        //        searchFilters = await _trainingProviderLoader.GetSearchLearnerFiltersAsync(User.GetUkPrn());

        //    var viewModel = new RegisteredLearnersViewModel
        //    {
        //        SearchCriteria = new SearchCriteriaViewModel { SearchLearnerFilters = searchFilters },
        //        SearchLearnerDetailsList = await _trainingProviderLoader.SearchLearnerDetailsAsync(User.GetUkPrn(), 2020) // TODO: ApplyFiter Story.
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[Route("manage-learners-new", Name = RouteConstants.SubmitGetRegisteredLearners)]
        //public async Task<IActionResult> GetRegisteredLearnersAsync(RegisteredLearnersViewModel viewModel)
        //{
        //    return View(viewModel);
        //}

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

        #region Update-Learner

        [HttpGet]
        [Route("search-learner-records", Name = RouteConstants.SearchLearnerRecord)]
        public async Task<IActionResult> SearchLearnerRecordAsync()
        {
            await _cacheService.RemoveAsync<SearchCriteriaViewModel>(CacheKey);
            var cacheModel = await _cacheService.GetAndRemoveAsync<SearchLearnerRecordViewModel>(CacheKey);
            var viewModel = cacheModel ?? new SearchLearnerRecordViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("search-learner-records", Name = RouteConstants.SubmitSearchLearnerRecord)]
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
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsLearnerRegistered)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learner record details found or learner is not registerd or learner record not added. Method: LearnerRecordDetailsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
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