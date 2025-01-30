﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RommContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireReviewsAndAppealsEditorAccess)]
    public class PostResultsServiceController : Controller
    {
        private readonly IPostResultsServiceLoader _postResultsServiceLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.PrsCacheKey); } }

        public PostResultsServiceController(IPostResultsServiceLoader postResultsServiceLoader, ICacheService cacheService, ILogger<PostResultsServiceController> logger)
        {
            _postResultsServiceLoader = postResultsServiceLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("post-results-reviews-appeals-and-grade-changes", Name = RouteConstants.StartReviewsAndAppeals)]
        public IActionResult StartReviewsAndAppealsAsync()
        {
            return View(new StartReviewsAndAppealsViewModel());
        }

        [HttpGet]
        [Route("results-reviews-appeals-and-grade-changes", Name = RouteConstants.ResultReviewsAndAppeals)]
        public IActionResult ResultReviewsAndAppealsAsync()
        {
            return View(new ResultReviewsAndAppealsViewModel());
        }

        [HttpGet]
        [Route("post-results-learner-withdrawn/{profileId}", Name = RouteConstants.PrsUlnWithdrawn)]
        public async Task<IActionResult> PrsUlnWithdrawnAsync(int profileId)
        {
            var prsLearnerRecord = await _postResultsServiceLoader.FindPrsLearnerRecordAsync(User.GetUkPrn(), null, profileId);
            if (prsLearnerRecord == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No post-result learner details found. Method: PrsUlnWithdrawnAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var prsUlnWithdrawnViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsUlnWithdrawnViewModel>(prsLearnerRecord);
            return View(prsUlnWithdrawnViewModel);
        }

        [HttpGet]
        [Route("post-results-no-results/{profileId}", Name = RouteConstants.PrsNoResults)]
        public async Task<IActionResult> PrsNoResultsAsync(int profileId)
        {
            var prsLearnerRecord = await _postResultsServiceLoader.FindPrsLearnerRecordAsync(User.GetUkPrn(), null, profileId);
            if (prsLearnerRecord == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No post-result learner details found. Method: PrsNoResultsAsync({User.GetUkPrn()}, {profileId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            var prsNoResultsViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsNoResultsViewModel>(prsLearnerRecord);
            return View(prsNoResultsViewModel);
        }

        [HttpGet]
        [Route("post-results-learners-grades/{profileId}", Name = RouteConstants.PrsLearnerDetails)]
        public async Task<IActionResult> PrsLearnerDetailsAsync(int profileId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(User.GetUkPrn(), profileId);
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("post-results-add-romm/{profileId}/{assessmentId}/{componentType}/{isBack:bool?}", Name = RouteConstants.PrsAddRomm)]
        public async Task<IActionResult> PrsAddRommAsync(int profileId, int assessmentId, ComponentType componentType, bool? isBack)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IsRommRequested = isBack;

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-add-romm/{profileId}/{assessmentId}/{componentType}/{isBack:bool?}", Name = RouteConstants.SubmitPrsAddRomm)]
        public async Task<IActionResult> PrsAddRommAsync(PrsAddRommViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                return View(prsDetails);
            }

            if (model.IsRommRequested == true)
                return RedirectToRoute(RouteConstants.PrsAddRommOutcomeKnown, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType });
            else
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("post-results-add-romm-outcome/{profileId}/{assessmentId}/{componentType}/{outcomeTypeId:int?}", Name = RouteConstants.PrsAddRommOutcome)]
        public async Task<IActionResult> PrsAddRommOutcomeAsync(int profileId, int assessmentId, ComponentType componentType, int? outcomeTypeId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SetOutcomeType(outcomeTypeId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-add-romm-outcome/{profileId}/{assessmentId}/{componentType}/{outcomeTypeId:int?}", Name = RouteConstants.SubmitPrsAddRommOutcome)]
        public async Task<IActionResult> PrsAddRommOutcomeAsync(PrsAddRommOutcomeViewModel model)
        {
            var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);

            if (!ModelState.IsValid)
                return View(prsDetails);

            if (prsDetails == null || !prsDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.RommOutcome == RommOutcomeType.GradeChanged)
            {
                await _cacheService.RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsRommGradeChange, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType, isRommOutcomeJourney = "true" });
            }
            else if (model.RommOutcome == RommOutcomeType.GradeNotChanged)
            {
                var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                checkAndSubmitViewModel.NewGrade = checkAndSubmitViewModel.OldGrade;
                checkAndSubmitViewModel.IsGradeChanged = false;
                await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

                return RedirectToRoute(RouteConstants.PrsRommCheckAndSubmit);
            }
            else if (model.RommOutcome == RommOutcomeType.Withdraw)
            {
                bool isSuccess = await _postResultsServiceLoader.PrsRommActivityAsync(User.GetUkPrn(), model);
                if (!isSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = prsDetails.Banner_HeaderMesage, Message = prsDetails.SuccessBannerMessage };
                await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
        }

        [HttpGet]
        [Route("post-results-romm-outcome-known/{profileId}/{assessmentId}/{componentType}/{outcomeKnownTypeId:int?}", Name = RouteConstants.PrsAddRommOutcomeKnown)]
        public async Task<IActionResult> PrsAddRommOutcomeKnownAsync(int profileId, int assessmentId, ComponentType componentType, int? outcomeKnownTypeId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SetOutcomeType(outcomeKnownTypeId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-romm-outcome-known/{profileId}/{assessmentId}/{componentType}/{outcomeKnownTypeId:int?}", Name = RouteConstants.SubmitPrsAddRommOutcomeKnown)]
        public async Task<IActionResult> PrsAddRommOutcomeKnownAsync(PrsAddRommOutcomeKnownViewModel model)
        {
            var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);

            if (!ModelState.IsValid)
                return View(prsDetails);

            if (prsDetails == null || !prsDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.RommOutcome == RommOutcomeKnownType.GradeChanged)
            {
                await _cacheService.RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsRommGradeChange, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType });
            }
            else if (model.RommOutcome == RommOutcomeKnownType.GradeNotChanged)
            {
                var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                checkAndSubmitViewModel.NewGrade = checkAndSubmitViewModel.OldGrade;
                checkAndSubmitViewModel.IsGradeChanged = false;
                await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

                return RedirectToRoute(RouteConstants.PrsRommCheckAndSubmit);
            }
            else if (model.RommOutcome == RommOutcomeKnownType.No)
            {
                bool isSuccess = await _postResultsServiceLoader.PrsRommActivityAsync(User.GetUkPrn(), model);
                if (!isSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = prsDetails.Banner_HeaderMesage, Message = prsDetails.SuccessBannerMessage };
                await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
        }

        [HttpGet]
        [Route("post-results-romm-change-grade/{profileId}/{assessmentId}/{componentType}/{isRommOutcomeJourney:bool?}/{isChangeMode:bool?}", Name = RouteConstants.PrsRommGradeChange)]
        public async Task<IActionResult> PrsRommGradeChangeAsync(int profileId, int assessmentId, ComponentType componentType, bool? isRommOutcomeJourney, bool? isChangeMode)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var checkAndSubmitDetails = await _cacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
            if (checkAndSubmitDetails != null && (isChangeMode == null || isChangeMode.Value == false))
                viewModel.SelectedGradeCode = viewModel.Grades?.FirstOrDefault(g => g.Value == checkAndSubmitDetails?.NewGrade)?.Code;

            viewModel.IsRommOutcomeJourney = isRommOutcomeJourney ?? false;
            viewModel.IsChangeMode = isChangeMode ?? false;
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-romm-change-grade/{profileId}/{assessmentId}/{componentType}/{isRommOutcomeJourney:bool?}/{isChangeMode:bool?}", Name = RouteConstants.SubmitPrsRommGradeChange)]
        public async Task<IActionResult> PrsRommGradeChangeAsync(PrsRommGradeChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsRommGradeChangeViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                prsDetails.IsRommOutcomeJourney = model.IsRommOutcomeJourney;
                prsDetails.IsChangeMode = model.IsChangeMode;
                return View(prsDetails);
            }

            var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
            checkAndSubmitViewModel.NewGrade = model.Grades?.FirstOrDefault(x => x.Code == model.SelectedGradeCode)?.Value;

            if (string.IsNullOrWhiteSpace(checkAndSubmitViewModel.NewGrade))
                return RedirectToRoute(RouteConstants.PageNotFound);

            checkAndSubmitViewModel.IsGradeChanged = true;
            await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

            return RedirectToRoute(RouteConstants.PrsRommCheckAndSubmit);
        }

        [HttpGet]
        [Route("post-results-romm-check", Name = RouteConstants.PrsRommCheckAndSubmit)]
        public async Task<IActionResult> PrsRommCheckAndSubmitAsync()
        {
            var viewModel = await _cacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsRommCheckAndSubmitViewModel from redis cache in Prs romm outcome check and submit page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-romm-check", Name = RouteConstants.SubmitPrsRommCheckAndSubmit)]
        public async Task<IActionResult> PrsRommCheckAndSubmitAsync(PrsRommCheckAndSubmitViewModel model)
        {
            bool isSuccess = await _postResultsServiceLoader.PrsRommActivityAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
            var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = model.Banner_HeaderMesage, Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("post-results-cancel-ROMM-update", Name = RouteConstants.PrsCancelRommUpdate)]
        public async Task<IActionResult> PrsCancelRommUpdateAsync()
        {
            var cacheModel = await _cacheService.GetAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new PrsCancelRommUpdateViewModel { ProfileId = cacheModel.ProfileId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-cancel-ROMM-update", Name = RouteConstants.SubmitPrsCancelRommUpdate)]
        public async Task<IActionResult> PrsCancelRommUpdateAsync(PrsCancelRommUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.AreYouSureToCancel.Value)
            {
                await _cacheService.RemoveAsync<PrsRommCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsRommCheckAndSubmit);
            }
        }

        // Appeals Process

        [HttpGet]
        [Route("post-results-add-appeal/{profileId}/{assessmentId}/{componentType}/{isBack:bool?}", Name = RouteConstants.PrsAddAppeal)]
        public async Task<IActionResult> PrsAddAppealAsync(int profileId, int assessmentId, ComponentType componentType, bool? isBack)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IsAppealRequested = isBack;
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-add-appeal/{profileId}/{assessmentId}/{componentType}/{isBack:bool?}", Name = RouteConstants.SubmitPrsAddAppeal)]
        public async Task<IActionResult> PrsAddAppealAsync(PrsAddAppealViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                return View(prsDetails);
            }

            if (model.IsAppealRequested == true)
                return RedirectToRoute(RouteConstants.PrsAddAppealOutcomeKnown, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType });
            else
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("post-results-add-appeal-outcome/{profileId}/{assessmentId}/{componentType}/{outcomeTypeId:int?}", Name = RouteConstants.PrsAddAppealOutcome)]
        public async Task<IActionResult> PrsAddAppealOutcomeAsync(int profileId, int assessmentId, ComponentType componentType, int? outcomeTypeId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SetOutcomeType(outcomeTypeId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-add-appeal-outcome/{profileId}/{assessmentId}/{componentType}/{outcomeTypeId:int?}", Name = RouteConstants.SubmitPrsAddAppealOutcome)]
        public async Task<IActionResult> PrsAddAppealOutcomeAsync(PrsAddAppealOutcomeViewModel model)
        {
            var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);

            if (!ModelState.IsValid)
                return View(prsDetails);

            if (prsDetails == null || !prsDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.AppealOutcome == AppealOutcomeType.GradeChanged)
            {
                await _cacheService.RemoveAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsAppealGradeChange, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType, isAppealOutcomeJourney = "true" });
            }
            else if (model.AppealOutcome == AppealOutcomeType.GradeNotChanged)
            {
                var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                checkAndSubmitViewModel.NewGrade = checkAndSubmitViewModel.OldGrade;
                checkAndSubmitViewModel.IsGradeChanged = false;
                await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

                return RedirectToRoute(RouteConstants.PrsAppealCheckAndSubmit);
            }
            else if (model.AppealOutcome == AppealOutcomeType.Withdraw)
            {
                bool isSuccess = await _postResultsServiceLoader.PrsAppealActivityAsync(User.GetUkPrn(), model);
                if (!isSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = prsDetails.Banner_HeaderMesage, Message = prsDetails.SuccessBannerMessage };
                await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
        }

        [HttpGet]
        [Route("post-results-appeal-outcome-known/{profileId}/{assessmentId}/{componentType}/{outcomeKnownTypeId:int?}", Name = RouteConstants.PrsAddAppealOutcomeKnown)]
        public async Task<IActionResult> PrsAddAppealOutcomeKnownAsync(int profileId, int assessmentId, ComponentType componentType, int? outcomeKnownTypeId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SetOutcomeType(outcomeKnownTypeId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-appeal-outcome-known/{profileId}/{assessmentId}/{componentType}/{outcomeKnownTypeId:int?}", Name = RouteConstants.SubmitPrsAddAppealOutcomeKnown)]
        public async Task<IActionResult> PrsAddAppealOutcomeKnownAsync(PrsAddAppealOutcomeKnownViewModel model)
        {
            var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAddAppealOutcomeKnownViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);

            if (!ModelState.IsValid)
                return View(prsDetails);

            if (prsDetails == null || !prsDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.AppealOutcome == AppealOutcomeKnownType.GradeChanged)
            {
                await _cacheService.RemoveAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsAppealGradeChange, new { profileId = model.ProfileId, assessmentId = model.AssessmentId, componentType = (int)model.ComponentType });
            }
            else if (model.AppealOutcome == AppealOutcomeKnownType.GradeNotChanged)
            {
                var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                checkAndSubmitViewModel.NewGrade = checkAndSubmitViewModel.OldGrade;
                checkAndSubmitViewModel.IsGradeChanged = false;
                await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

                return RedirectToRoute(RouteConstants.PrsAppealCheckAndSubmit);
            }
            else if (model.AppealOutcome == AppealOutcomeKnownType.No)
            {
                bool isSuccess = await _postResultsServiceLoader.PrsAppealActivityAsync(User.GetUkPrn(), model);
                if (!isSuccess)
                    return RedirectToRoute(RouteConstants.ProblemWithService);

                var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = prsDetails.Banner_HeaderMesage, Message = prsDetails.SuccessBannerMessage };
                await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
        }

        [HttpGet]
        [Route("post-results-appeal-change-grade/{profileId}/{assessmentId}/{componentType}/{isAppealOutcomeJourney:bool?}/{isChangeMode:bool?}", Name = RouteConstants.PrsAppealGradeChange)]
        public async Task<IActionResult> PrsAppealGradeChangeAsync(int profileId, int assessmentId, ComponentType componentType, bool? isAppealOutcomeJourney, bool? isChangeMode)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var checkAndSubmitDetails = await _cacheService.GetAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
            if (checkAndSubmitDetails != null && (isChangeMode == null || isChangeMode.Value == false))
                viewModel.SelectedGradeCode = viewModel.Grades?.FirstOrDefault(g => g.Value == checkAndSubmitDetails?.NewGrade)?.Code;

            viewModel.IsAppealOutcomeJourney = isAppealOutcomeJourney ?? false;
            viewModel.IsChangeMode = isChangeMode ?? false;
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-appeal-change-grade/{profileId}/{assessmentId}/{componentType}/{isAppealOutcomeJourney:bool?}/{isChangeMode:bool?}", Name = RouteConstants.SubmitPrsAppealGradeChange)]
        public async Task<IActionResult> PrsAppealGradeChangeAsync(PrsAppealGradeChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAppealGradeChangeViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
                prsDetails.IsAppealOutcomeJourney = model.IsAppealOutcomeJourney;
                prsDetails.IsChangeMode = model.IsChangeMode;
                return View(prsDetails);
            }

            var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.AssessmentId, model.ComponentType);
            checkAndSubmitViewModel.NewGrade = model.Grades?.FirstOrDefault(x => x.Code == model.SelectedGradeCode)?.Value;

            if (string.IsNullOrWhiteSpace(checkAndSubmitViewModel.NewGrade))
                return RedirectToRoute(RouteConstants.PageNotFound);

            checkAndSubmitViewModel.IsGradeChanged = true;
            await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

            return RedirectToRoute(RouteConstants.PrsAppealCheckAndSubmit);
        }

        [HttpGet]
        [Route("post-results-appeal-check", Name = RouteConstants.PrsAppealCheckAndSubmit)]
        public async Task<IActionResult> PrsAppealCheckAndSubmitAsync()
        {
            var viewModel = await _cacheService.GetAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsAppealCheckAndSubmitViewModel from redis cache in Prs appeal outcome check and submit page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-appeal-check", Name = RouteConstants.SubmitPrsAppealCheckAndSubmit)]
        public async Task<IActionResult> PrsAppealCheckAndSubmitAsync(PrsAppealCheckAndSubmitViewModel model)
        {
            bool isSuccess = await _postResultsServiceLoader.PrsAppealActivityAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.RemoveAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
            var notificationBanner = new NotificationBannerModel { DisplayMessageBody = true, HeaderMessage = model.Banner_HeaderMesage, Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("post-results-cancel-appeal-update", Name = RouteConstants.PrsCancelAppealUpdate)]
        public async Task<IActionResult> PrsCancelAppealUpdateAsync()
        {
            var cacheModel = await _cacheService.GetAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new PrsCancelAppealUpdateViewModel { ProfileId = cacheModel.ProfileId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-cancel-appeal-update", Name = RouteConstants.SubmitPrsCancelAppealUpdate)]
        public async Task<IActionResult> PrsCancelAppealUpdateAsync(PrsCancelAppealUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.AreYouSureToCancel.Value)
            {
                await _cacheService.RemoveAsync<PrsAppealCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId });
            }
            else
            {
                return RedirectToRoute(RouteConstants.PrsAppealCheckAndSubmit);
            }
        }

        [HttpGet]
        [Route("post-results-final-grade-change-request/{profileId}/{assessmentId}/{componentType}", Name = RouteConstants.PrsGradeChangeRequest)]
        public async Task<IActionResult> PrsGradeChangeRequestAsync(int profileId, int assessmentId, ComponentType componentType)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);
            if (viewModel == null || !viewModel.CanRequestFinalGradeChange)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-final-grade-change-request/{profileId}/{assessmentId}/{componentType}", Name = RouteConstants.SubmitPrsGradeChangeRequest)]
        public async Task<IActionResult> PrsGradeChangeRequestAsync(PrsGradeChangeRequestViewModel viewModel)
        {
            var learnerDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(User.GetUkPrn(), viewModel.ProfileId, viewModel.AssessmentId, viewModel.ComponentType);

            if (learnerDetails == null || !learnerDetails.CanRequestFinalGradeChange)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
            {
                return View(learnerDetails);
            }

            var isSuccess = await _postResultsServiceLoader.PrsGradeChangeRequestAsync(viewModel);

            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var confirmationViewModel = new PrsGradeChangeRequestConfirmationViewModel { ProfileId = viewModel.ProfileId };
            await _cacheService.SetAsync(CacheKey, confirmationViewModel, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsGradeChangeRequestConfirmation);
        }

        [HttpGet]
        [Route("post-results-cancel-final-grade-change-request/{profileId}/{assessmentId}/{componentType}/{isResultJourney:bool?}", Name = RouteConstants.PrsCancelGradeChangeRequest)]
        public async Task<IActionResult> PrsCancelGradeChangeRequestAsync(int profileId, int assessmentId, ComponentType componentType, bool isResultJourney)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(User.GetUkPrn(), profileId, assessmentId, componentType);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IsResultJourney = isResultJourney;

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-cancel-final-grade-change-request/{profileId}/{assessmentId}/{componentType}/{isResultJourney:bool?}", Name = RouteConstants.SubmitPrsCancelGradeChangeRequest)]
        public IActionResult PrsCancelGradeChangeRequest(PrsCancelGradeChangeRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (viewModel.IsResultJourney)
            {
                if (!viewModel.AreYouSureToCancel.Value)
                    return RedirectToRoute(RouteConstants.PrsGradeChangeRequest, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId, isResultJourney = true.ToString() });

                return RedirectToRoute(RouteConstants.ResultDetails, new { profileId = viewModel.ProfileId });
            }

            if (!viewModel.AreYouSureToCancel.Value)
                return RedirectToRoute(RouteConstants.PrsGradeChangeRequest, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId, componentType = (int)viewModel.ComponentType });

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId });
        }

        [HttpGet]
        [Route("post-results-final-grade-change-request-sent", Name = RouteConstants.PrsGradeChangeRequestConfirmation)]
        public async Task<IActionResult> PrsGradeChangeRequestConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<PrsGradeChangeRequestConfirmationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read PrsGradeChangeRequestConfirmationViewModel from redis cache in request Prs grade change confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("post-results-final-grade-change-request-sent", Name = RouteConstants.SubmitPrsGradeChangeRequestConfirmation)]
        public IActionResult PrsGradeChangeRequestConfirmation(PrsGradeChangeRequestConfirmationViewModel viewModel)
        {
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return viewModel.NavigationOption switch
            {
                PrsGradeChangeConfirmationNavigationOptions.BackToLearnersPage => RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId }),
                PrsGradeChangeConfirmationNavigationOptions.SearchForAnotherLearner => RedirectToRoute(RouteConstants.SearchRegistration, new { type = SearchRegistrationType.PostResult.ToString() }),
                PrsGradeChangeConfirmationNavigationOptions.BackToHome => RedirectToRoute(RouteConstants.Home),
                _ => RedirectToRoute(RouteConstants.Home)
            };
        }

        #region Romms Upload

        [HttpGet]
        [Route("upload-romms-file/{requestErrorTypeId:int?}", Name = RouteConstants.UploadRommsFile)]
        public IActionResult UploadRommsFile(int? requestErrorTypeId)
        {
            var model = new UploadRommsRequestViewModel { RequestErrorTypeId = requestErrorTypeId };
            model.SetAnyModelErrors(ModelState);
            return View(model);
        }

        [HttpPost]
        [Route("upload-romms-file", Name = RouteConstants.SubmitUploadRommsFile)]
        public async Task<IActionResult> UploadRommsFileAsync(UploadRommsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _postResultsServiceLoader.ProcessBulkRommsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);

                return RedirectToRoute(RouteConstants.RommsUploadSuccessful);
            }
            else
            {
                if (response.ShowProblemWithServicePage)
                {
                    return RedirectToRoute(RouteConstants.ProblemWithRommsUpload);
                }
                else
                {
                    var unsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = response.BlobUniqueReference, FileSize = response.ErrorFileSize, FileType = FileType.Csv.ToString().ToUpperInvariant() };
                    await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel), unsuccessfulViewModel, CacheExpiryTime.XSmall);
                    return RedirectToRoute(RouteConstants.RommsUploadUnsuccessful);
                }
            }
        }

        [HttpGet]
        [Route("romms-upload-success", Name = RouteConstants.RommsUploadSuccessful)]
        public async Task<IActionResult> UploadRommsSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadSuccessfulViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed,
                    $"Unable to read upload successful romms response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("romms-upload-unsuccessful", Name = RouteConstants.RommsUploadUnsuccessful)]
        public async Task<IActionResult> UploadRommsUnsuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful romms response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-romm-errors", Name = RouteConstants.DownloadRommErrors)]
        public async Task<IActionResult> DownloadRommsErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _postResultsServiceLoader.GetRommValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download withdrawl validation errors. Method: GetRommValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = RommContent.UploadRommsUnsuccessful.Romms_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadRommErrorsFailed, $"Not a valid guid to read file.Method: DownloadRommErrors(Id = {id}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }
        #endregion

        #region Romms Download

        [HttpGet]
        [Route("romms-generating-download", Name = RouteConstants.RommsGeneratingDownload)]
        public IActionResult RommsGeneratingDownload()
        {
            return View();
        }

        [HttpPost]
        [Route("romms-generating-download", Name = RouteConstants.SubmitRommsGeneratingDownload)]
        public async Task<IActionResult> SubmitRommsGeneratingDownloadAsync()
        {
            long ukprn = User.GetUkPrn();
            string email = User.GetUserEmail();

            var postResultServiceResponse = await _postResultsServiceLoader.GenerateRommsDataExportAsync(ukprn, email);

            if (!postResultServiceResponse.ContainsSingle())
                return RedirectToRoute(RouteConstants.ProblemWithService);

            if (!postResultServiceResponse.Any(x => x.IsDataFound))
            {
                _logger.LogWarning(LogEvent.NoDataFound,
                    $"There are no romms found. Method: GenerateRommsDataFileAsync({ukprn}, {email})");

                return RedirectToRoute(RouteConstants.RegistrationsNoRecordsFound);
            }

            RommsDownloadViewModel rommsDownloadViewModel = new()
            {
                RommsDownloadLinkViewModel = CreateDownloadLink(postResultServiceResponse.First()),
            };

            await _cacheService.SetAsync(CacheKey, rommsDownloadViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.RommsDownloadData);

            static DownloadLinkViewModel CreateDownloadLink(DataExportResponse response)
                => new()
                {
                    BlobUniqueReference = response.BlobUniqueReference,
                    FileSize = response.FileSize,
                    FileType = FileType.Csv.ToString().ToUpperInvariant()
                };
        }

        [HttpGet]
        [Route("romms-no-records-found", Name = RouteConstants.RommsNoRecordsFound)]
        public IActionResult RommsNoRecordsFound()
        {
            return View(new RommsNoRecordsFoundViewModel());
        }

        [HttpGet]
        [Route("romms-download-data", Name = RouteConstants.RommsDownloadData)]
        public async Task<IActionResult> RommsDownloadDataAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<RommsDownloadViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read DataExportResponse from redis cache in romms download page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("download-romms-data/{id}", Name = RouteConstants.RommsDownloadDataLink)]
        public Task<IActionResult> RommsDownloadDataLinkAsync(string id)
        {
            return DownloadDataLinkAsync(
                id,
                () => _postResultsServiceLoader.GetRommsDataFileAsync(User.GetUkPrn(), id.ToGuid()),
                RommContent.RommsDownloadData.Romms_Data_Report_File_Name_Text,
                nameof(RommsDownloadDataLinkAsync));
        }

        #endregion

        private async Task<IActionResult> DownloadDataLinkAsync(string id, Func<Task<Stream>> getDataFile, string fileDownloadName, string methodName)
        {
            if (!id.IsGuid())
            {
                _logger.LogWarning(LogEvent.DocumentDownloadFailed, $"Not a valid guid to read file.Method: {methodName}(Id = {id}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }

            var fileStream = await getDataFile();
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download romm data. Method: {methodName}(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/csv")
            {
                FileDownloadName = fileDownloadName
            };
        }
    }
}