using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireResultsEditorAccess)]
    public class ResultController : Controller
    {
        private readonly IResultLoader _resultLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        public ResultController(IResultLoader resultLoader, ICacheService cacheService, ILogger<ResultController> logger)
        {
            _resultLoader = resultLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("results", Name = RouteConstants.ResultsDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-results-file/{requestErrorTypeId:int?}", Name = RouteConstants.UploadResultsFile)]
        public IActionResult UploadResultsFile(int? requestErrorTypeId)
        {
            var model = new UploadResultsRequestViewModel { RequestErrorTypeId = requestErrorTypeId };
            model.SetAnyModelErrors(ModelState);
            return View(model);
        }

        [HttpPost]
        [Route("upload-results-file", Name = RouteConstants.SubmitUploadResultsFile)]
        public async Task<IActionResult> UploadResultsFileAsync(UploadResultsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _resultLoader.ProcessBulkResultsAsync(viewModel);

            // TODO: refine in upcoming stories
            if (response.IsSuccess)
                return RedirectToRoute(RouteConstants.ResultsUploadSuccessful);
            else
                return RedirectToRoute(RouteConstants.ResultsUploadUnsuccessful);
        }

        [HttpGet]
        [Route("results-upload-successful", Name = RouteConstants.ResultsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            return View();
        }

        [HttpGet]
        [Route("results-upload-unsuccessful", Name = RouteConstants.ResultsUploadUnsuccessful)]
        public async Task<IActionResult> UploadUnsuccessful()
        {
            return View();
        }

        [HttpGet]
        [Route("results-learner-search", Name = RouteConstants.SearchResults)]
        public async Task<IActionResult> SearchResultsAsync()
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(Constants.ResultsSearchCriteria);
            var viewModel = new SearchResultsViewModel { SearchUln = defaultValue };
            return View(viewModel);
        }

        [HttpPost]
        [Route("results-learner-search", Name = RouteConstants.SubmitSearchResults)]
        public IActionResult SearchResultsAsync(SearchResultsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return View(model);
        }
    }
}