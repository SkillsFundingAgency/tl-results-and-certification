using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Web.Session;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireTLevelsReviewerAccess)]
    public class TlevelController : Controller
    {
        private readonly ITlevelLoader _tlevelLoader;
        private readonly ISessionService _sessionService;
        private readonly ILogger _logger;

        // TODO: Standard unique-key to be finalised.
        public string TlevelVerifySessionKey { get { return $"TlevelVerification-{HttpContext.User.GetUserEmail()}"; } }

        public TlevelController(ITlevelLoader tlevelLoader, 
            ISessionService sessionService,
            ILogger<TlevelController> logger)
        {
            _tlevelLoader = tlevelLoader;
            _sessionService = sessionService;
            _logger = logger;
        }

        [Route("tlevels", Name = RouteConstants.Tlevels)]
        public async Task<IActionResult> IndexAsync()
        {
            var pendingTlevels = await _tlevelLoader.GetTlevelsByStatusIdAsync(User.GetUkPrn(), (int)TlevelReviewStatus.AwaitingConfirmation);
            if (pendingTlevels?.Count() > 0)
            {
                _sessionService.Remove(TlevelVerifySessionKey);
                return RedirectToRoute(RouteConstants.TlevelSelect);
            }

            return RedirectToRoute(RouteConstants.ViewAllTlevels);
        }

        [Route("view-all-tlevels", Name = RouteConstants.ViewAllTlevels)]
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

        [Route("tlevel-details/{id}", Name = RouteConstants.TlevelDetails)]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var viewModel = await _tlevelLoader.GetTlevelDetailsByPathwayIdAsync(User.GetUkPrn(), id);

            if(viewModel == null)
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound, $"No T levels found. Method: GetTlevelDetailsByPathwayIdAsync(Ukprn: {User.GetUkPrn()}, id: {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("tlevel-select/{id:int?}", Name = RouteConstants.TlevelSelect)]
        public async Task<IActionResult> SelectToReviewAsync(int? id)
        {
            return await GetSelectToReviewByUkprn(User.GetUkPrn(), id);
        }

        [HttpPost]
        [Route("tlevel-select", Name = RouteConstants.TlevelSelectSubmit)]
        public async Task<IActionResult> SelectToReviewAsync(SelectToReviewPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await GetSelectToReviewByUkprn(User.GetUkPrn());
            }

            _sessionService.Set(TlevelVerifySessionKey, model.SelectedPathwayId);

            return RedirectToRoute(RouteConstants.VerifyTlevel, new { id = model.SelectedPathwayId });
        }

        [HttpGet]
        [Route("verify-tlevel/{id}/{isback:bool?}", Name = RouteConstants.VerifyTlevel)]
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
        [Route("tlevel-confirmation/{id}", Name = RouteConstants.TlevelConfirmation)]
        public async Task<IActionResult> ConfirmationAsync(int id)
        {
            if (id == 0 || TempData[Constants.IsRedirect] == null || !(bool.TryParse(TempData[Constants.IsRedirect].ToString(), out bool isRedirect) && isRedirect))
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed,
                    $"Unable to read T level confirmation page temp data. Ukprn: {User.GetUkPrn()}, PathwayId: {id}, User: {User.GetUserEmail()}");
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
                return RedirectToRoute(RouteConstants.ReportTlevelIssue, new { id = viewModel.PathwayId });
            }

            var isSuccess = await _tlevelLoader.ConfirmTlevelAsync(viewModel);
            
            if(isSuccess)
            {
                TempData["IsRedirect"] = true;
                return RedirectToRoute(RouteConstants.TlevelConfirmation, new { id = viewModel.PathwayId });
            }
            else
            {
                _logger.LogWarning(LogEvent.TlevelsNotConfirmed,
                    $"Unable to confirm T level. Method: ConfirmTlevelAsync, Ukprn: {User.GetUkPrn()}, PathwayId: {viewModel.PathwayId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("report-tlevel-issue/{id}", Name=RouteConstants.ReportTlevelIssue)]
        public async Task<IActionResult> ReportIssueAsync(int id)
        {
            var tlevelDetails = await _tlevelLoader.GetQueryTlevelViewModelAsync(User.GetUkPrn(), id);
            if (tlevelDetails == null || (tlevelDetails.PathwayStatusId == (int)TlevelReviewStatus.Queried))
            {
                _logger.LogWarning(LogEvent.TlevelsNotFound,
                    $"Unable to confirm T level. Method: GetQueryTlevelViewModelAsync(Ukprn: {User.GetUkPrn()}, Id: {id}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            tlevelDetails.IsBackToVerifyPage = TempData.Get<bool>(Constants.IsBackToVerifyPage);

            return View(tlevelDetails);
        }

        [HttpPost]
        [Route("report-tlevel-issue", Name = RouteConstants.SubmitTlevelIssue)]
        public async Task<IActionResult> ReportIssueAsync(TlevelQueryViewModel viewModel) 
        {
            if (viewModel == null || viewModel.PathwayStatusId == (int)TlevelReviewStatus.Queried)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            if (!ModelState.IsValid)
            {
                var tlevelDetails = await _tlevelLoader.GetQueryTlevelViewModelAsync(User.GetUkPrn(), viewModel.PathwayId);
                return View(tlevelDetails);
            }

            var isSuccess = await _tlevelLoader.ReportIssueAsync(viewModel);

            if (isSuccess)
            {
                TempData["IsRedirect"] = true;
                return RedirectToRoute(RouteConstants.TlevelConfirmation, new { id = viewModel.PathwayId });
            }
            else
            {
                _logger.LogWarning(LogEvent.TlevelReportIssueFailed,
                    $"Unable to report T level issue. Method: ReportIssueAsync, Ukprn: {User.GetUkPrn()}, TqAwardingOrganisationId: {viewModel.TqAwardingOrganisationId}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
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