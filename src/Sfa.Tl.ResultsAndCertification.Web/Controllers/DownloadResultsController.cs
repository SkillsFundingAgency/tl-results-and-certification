using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.DownloadResults;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class DownloadResultsController : Controller
    {
        [HttpGet]
        [Route("download-tlevel-results", Name = RouteConstants.DownloadTlevelResults)]
        public IActionResult DownloadTlevelResults()
        {
            var viewModel = new DownloadTlevelResultsViewModel();
            return View(viewModel);
        }
    }
}