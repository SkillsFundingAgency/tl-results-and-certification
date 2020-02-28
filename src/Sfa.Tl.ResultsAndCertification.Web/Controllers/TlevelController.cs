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

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireTLevelsReviewerAccess)]
    public class TlevelController : Controller
    {
        private readonly ITlevelLoader _tlevelLoader;

        public TlevelController(ITlevelLoader tlevelLoader)
        {
            _tlevelLoader = tlevelLoader;
        }

        public async Task<IActionResult> Index()
        {
            var pendingTlevels = await _tlevelLoader.GetTlevelsByStatusIdAsync(User.GetUkPrn(), (int)TlevelReviewStatus.AwaitingConfirmation);
            if (pendingTlevels?.Count() > 0)
            {
                return RedirectToAction(nameof(SelectToReview));
            }

            return RedirectToAction(nameof(ViewAll));
        }

        public async Task<IActionResult> ViewAll()
        {
            var viewModel = await _tlevelLoader.GetAllTlevelsByUkprnAsync(User.GetUkPrn());
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _tlevelLoader.GetTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), id);

            if(viewModel == null)
            {
                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.ErrorController);
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SelectToReview()
        {
            var viewModel = await _tlevelLoader.GetTlevelsToReviewByUkprnAsync(User.GetUkPrn());
            
            if (viewModel.TlevelsToReview?.Count() == 0)
                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.ErrorController);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectToReview(SelectToReviewPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                
            return await Task.Run(() => RedirectToRoute(RouteConstants.VerifyTlevel, new { id = model.SelectedPathwayId }));
        }

        public async Task<IActionResult> ReportIssue()
        {
            return await Task.Run(() => View());
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
            return View("Verify", viewModel);
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
            
            return View("Confirmation", confirmationViewModel);
        }

        [HttpPost]
        [Route("confirm-tlevel", Name = RouteConstants.ConfirmTlevel)]
        public async Task<IActionResult> ConfirmTlevel(VerifyTlevelViewModel viewModel)
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

        private async Task<VerifyTlevelViewModel> GetVerifyTlevelData(int pathwayId)
        {
            return await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), pathwayId);
        }
    }
}