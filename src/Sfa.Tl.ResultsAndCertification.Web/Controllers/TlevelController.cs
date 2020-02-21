using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

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
            // TODO: Change below to new endpoint get by status rather all Tlevels status. 
            var viewModel = await _tlevelLoader.GetAllTlevelsByUkprnAsync(User.GetUkPrn());

            var anyReviewPending = viewModel.Any(x => x.StatusId == (int)TlevelReviewStatus.AwaitingConfirmation);
            if (anyReviewPending)
            {
                return RedirectToAction("SelectToReview");
            }
            
            return RedirectToAction("ViewAll");
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

        public async Task<IActionResult> SelectToReview()
        {
            var viewModel = await _tlevelLoader.GetTlevelsToReviewByUkprnAsync(User.GetUkPrn());
            return View(viewModel);
        }

        public async Task<IActionResult> ReportIssue()
        {
            return await Task.Run(() => View());
        }

        public async Task<IActionResult> Verify(int id)
        {
            var viewModel = await _tlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(HttpContext.User.GetUkPrn(), id);

            if (viewModel == null)
            {
                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.ErrorController);
            }
            return View(viewModel);
        }
    }
}