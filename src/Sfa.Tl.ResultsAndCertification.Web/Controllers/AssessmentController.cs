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
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;


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

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _assessmentLoader.ProcessBulkAssessmentsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.AssessmentsUploadSuccessfulViewModel), successfulViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.AssessmentsUploadSuccessful);
            }

            if (response.ShowProblemWithServicePage)
                return RedirectToRoute(RouteConstants.ProblemWithService);

            var unsuccessfulViewModel = new ViewModel.Registration.UploadUnsuccessfulViewModel 
            { 
                BlobUniqueReference = response.BlobUniqueReference, 
                FileSize = response.ErrorFileSize, 
                FileType = FileType.Csv.ToString().ToUpperInvariant() 
            };

            await _cacheService.SetAsync(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel), unsuccessfulViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.AssessmentsUploadUnsuccessful);
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

        [HttpGet]
        [Route("assessment-entries-upload-unsuccessful", Name = RouteConstants.AssessmentsUploadUnsuccessful)]
        public async Task<IActionResult> UploadUnsuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ViewModel.Registration.UploadUnsuccessfulViewModel>(string.Concat(CacheKey, Constants.UploadUnsuccessfulViewModel));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-assessment-errors", Name = RouteConstants.DownloadAssessmentErrors)]
        public async Task<IActionResult> DownloadssessmentErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _assessmentLoader.GetAssessmentValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download assessment validation errors. Method: GetAssessmentValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = AssessmentContent.UploadUnsuccessful.Assessment_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadAssesssmentErrorsFailed, $"Not a valid guid to read file.Method: DownloadAssessmentErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }
    }
}
