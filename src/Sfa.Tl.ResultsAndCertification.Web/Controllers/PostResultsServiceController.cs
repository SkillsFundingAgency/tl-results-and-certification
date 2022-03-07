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
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Linq;
using System.Threading.Tasks;
using WithdrawAppealContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsWithdrawAppeal;

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
        public async Task<IActionResult> StartReviewsAndAppealsAsync()
        {
            await _cacheService.RemoveAsync<PrsSearchLearnerViewModel>(CacheKey);
            return View(new StartReviewsAndAppealsViewModel());
        }

        [HttpGet]
        [Route("reviews-and-appeals-search-learner/{populateUln:bool?}", Name = RouteConstants.PrsSearchLearner)]
        public async Task<IActionResult> PrsSearchLearnerAsync(bool populateUln)
        {
            var cacheModel = await _cacheService.GetAsync<PrsSearchLearnerViewModel>(CacheKey);
            var viewModel = cacheModel != null && populateUln ? cacheModel : new PrsSearchLearnerViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-search-learner", Name = RouteConstants.SubmitPrsSearchLearner)]
        public async Task<IActionResult> PrsSearchLearnerAsync(PrsSearchLearnerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prsLearnerRecord = await _postResultsServiceLoader.FindPrsLearnerRecordAsync(User.GetUkPrn(), model.SearchUln.ToLong());
            await _cacheService.SetAsync(CacheKey, model);

            if (prsLearnerRecord == null)
            {
                await _cacheService.SetAsync(CacheKey, new PrsUlnNotFoundViewModel { Uln = model.SearchUln }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsUlnNotFound);
            }
            else if (prsLearnerRecord.IsWithdrawn)
            {
                var prsUlnWithdrawnViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsUlnWithdrawnViewModel>(prsLearnerRecord);
                await _cacheService.SetAsync(CacheKey, prsUlnWithdrawnViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsUlnWithdrawn);
            }
            else if (prsLearnerRecord.NoAssessmentEntryRegistered)
            {
                var prsNoAssessmentEntryViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsNoAssessmentEntryViewModel>(prsLearnerRecord);
                await _cacheService.SetAsync(CacheKey, prsNoAssessmentEntryViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsNoAssessmentEntry);
            }
            else if (prsLearnerRecord.SingleAssessmentWithNoGrade)
            {
                var prsNoGradeViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsNoGradeRegisteredViewModel>(prsLearnerRecord);
                await _cacheService.SetAsync(CacheKey, prsNoGradeViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsNoGradeRegistered);
            }
            else if (prsLearnerRecord.HasMultipleAssessments)
            {
                return RedirectToRoute(RouteConstants.PrsSelectAssessmentSeries, new { profileId = prsLearnerRecord.ProfileId });
            }

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = prsLearnerRecord.ProfileId });
        }

        [HttpGet]
        [Route("reviews-and-appeals-uln-not-found", Name = RouteConstants.PrsUlnNotFound)]
        public async Task<IActionResult> PrsUlnNotFoundAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<PrsUlnNotFoundViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsUlnNotFoundViewModel from redis cache in request Prs Uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("reviews-and-appeals-learner-withdrawn", Name = RouteConstants.PrsUlnWithdrawn)]
        public async Task<IActionResult> PrsUlnWithdrawnAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<PrsUlnWithdrawnViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsUlnWithdrawnViewModel from redis cache in post results service uln withdrawn page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("reviews-and-appeals-no-assessment-entry", Name = RouteConstants.PrsNoAssessmentEntry)]
        public async Task<IActionResult> PrsNoAssessmentEntryAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<PrsNoAssessmentEntryViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsNoAssessmentEntryViewModel from redis cache in post results service no assessment entry page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("reviews-and-appeals-no-registered-grades", Name = RouteConstants.PrsNoGradeRegistered)]
        public async Task<IActionResult> PrsNoGradeRegisteredAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<PrsNoGradeRegisteredViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsNoGradeRegisteredViewModel from redis cache in post results service no grade registered page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("post-results-learners-grades/{profileId}", Name = RouteConstants.PrsLearnerDetails)]
        public async Task<IActionResult> PrsLearnerDetailsAsync(int profileId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(User.GetUkPrn(), profileId);
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            //viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
            return View(viewModel);
        }

        [HttpGet]
        [Route("reviews-and-appeals-appeal-grade/{profileId}/{assessmentId}/{resultId}", Name = RouteConstants.PrsAppealCoreGrade)]
        public async Task<IActionResult> PrsAppealCoreGradeAsync(int profileId, int assessmentId, int resultId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(User.GetUkPrn(), profileId, assessmentId);

            if (viewModel == null || viewModel.PathwayResultId != resultId || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-appeal-grade", Name = RouteConstants.SubmitPrsAppealCoreGrade)]
        public async Task<IActionResult> PrsAppealCoreGradeAsync(AppealCoreGradeViewModel model)
        {
            var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(User.GetUkPrn(), model.ProfileId, model.PathwayAssessmentId);
            if (!ModelState.IsValid)
                return View(prsDetails);

            if (prsDetails == null || !prsDetails.IsValid) 
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.AppealGrade == false)
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId });

            bool isSuccess = await _postResultsServiceLoader.AppealCoreGradeAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = prsDetails.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId });
        }

        [HttpGet]
        [Route("reviews-and-appeals-appeal-outcome-grade/{profileId}/{assessmentId}/{resultId}/{outcomeTypeId:int?}", Name = RouteConstants.PrsAppealOutcomePathwayGrade)]
        public async Task<IActionResult> PrsAppealOutcomePathwayGradeAsync(int profileId, int assessmentId, int resultId, int? outcomeTypeId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(User.GetUkPrn(), profileId, assessmentId);

            if (viewModel == null || viewModel.PathwayResultId != resultId || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SetOutcomeType(outcomeTypeId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-appeal-outcome-grade", Name = RouteConstants.SubmitPrsAppealOutcomePathwayGrade)]
        public async Task<IActionResult> PrsAppealOutcomePathwayGradeAsync(AppealOutcomePathwayGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealOutcomePathwayGradeViewModel>(User.GetUkPrn(), model.ProfileId, model.PathwayAssessmentId);
                return View(prsDetails);
            }

            await _cacheService.RemoveAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);

            if (model.AppealOutcome == AppealOutcomeType.SameGrade)
            {
                var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.PathwayAssessmentId);
                checkAndSubmitViewModel.NewGrade = checkAndSubmitViewModel.OldGrade;
                checkAndSubmitViewModel.IsGradeChanged = false;
                await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

                return RedirectToRoute(RouteConstants.PrsPathwayGradeCheckAndSubmit);
            }
            else if (model.AppealOutcome == AppealOutcomeType.WithdrawAppeal)
                return await PrsWithdrawAppealAsync(model);
            else
                return RedirectToRoute(RouteConstants.PrsAppealUpdatePathwayGrade, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId, resultId = model.PathwayResultId });
        }

        [HttpGet]
        [Route("reviews-and-appeals-check-and-submit", Name = RouteConstants.PrsPathwayGradeCheckAndSubmit)]
        public async Task<IActionResult> PrsPathwayGradeCheckAndSubmitAsync()
        {
            var viewModel = await _cacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read PrsPathwayGradeCheckAndSubmitViewModel from redis cache in Prs outcome check and submit page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-check-and-submit", Name = RouteConstants.SubmitPrsPathwayGradeCheckAndSubmit)]
        public async Task<IActionResult> PrsPathwayGradeCheckAndSubmitAsync(PrsPathwayGradeCheckAndSubmitViewModel model)
        {
            bool isSuccess = await _postResultsServiceLoader.AppealCoreGradeAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var notificationBanner = new NotificationBannerModel { Message = model.SuccessBannerMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.AssessmentId });
        }

        [HttpGet]
        [Route("reviews-and-appeals-appeal-update-grade/{profileId}/{assessmentId}/{resultId}/{isChangeMode:bool?}", Name = RouteConstants.PrsAppealUpdatePathwayGrade)]
        public async Task<IActionResult> PrsAppealUpdatePathwayGradeAsync(int profileId, int assessmentId, int resultId, bool isChangeMode = false)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(User.GetUkPrn(), profileId, assessmentId);

            if (viewModel == null || viewModel.PathwayResultId != resultId || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var checkAndSubmitDetails = await _cacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
            if (checkAndSubmitDetails != null && !isChangeMode)
                viewModel.SelectedGradeCode = viewModel.Grades?.FirstOrDefault(g => g.Value == checkAndSubmitDetails?.NewGrade)?.Code;

            viewModel.IsChangeMode = isChangeMode;
            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-appeal-update-grade", Name = RouteConstants.SubmitPrsAppealUpdatePathwayGrade)]
        public async Task<IActionResult> PrsAppealUpdatePathwayGradeAsync(AppealUpdatePathwayGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var prsDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealUpdatePathwayGradeViewModel>(User.GetUkPrn(), model.ProfileId, model.PathwayAssessmentId);
                prsDetails.IsChangeMode = model.IsChangeMode;
                return View(prsDetails);
            }

            var checkAndSubmitViewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(User.GetUkPrn(), model.ProfileId, model.PathwayAssessmentId);
            checkAndSubmitViewModel.NewGrade = model.Grades?.FirstOrDefault(x => x.Code == model.SelectedGradeCode)?.Value;

            if (string.IsNullOrWhiteSpace(checkAndSubmitViewModel.NewGrade))
                return RedirectToRoute(RouteConstants.PageNotFound);

            checkAndSubmitViewModel.IsGradeChanged = true;
            await _cacheService.SetAsync(CacheKey, checkAndSubmitViewModel);

            return RedirectToRoute(RouteConstants.PrsPathwayGradeCheckAndSubmit);
        }

        [NonAction]
        public async Task<IActionResult> PrsWithdrawAppealAsync(AppealOutcomePathwayGradeViewModel model)
        {
            bool isSuccess = await _postResultsServiceLoader.WithdrawAppealCoreGradeAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var successMessage = string.Format(WithdrawAppealContent.Success_Banner_Message, model.PathwayName, model.PathwayCode);
            var notificationBanner = new NotificationBannerModel { Message = successMessage };
            await _cacheService.SetAsync(CacheKey, notificationBanner, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId });
        }

        [HttpGet]
        [Route("select-exam-period/{profileId}", Name = RouteConstants.PrsSelectAssessmentSeries)]
        public async Task<IActionResult> PrsSelectAssessmentSeriesAsync(int profileId)
        {
            var prsLearner = await _postResultsServiceLoader.FindPrsLearnerRecordAsync(User.GetUkPrn(), null, profileId);
            if (prsLearner == null || prsLearner.Status != RegistrationPathwayStatus.Active || !prsLearner.HasMultipleAssessments)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(prsLearner);
            return View(viewModel);
        }

        [HttpPost]
        [Route("select-exam-period", Name = RouteConstants.SubmitPrsSelectAssessmentSeries)]
        public async Task<IActionResult> PrsSelectAssessmentSeriesAsync(PrsSelectAssessmentSeriesViewModel model)
        {
            var prsLearnerRecord = await _postResultsServiceLoader.FindPrsLearnerRecordAsync(User.GetUkPrn(), model.Uln);
            if (!ModelState.IsValid)
            {
                var prsSelectAssessmentSeriesViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsSelectAssessmentSeriesViewModel>(prsLearnerRecord);
                return View(prsSelectAssessmentSeriesViewModel);
            }

            var isSelectedSeriesHasResult = prsLearnerRecord.PathwayAssessments.Any(x => x.AssessmentId == model.SelectedAssessmentId && x.HasResult);
            if (!isSelectedSeriesHasResult)
            {
                var prsNoGradeViewModel = _postResultsServiceLoader.TransformLearnerDetailsTo<PrsNoGradeRegisteredViewModel>(prsLearnerRecord);
                await _cacheService.SetAsync(CacheKey, prsNoGradeViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsNoGradeRegistered);
            }

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = prsLearnerRecord.ProfileId, assessmentId = model.SelectedAssessmentId });
        }

        [HttpGet]
        [Route("reviews-and-appeals-cancel-appeal-update", Name = RouteConstants.PrsCancelAppealUpdate)]
        public async Task<IActionResult> PrsCancelAppealUpdateAsync()
        {
            var cacheModel = await _cacheService.GetAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new PrsCancelAppealUpdateViewModel { ProfileId = cacheModel.ProfileId, AssessmentId = cacheModel.AssessmentId };
            return View(viewModel);
        }

        [HttpPost]
        [Route("reviews-and-appeals-cancel-appeal-update", Name = RouteConstants.SubmitPrsCancelAppealUpdate)]
        public async Task<IActionResult> PrsCancelAppealUpdateAsync(PrsCancelAppealUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (viewModel.CancelRequest.Value)
            {
                await _cacheService.RemoveAsync<PrsPathwayGradeCheckAndSubmitViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId });
            }
            else
                return RedirectToRoute(RouteConstants.PrsPathwayGradeCheckAndSubmit);
        }

        [HttpGet]
        [Route("request-grade-change/{profileId}/{assessmentId}/{isResultJourney:bool?}", Name = RouteConstants.PrsGradeChangeRequest)]
        public async Task<IActionResult> PrsGradeChangeRequestAsync(int profileId, int assessmentId, bool? isResultJourney)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(User.GetUkPrn(), profileId, assessmentId);
            if (viewModel == null || !viewModel.CanRequestFinalGradeChange)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IsResultJourney = isResultJourney;
            return View(viewModel);
        }

        [HttpPost]
        [Route("request-grade-change/{profileId}/{assessmentId}/{isResultJourney:bool?}", Name = RouteConstants.SubmitPrsGradeChangeRequest)]
        public async Task<IActionResult> PrsGradeChangeRequestAsync(PrsGradeChangeRequestViewModel viewModel)
        {
            var learnerDetails = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(User.GetUkPrn(), viewModel.ProfileId, viewModel.AssessmentId);

            if (learnerDetails == null || !learnerDetails.CanRequestFinalGradeChange)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
            {
                learnerDetails.IsResultJourney = viewModel.IsResultJourney;
                return View(learnerDetails);
            }

            var isSuccess = await _postResultsServiceLoader.PrsGradeChangeRequestAsync(viewModel);

            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var confirmationViewModel = new PrsGradeChangeRequestConfirmationViewModel { ProfileId = viewModel.ProfileId, AssessmentId = viewModel.AssessmentId };
            await _cacheService.SetAsync(CacheKey, confirmationViewModel, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsGradeChangeRequestConfirmation);
        }

        [HttpGet]
        [Route("cancel-grade-change-request/{profileId}/{assessmentId}/{isResultJourney:bool?}", Name = RouteConstants.PrsCancelGradeChangeRequest)]
        public async Task<IActionResult> PrsCancelGradeChangeRequestAsync(int profileId, int assessmentId, bool isResultJourney)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsCancelGradeChangeRequestViewModel>(User.GetUkPrn(), profileId, assessmentId);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IsResultJourney = isResultJourney;
            return View(viewModel);
        }

        [HttpPost]
        [Route("cancel-grade-change-request/{profileId}/{assessmentId}/{isResultJourney:bool?}", Name = RouteConstants.SubmitPrsCancelGradeChangeRequest)]
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
                return RedirectToRoute(RouteConstants.PrsGradeChangeRequest, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId });

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId });
        }

        [HttpGet]
        [Route("grade-change-request-sent", Name = RouteConstants.PrsGradeChangeRequestConfirmation)]
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
        [Route("grade-change-request-sent", Name = RouteConstants.SubmitPrsGradeChangeRequestConfirmation)]
        public IActionResult PrsGradeChangeRequestConfirmation(PrsGradeChangeRequestConfirmationViewModel viewModel)
        {
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return viewModel.NavigationOption switch
            {
                PrsGradeChangeConfirmationNavigationOptions.BackToLearnersPage => RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId, assessmentId = viewModel.AssessmentId }),
                PrsGradeChangeConfirmationNavigationOptions.SearchForAnotherLearner => RedirectToRoute(RouteConstants.PrsSearchLearner),
                PrsGradeChangeConfirmationNavigationOptions.BackToHome => RedirectToRoute(RouteConstants.Home),
                _ => RedirectToRoute(RouteConstants.Home)
            };
        }

        [HttpGet]
        [Route("appeal-grade-after-deadline/{profileId}/{assessmentId}", Name = RouteConstants.PrsAppealGradeAfterDeadline)]
        public async Task<IActionResult> PrsAppealGradeAfterDeadlineAsync(int profileId, int assessmentId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineViewModel>(User.GetUkPrn(), profileId, assessmentId);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpGet]
        [Route("confirm-appeal-after-deadline/{profileId}/{assessmentId}", Name = RouteConstants.PrsAppealAfterDeadlineConfirm)]
        public async Task<IActionResult> PrsAppealGradeAfterDeadlineConfirmAsync(int profileId, int assessmentId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(User.GetUkPrn(), profileId, assessmentId);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("confirm-appeal-after-deadline/{profileId}/{assessmentId}", Name = RouteConstants.SubmitPrsAppealAfterDeadlineConfirm)]
        public async Task<IActionResult> PrsAppealGradeAfterDeadlineConfirmAsync(AppealGradeAfterDeadlineConfirmViewModel viewModel)
        {
            var prsLearner = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealGradeAfterDeadlineConfirmViewModel>(User.GetUkPrn(), viewModel.ProfileId, viewModel.PathwayAssessmentId);

            if (prsLearner == null || !prsLearner.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
                return View(prsLearner);

            if (viewModel.IsThisGradeBeingAppealed == false)
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = viewModel.ProfileId, assessmentId = viewModel.PathwayAssessmentId });

            var isSuccess = await _postResultsServiceLoader.AppealGradeAfterDeadlineRequestAsync(viewModel);

            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var confirmationViewModel = new PrsAppealGradeAfterDeadlineRequestConfirmationViewModel { ProfileId = viewModel.ProfileId, AssessmentId = viewModel.PathwayAssessmentId };
            await _cacheService.SetAsync(CacheKey, confirmationViewModel, CacheExpiryTime.XSmall);

            return RedirectToRoute(RouteConstants.PrsAppealGradeAfterDeadlineRequestConfirmation);
        }

        [HttpGet]
        [Route("appeal-after-deadline-request-sent", Name = RouteConstants.PrsAppealGradeAfterDeadlineRequestConfirmation)]
        public async Task<IActionResult> PrsAppealGradeAfterDeadlineRequestConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<PrsAppealGradeAfterDeadlineRequestConfirmationViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read PrsAppealGradeAfterDeadlineRequestConfirmationViewModel from redis cache in request Prs appeal grade after deadline request confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }        
    }
}