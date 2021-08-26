using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Common.Constants;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireTLevelsReviewerAccess)]
    public class TlevelController : Controller
    {
        private readonly ITlevelLoader _tlevelLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;
        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TlevelCacheKey); } }


        public TlevelController(ITlevelLoader tlevelLoader, ICacheService cacheService,
            ILogger<TlevelController> logger)
        {
            _tlevelLoader = tlevelLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [Route("tlevels", Name = RouteConstants.TlevelsDashboard)]
        public IActionResult Index()
        {
            return View(new TlevelsDashboardViewModel());
        }

        [Route("review-tlevels", Name = RouteConstants.ReviewTlevels)]
        public async Task<IActionResult> ReviewTlevelsAsync()
        {
            var pendingTlevels = await _tlevelLoader.GetTlevelsByStatusIdAsync(User.GetUkPrn(), (int)TlevelReviewStatus.AwaitingConfirmation);
            if (pendingTlevels?.Count() > 1)
                return RedirectToRoute(RouteConstants.SelectTlevel);

            if (pendingTlevels?.Count() == 1)
                return RedirectToRoute(RouteConstants.ReviewTlevelDetails, new { id = pendingTlevels.FirstOrDefault().PathwayId });

            return RedirectToRoute(RouteConstants.AllTlevelsReviewed);
        }

        [Route("all-tlevels-reviewed", Name = RouteConstants.AllTlevelsReviewed)]
        public async Task<IActionResult> AllTlevelsReviewedAsync()
        {
            var pendingTlevels = await _tlevelLoader.GetTlevelsByStatusIdAsync(User.GetUkPrn(), (int)TlevelReviewStatus.AwaitingConfirmation);
            if (pendingTlevels?.Count() != 0)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(new AllTlevelsReviewedViewModel());
        }

        [Route("your-tlevels", Name = RouteConstants.YourTlevels)]
        public async Task<IActionResult> ViewAllAsync()
        {
            var viewModel = await _tlevelLoader.GetYourTlevelsViewModel(User.GetUkPrn());
            
            if (viewModel == null || (!viewModel.ConfirmedTlevels.Any() && !viewModel.QueriedTlevels.Any()))
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound, $"No T levels available to view. Method: GetYourTlevelsViewModel(Ukprn: {User.GetUkPrn()}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [Route("confirmed-tlevels", Name = RouteConstants.ConfirmedTlevels)]
        public async Task<IActionResult> ConfirmedTlevelsAsync()
        {
            var viewModel = await _tlevelLoader.GetConfirmedTlevelsViewModelAsync(User.GetUkPrn());
            if (viewModel == null || !viewModel.Tlevels.Any())
                return RedirectToRoute(RouteConstants.Home); // TODO: Other story to show none present. 

            return View(viewModel);
        }

        [Route("queried-tlevels", Name = RouteConstants.QueriedTlevels)]
        public async Task<IActionResult> QueriedTlevelsAsync()
        {
            var viewModel = await _tlevelLoader.GetQueriedTlevelsViewModelAsync(User.GetUkPrn());
            if (viewModel == null || !viewModel.Tlevels.Any())
                return RedirectToRoute(RouteConstants.Home); // TODO: Other story to show none present. 

            return View(viewModel);
        }

        [Route("tlevel-details/{id}", Name = RouteConstants.TlevelDetails)]
        [Route("tlevel-confirmed-details/{id}", Name = RouteConstants.TlevelConfirmedDetails)]        
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var viewModel = await _tlevelLoader.GetTlevelDetailsByPathwayIdAsync(User.GetUkPrn(), id);

            if(viewModel == null || !viewModel.IsValid)
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound, $"No T levels found. Method: GetTlevelDetailsByPathwayIdAsync(Ukprn: {User.GetUkPrn()}, id: {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("select-tlevel/{id:int?}", Name = RouteConstants.SelectTlevel)]
        public async Task<IActionResult> SelectToReviewAsync(int? id)
        {
            return await GetSelectToReviewByUkprn(User.GetUkPrn(), id);
        }

        [HttpPost]
        [Route("select-tlevel", Name = RouteConstants.SelectTlevelSubmit)]
        public async Task<IActionResult> SelectToReviewAsync(SelectToReviewPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await GetSelectToReviewByUkprn(User.GetUkPrn());
            }

            return RedirectToRoute(RouteConstants.ReviewTlevelDetails, new { id = model.SelectedPathwayId });
        }

        [HttpGet]
        [Route("review-t-level-details/{id}/{isback:bool?}", Name = RouteConstants.ReviewTlevelDetails)]
        public async Task<IActionResult> VerifyAsync(int id, bool isback)
        {
            var viewModel = await GetVerifyTlevelData(id);

            if (viewModel == null || viewModel.PathwayStatusId != (int)TlevelReviewStatus.AwaitingConfirmation)
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound, $"No T level found to verify. Method: GetVerifyTlevelDetailsByPathwayIdAsync(Ukprn: {User.GetUkPrn()}, PathwayId: {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (isback)
                viewModel.IsEverythingCorrect = false;

            return View(viewModel);
        }

        [HttpGet]        
        [Route("tlevel-details-queried-confirmation/{id}", Name = RouteConstants.TlevelDetailsQueriedConfirmation)]
        [Route("all-tlevels-reviewed-success/{id}", Name = RouteConstants.AllTlevelsReviewedSuccess)]
        [Route("review-more-tlevels/{id}", Name = RouteConstants.TlevelDetailsConfirmed)]
        public async Task<IActionResult> ConfirmationAsync(int id)
        {
            var isValid = await _cacheService.GetAndRemoveAsync<bool>(string.Concat(CacheKey, Constants.TlevelConfirmation));

            if (id == 0 || isValid == false)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed,
                    $"Unable to read T level confirmation result response from redis cache. Ukprn: {User.GetUkPrn()}, PathwayId: {id}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var confirmationViewModel = await _tlevelLoader.GetTlevelConfirmationDetailsAsync(User.GetUkPrn(), id);
            return View(confirmationViewModel);
        }

        [HttpPost]
        [Route("confirm-tlevel", Name = RouteConstants.ConfirmTlevel)]
        public async Task<IActionResult> ConfirmTlevelAsyc(ConfirmTlevelViewModel viewModel)
        {
            if (viewModel == null || viewModel.PathwayStatusId != (int)TlevelReviewStatus.AwaitingConfirmation)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!ModelState.IsValid)
            {
                var model = await GetVerifyTlevelData(viewModel.PathwayId);
                return View("Verify", model);
            }

            if (viewModel.IsEverythingCorrect == false)
            {
                TempData.Set(Constants.IsBackToVerifyPage, true);                
                return RedirectToRoute(RouteConstants.QueryTlevelDetails, new { id = viewModel.PathwayId });
            }

            var isSuccess = await _tlevelLoader.ConfirmTlevelAsync(viewModel);
            
            if(isSuccess)
            {
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.TlevelConfirmation), true, CacheExpiryTime.XSmall);
                
                var tlevelsToReview = await _tlevelLoader.GetTlevelsToReviewByUkprnAsync(User.GetUkPrn());
                return tlevelsToReview?.TlevelsToReview?.Count() == 0 
                    ? RedirectToRoute(RouteConstants.AllTlevelsReviewedSuccess, new { id = viewModel.PathwayId }) 
                    : RedirectToRoute(RouteConstants.TlevelDetailsConfirmed, new { id = viewModel.PathwayId });
            }
            else
            {
                _logger.LogWarning(LogEvent.TlevelsNotConfirmed,
                    $"Unable to confirm T level. Method: ConfirmTlevelAsync, Ukprn: {User.GetUkPrn()}, PathwayId: {viewModel.PathwayId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("query-t-level/{id}", Name=RouteConstants.QueryTlevelDetails)]
        public async Task<IActionResult> ReportIssueAsync(int id)
        {
            var tlevelDetails = await _tlevelLoader.GetQueryTlevelViewModelAsync(User.GetUkPrn(), id);
            if (tlevelDetails == null || (tlevelDetails.PathwayStatusId == (int)TlevelReviewStatus.Queried))
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound,
                    $"Unable to confirm T level. Method: GetQueryTlevelViewModelAsync(Ukprn: {User.GetUkPrn()}, Id: {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            tlevelDetails.IsBackToVerifyPage = TempData.Get<bool>(Constants.IsBackToVerifyPage); // TODO: next story.

            return View(tlevelDetails);
        }

        [HttpPost]
        [Route("query-t-level/{id}", Name = RouteConstants.SubmitTlevelIssue)]
        public async Task<IActionResult> ReportIssueAsync(TlevelQueryViewModel viewModel)
        {
            if (viewModel == null || viewModel.PathwayStatusId == (int)TlevelReviewStatus.Queried)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var tlevelDetails = await _tlevelLoader.GetQueryTlevelViewModelAsync(User.GetUkPrn(), viewModel.PathwayId);
            if (!ModelState.IsValid)
            {
                tlevelDetails.IsBackToVerifyPage = viewModel.IsBackToVerifyPage; // TODO
                return View(tlevelDetails);
            }

            var isSuccess = await _tlevelLoader.ReportIssueAsync(viewModel);
            if (!isSuccess)
            {
                _logger.LogWarning(LogEvent.TlevelReportIssueFailed, $"Unable to report T level issue. Method: ReportIssueAsync, Ukprn: {User.GetUkPrn()}, TqAwardingOrganisationId: {viewModel.TqAwardingOrganisationId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.QueryServiceProblem);
            }

            await _cacheService.SetAsync(CacheKey, new TlevelQuerySentViewModel { TlevelTitle = tlevelDetails.TlevelTitle });
            return RedirectToRoute(RouteConstants.QueryTlevelSent);
        }

        [HttpGet]
        [Route("tlevel-query-sent", Name = RouteConstants.QueryTlevelSent)]
        public async Task<IActionResult> ReportIssueSentAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<TlevelQuerySentViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read TlevelQuerySentViewModel from redis cache in T level query sent page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("query-service-problem", Name = RouteConstants.QueryServiceProblem)]
        public IActionResult QueryServiceProblem()
        {
            return View();
        }

        private async Task<ConfirmTlevelViewModel> GetVerifyTlevelData(int pathwayId)
        {
            return await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(User.GetUkPrn(), pathwayId);
        }

        private async Task<IActionResult> GetSelectToReviewByUkprn(long ukPrn, int? selectedPathwayId = null)
        {
            var viewModel = await _tlevelLoader.GetTlevelsToReviewByUkprnAsync(ukPrn);

            if (viewModel.TlevelsToReview?.Count() == 0)
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound, $"No T levels found to review. Method: GetTlevelsToReviewByUkprnAsync(Ukprn: {User.GetUkPrn()}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (selectedPathwayId.HasValue)
                viewModel.SelectedPathwayId = selectedPathwayId.Value;

            return View(viewModel);
        }
    }
}