using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;

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
    }
}
