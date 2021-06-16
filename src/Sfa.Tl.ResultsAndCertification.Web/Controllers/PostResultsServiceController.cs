using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    //[Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)] // TODO
    public class PostResultsServiceController : Controller
    {

        [HttpGet]
        [Route("reviews-and-appeals", Name = RouteConstants.StartReviewAndAppeals)]
        public IActionResult StartReviewsAndAppealsAsync()
        {
            return View(new StartReviewsAndAppealsViewModel());
        }

        [HttpGet]
        [Route("reviews-and-appeals-search-learner", Name = RouteConstants.SearchPostResultsService)]
        public async Task<IActionResult> SearchPostResultsServiceAsync()
        {
            await Task.CompletedTask;
            return View(new SearchPostResultsServiceViewModel());
        }
    }
}
