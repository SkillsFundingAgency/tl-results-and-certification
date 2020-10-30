using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class AssessmentController : Controller
    {
        private readonly IAssessmentLoader _assessmentLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AssessmentCacheKey); }
        }

        public AssessmentController(IAssessmentLoader assessmentLoader, ICacheService cacheService, ILogger<AssessmentController> logger)
        {
            _assessmentLoader = assessmentLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("assessment-entries", Name = RouteConstants.AssessmentDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-assessment-entries-file", Name = RouteConstants.UploadAssessmentsFile)]
        public IActionResult UploadAssessmentsFile()
        {
            return View(new UploadAssessmentsRequestViewModel());
        }

        [HttpPost]
        [Route("upload-assessment-entries-file", Name = RouteConstants.SubmitUploadAssessmentsFile)]
        public async Task<IActionResult> UploadAssessmentsFileAsync(UploadAssessmentsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            await Task.Delay(3000); // This is just for test

            viewModel.AoUkprn = User.GetUkPrn();
            var response = new UploadAssessmentsResponseViewModel { IsSuccess = true, Stats = new ViewModel.BulkUploadStatsViewModel { TotalRecordsCount = 20 } }; //await _assessmentLoader.ProcessBulkAssessmentsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AssessmentsUploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AssessmentsUploadSuccessful);
            }
            return RedirectToRoute(RouteConstants.AssessmentsUploadSuccessful);
        }

        [HttpGet]
        [Route("assessment-entries-upload-confirmation", Name = RouteConstants.AssessmentsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(string.Concat(CacheKey, Constants.AssessmentsUploadSuccessfulViewModel));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed, $"Unable to read upload successful assessment response from redis cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }
    }
}
