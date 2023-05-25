using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Linq;
using System.Threading.Tasks;

using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;
using RequestReplacementDocumentContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.RequestReplacementDocument;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class TrainingProviderController : Controller
    {
        private readonly ITrainingProviderLoader _trainingProviderLoader;
        private readonly ICacheService _cacheService;
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TrainingProviderCacheKey); } }
        private string InformationCacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TrainingProviderInformationCacheKey); } }

        public TrainingProviderController(ITrainingProviderLoader trainingProviderLoader, ICacheService cacheService, ResultsAndCertificationConfiguration configuration, ILogger<TrainingProviderController> logger)
        {
            _trainingProviderLoader = trainingProviderLoader;
            _cacheService = cacheService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("manage-learners/{academicYear}/{pageNumber:int?}", Name = RouteConstants.SearchLearnerDetails)]
        public async Task<IActionResult> SearchLearnerDetailsAsync(int academicYear, int? pageNumber)
        {
            var searchFilters = await _trainingProviderLoader.GetSearchLearnerFiltersAsync(User.GetUkPrn());
            if (searchFilters == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            if (searchCriteria == null)
                searchCriteria = new SearchCriteriaViewModel { AcademicYear = academicYear, PageNumber = pageNumber };
            else
            {
                searchCriteria.AcademicYear = academicYear;
                searchCriteria.PageNumber = pageNumber;

                if (searchCriteria.SearchLearnerFilters != null)
                {
                    searchCriteria.SearchLearnerFilters.Tlevels?.ToList().ForEach(tl => tl.Name = searchFilters.Tlevels.FirstOrDefault(x => x.Id == tl.Id)?.Name);
                    searchCriteria.SearchLearnerFilters.Status?.ToList().ForEach(s => s.Name = searchFilters.Status.FirstOrDefault(x => x.Id == s.Id)?.Name);
                }
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
            if (searchCriteria != null)
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

            if (searchCriteria != null)
            {
                searchCriteria.SearchLearnerFilters = null;
                await _cacheService.SetAsync(CacheKey, searchCriteria);
            }

            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear });
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
        [Route("request-replacement-document/{profileId}", Name = RouteConstants.RequestReplacementDocument)]
        public async Task<IActionResult> RequestReplacementDocumentAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null || !CommonHelper.IsDocumentRerequestEligible(_configuration.DocumentRerequestInDays, viewModel.LastDocumentRequestedDate))
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("request-replacement-document/{profileId}", Name = RouteConstants.SubmitRequestReplacementDocument)]
        public async Task<IActionResult> RequestReplacementDocumentAsync(RequestReplacementDocumentViewModel model)
        {
            var isSuccess = await _trainingProviderLoader.CreateReplacementDocumentPrintingRequestAsync(User.GetUkPrn(), model);

            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { HeaderMessage = RequestReplacementDocumentContent.Success_Header_Replacement_Document_Requested, Message = RequestReplacementDocumentContent.Success_Message_Replacement_Documents, DisplayMessageBody = true, IsRawHtml = true };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        #region Pending withdrawal

        [HttpGet]
        [Route("manage-add-withdrawn-status/{profileId}", Name = RouteConstants.AddWithdrawnStatus)]
        public async Task<IActionResult> AddWithdrawnStatusAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<AddWithdrawnStatusViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-add-withdrawn-status", Name = RouteConstants.SubmitWithdrawnStatus)]
        public async Task<IActionResult> SubmitWithdrawnStatusAsync(WithdrawnConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool pendingWithdrawl = model.IsPendingWithdrawl.HasValue && model.IsPendingWithdrawl.Value;
            bool withdrawnConfirmed = model.IsWithdrawnConfirmed.HasValue && model.IsWithdrawnConfirmed.Value;

            if (pendingWithdrawl && withdrawnConfirmed) {
                var isSuccess = await _trainingProviderLoader.UpdateLearnerWithdrawnStatusAsync(User.GetUkPrn(), model);

                if (!isSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
            }
            else if (pendingWithdrawl){
                return View(model);
            }

            return RedirectToRoute(RouteConstants.WithdrawLearnerAOMessage, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("manage-learners-change-back-to-active-status/{profileId}", Name = RouteConstants.ChangeBackToActiveStatus)]
        public async Task<IActionResult> ChangeBackToActiveStatusAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-learners-change-back-to-active-status", Name = RouteConstants.SubmitChangeBackToActiveStatus)]
        public IActionResult ChangeBackToActiveStatusAsync(ChangeBackToActiveStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool yesSelected = model.ChangeBackToActive.HasValue && model.ChangeBackToActive.Value;

            if (yesSelected)
            {
                return RedirectToRoute(RouteConstants.ChangeBackToActiveStatusHaveYouToldAwardingOrganisation, new { profileId = model.ProfileId });
            }

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });

        }

        [HttpGet]
        [Route("withdraw-learner-ao-message/{profileId}", Name = RouteConstants.WithdrawLearnerAOMessage)]
        public async Task<IActionResult> WithdrawLearnerAOMessageAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<WithdrawLearnerAOMessageViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpGet]
        [Route("manage-learners-change-back-to-active-status-have-you-told-ao/{profileId}", Name = RouteConstants.ChangeBackToActiveStatusHaveYouToldAwardingOrganisation)]
        public async Task<IActionResult> ChangeBackToActiveStatusHaveYouToldAwardingOrganisationAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("manage-learners-change-back-to-active-status-have-you-told-ao", Name = RouteConstants.SubmitChangeBackToActiveStatusHaveYouToldAwardingOrganisation)]
        public async Task<IActionResult> ChangeBackToActiveStatusHaveYouToldAwardingOrganisationAsync(ChangeBackToActiveStatusHaveYouToldAwardingOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool yesSelected = model.HaveYouToldAwardingOrganisation.HasValue && model.HaveYouToldAwardingOrganisation.Value;

            if (yesSelected)
            {
                await _trainingProviderLoader.ReinstateRegistrationFromPendingWithdrawalAsync(model);

                await _cacheService.SetAsync(
                    InformationCacheKey,
                    new InformationBannerModel
                    {
                        Message = string.Format(LearnerDetailsContent.Reinstate_Message_Template, model.LearnerName)
                    },
                    CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
            }

            return RedirectToRoute(RouteConstants.ChangeBackToActiveAOMessage, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("manage-learners-change-back-to-active-status-ao-message/{profileId}", Name = RouteConstants.ChangeBackToActiveAOMessage)]
        public async Task<IActionResult> ChangeBackToActiveAOMessageAsync(int profileId)
        {
            var viewModel = await _trainingProviderLoader.GetLearnerRecordDetailsAsync<ChangeBackToActiveAOMessageViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        #endregion

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

            viewModel.IsDocumentRerequestEligible = CommonHelper.IsDocumentRerequestEligible(_configuration.DocumentRerequestInDays, viewModel.LastDocumentRequestedDate);

            viewModel.InformationBanner = await _cacheService.GetAndRemoveAsync<InformationBannerModel>(InformationCacheKey);
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