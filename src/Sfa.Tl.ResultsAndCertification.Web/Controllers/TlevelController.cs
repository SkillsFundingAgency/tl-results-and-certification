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

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireTLevelsReviewerAccess)]
    public class TlevelController : Controller
    {
        private readonly ITlevelLoader _tlevelLoader;
        private readonly ILogger _logger;

        public TlevelController(ITlevelLoader tlevelLoader, ILogger<TlevelController> logger)
        {
            _tlevelLoader = tlevelLoader;
            _logger = logger;
        }

        [Route("tlevels", Name = RouteConstants.Tlevels)]
        public async Task<IActionResult> IndexAsync()
        {
            var pendingTlevels = await _tlevelLoader.GetTlevelsByStatusIdAsync(User.GetUkPrn(), (int)TlevelReviewStatus.AwaitingConfirmation);
            if (pendingTlevels?.Count() > 0)
            {
                return RedirectToRoute(RouteConstants.TlevelSelect);
            }

            return RedirectToRoute(RouteConstants.ViewAllTlevels);
        }

        [Route("view-all-tlevels", Name = RouteConstants.ViewAllTlevels)]
        public async Task<IActionResult> ViewAllAsync()
        {
            var viewModel = await _tlevelLoader.GetYourTlevelsViewModel(User.GetUkPrn());
            
            if (viewModel == null || (!viewModel.ConfirmedTlevels.Any() && !viewModel.QueriedTlevels.Any()))
                return RedirectToRoute(RouteConstants.PageNotFound);
            
            return View(viewModel);
        }

        [Route("tlevel-details/{id}", Name = RouteConstants.TlevelDetails)]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            var viewModel = await _tlevelLoader.GetTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), id);

            if(viewModel == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("tlevel-select", Name = RouteConstants.TlevelSelect)]
        public async Task<IActionResult> SelectToReviewAsync()
        {
            return await GetSelectToReviewByUkprn(User.GetUkPrn());
        }

        [HttpPost]
        [Route("tlevel-select", Name = RouteConstants.TlevelSelect)]
        public async Task<IActionResult> SelectToReviewAsync(SelectToReviewPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await GetSelectToReviewByUkprn(User.GetUkPrn());
            }
                
            return RedirectToRoute(RouteConstants.VerifyTlevel, new { id = model.SelectedPathwayId });
        }

        [HttpGet]
        [Route("verify-tlevel/{id}", Name = RouteConstants.VerifyTlevel)]
        public async Task<IActionResult> VerifyAsync(int id)
        {
            var viewModel = await GetVerifyTlevelData(id);

            if (viewModel == null || viewModel.PathwayStatusId != (int)TlevelReviewStatus.AwaitingConfirmation)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("tlevel-confirmation/{id}", Name = RouteConstants.TlevelConfirmation)]
        public async Task<IActionResult> ConfirmationAsync(int id)
        {
            if (id == 0 || TempData[Constants.IsRedirect] == null || !(bool.TryParse(TempData[Constants.IsRedirect].ToString(), out bool isRedirect) && isRedirect))
            {
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
                return RedirectToRoute(RouteConstants.ReportTlevelIssue, new { id = viewModel.PathwayId });

            var isSuccess = await _tlevelLoader.ConfirmTlevelAsync(viewModel);
            
            if(isSuccess)
            {
                TempData["IsRedirect"] = true;
                return RedirectToRoute(RouteConstants.TlevelConfirmation, new { id = viewModel.PathwayId });
            }
            else
            {
                return RedirectToRoute("error/500");
            }
        }

        [HttpGet]
        [Route("report-tlevel-issue/{id}", Name=RouteConstants.ReportTlevelIssue)]
        public async Task<IActionResult> ReportIssueAsync(int id)
        {
            var tlevelDetails = await _tlevelLoader.GetQueryTlevelViewModelAsync(User.GetUkPrn(), id);
            if (tlevelDetails == null || (tlevelDetails.PathwayStatusId == (int)TlevelReviewStatus.Queried))
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
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
                return RedirectToRoute("error/500");
            }
        }

        private async Task<ConfirmTlevelViewModel> GetVerifyTlevelData(int pathwayId)
        {
            return await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), pathwayId);
        }

        private async Task<IActionResult> GetSelectToReviewByUkprn(long ukPrn)
        {
            var viewModel = await _tlevelLoader.GetTlevelsToReviewByUkprnAsync(ukPrn);

            if (viewModel.TlevelsToReview?.Count() == 0)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }
    }
}