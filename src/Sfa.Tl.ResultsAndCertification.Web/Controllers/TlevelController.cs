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
                
            return await Task.Run(() => RedirectToAction(nameof(Verify), new { id = model.SelectedPathwayId }));
        }

        public async Task<IActionResult> ReportIssue()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Verify(int id)
        {
            var viewModel = await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), id);

            if (viewModel == null || viewModel.PathwayStatusId != (int)TlevelReviewStatus.AwaitingConfirmation)
            {
                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.ErrorController);
            }
            return View(viewModel);
        }

        [HttpPost]
        [Route("confirm-tlevel", Name = "ConfirmTlevel")]
        public async Task<IActionResult> ConfirmTlevel(VerifyTlevelViewModel viewModel)
        {
            if (viewModel == null || viewModel.PathwayStatusId != (int)TlevelReviewStatus.AwaitingConfirmation)
            {
                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.ErrorController);
            }

            if (!ModelState.IsValid)
            {
                var model = await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), viewModel.PathwayId);
                return View("Verify", model);
            }

            var result = await _tlevelLoader.ConfirmTlevelAsync(viewModel);
            
            return RedirectToAction("Details", new { id = viewModel.PathwayId });
        }
    }
}