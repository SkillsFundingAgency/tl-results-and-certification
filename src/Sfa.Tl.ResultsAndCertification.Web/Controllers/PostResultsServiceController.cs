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
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using PrsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireReviewsAndAppealsEditorAccess)]
    public class PostResultsServiceController : Controller
    {
        private readonly IPostResultsServiceLoader _postResultsServiceLoader;
        private readonly ICacheService _cacheService;
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.PrsCacheKey); } }

        public PostResultsServiceController(IPostResultsServiceLoader postResultsServiceLoader, ICacheService cacheService, ResultsAndCertificationConfiguration configuration, ILogger<PostResultsServiceController> logger)
        {
            _postResultsServiceLoader = postResultsServiceLoader;
            _cacheService = cacheService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("reviews-and-appeals", Name = RouteConstants.StartReviewsAndAppeals)]
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
                await _cacheService.SetAsync(CacheKey, new PrsUlnWithdrawnViewModel
                {
                    Uln = prsLearnerRecord.Uln,
                    Firstname = prsLearnerRecord.Firstname,
                    Lastname = prsLearnerRecord.Lastname,
                    DateofBirth = prsLearnerRecord.DateofBirth,
                    ProviderName = prsLearnerRecord.ProviderName,
                    ProviderUkprn = prsLearnerRecord.ProviderUkprn,
                    TlevelTitle = prsLearnerRecord.TlevelTitle
                }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.PrsUlnWithdrawn);
            }

            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = prsLearnerRecord.ProfileId, assessmentId = 1 }); // TODO: temporarily redirected to assessmentId
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
        [Route("reviews-and-appeals-learner-status/{profileId}/{assessmentId}", Name = RouteConstants.PrsLearnerDetails)]
        public async Task<IActionResult> PrsLearnerDetailsAsync(int profileId, int assessmentId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(User.GetUkPrn(), profileId, assessmentId);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.SuccessBanner = await _cacheService.GetAndRemoveAsync<NotificationBannerModel>(CacheKey);
            if (viewModel.SuccessBanner != null)
                viewModel.SuccessBanner.Message = string.Format(PrsContent.Banner_Message, string.Format(PrsContent.Heading_Core, viewModel.PathwayTitle));

            return View(viewModel);
        }

        [HttpGet]
        [Route("reviews-and-appeals-appeal-grade/{profileId}/{assessmentId}/{resultId}", Name = RouteConstants.PrsAppealCoreGrade)]
        public async Task<IActionResult> PrsAppealCoreGradeAsync(int profileId, int assessmentId, int resultId)
        {
            var viewModel = await _postResultsServiceLoader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(User.GetUkPrn(), profileId, assessmentId);

            if (viewModel == null || viewModel.PathwayResultId != resultId || !CommonHelper.IsAppealsAllowed(_configuration.AppealsEndDate))
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

            if (prsDetails == null) // && TODO: check Current Prs status should be Null -> TLRC: 3480
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.AppealGrade == false)
                return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId });

            bool isSuccess = await _postResultsServiceLoader.AppealCoreGradeAsync(User.GetUkPrn(), model);
            if (!isSuccess)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            await _cacheService.SetAsync(CacheKey, new NotificationBannerModel(), CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.PrsLearnerDetails, new { profileId = model.ProfileId, assessmentId = model.PathwayAssessmentId });
        }
    }
}