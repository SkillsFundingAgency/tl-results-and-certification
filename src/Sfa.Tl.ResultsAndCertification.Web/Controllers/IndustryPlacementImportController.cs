using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using IndustryPlacementContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class IndustryPlacementImportController : Controller
    {
        private readonly IIndustryPlacementLoader _industryPlacementLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.IpCacheKey); }
        }

        public IndustryPlacementImportController(IIndustryPlacementLoader industryPlacementLoader, ICacheService cacheService, ILogger<IndustryPlacementImportController> logger)
        {
            _industryPlacementLoader = industryPlacementLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("upload-industry-placements-file/{requestErrorTypeId:int?}", Name = RouteConstants.UploadIndustryPlacementsFile)]
        public IActionResult UploadIndustryPlacementsFile(int? requestErrorTypeId)
        {
            var model = new UploadIndustryPlacementsRequestViewModel { RequestErrorTypeId = requestErrorTypeId };
            model.SetAnyModelErrors(ModelState);
            return View(model);
        }

        [HttpPost]
        [Route("upload-industry-placements-file", Name = RouteConstants.SubmitUploadIndustryPlacementsFile)]
        public async Task<IActionResult> UploadIndustryPlacementsFileAsync(UploadIndustryPlacementsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _industryPlacementLoader.ProcessBulkIndustryPlacementsAsync(viewModel);

            if (response.IsSuccess)
            {
                var successfulViewModel = new UploadSuccessfulViewModel { Stats = response.Stats };
                await _cacheService.SetAsync(CacheKey, successfulViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.IndustryPlacementsUploadSuccessful);
            }

            if (response.ShowProblemWithServicePage)
                return RedirectToRoute(RouteConstants.ProblemWithAssessmentsUpload);

            var unsuccessfulViewModel = new UploadUnsuccessfulViewModel
            {
                BlobUniqueReference = response.BlobUniqueReference,
                FileSize = response.ErrorFileSize,
                FileType = FileType.Csv.ToString().ToUpperInvariant()
            };

            await _cacheService.SetAsync(CacheKey, unsuccessfulViewModel, CacheExpiryTime.XSmall);
            return RedirectToRoute(RouteConstants.IndustryPlacementsUploadUnsuccessful);
        }

        [HttpGet]
        [Route("upload-ip-file-success", Name = RouteConstants.IndustryPlacementsUploadSuccessful)]
        public async Task<IActionResult> UploadSuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadSuccessfulViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadSuccessfulPageFailed, $"Unable to read upload successful industry placement response from redis cache. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("ip-upload-unsuccessful", Name = RouteConstants.IndustryPlacementsUploadUnsuccessful)]
        public async Task<IActionResult> UploadUnsuccessful()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(CacheKey);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload unsuccessful industry placement response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-industry-placement-errors", Name = RouteConstants.DownloadIndustryPlacementErrors)]
        public async Task<IActionResult> DownloadIndustryPlacementErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _industryPlacementLoader.GetIndustryPlacementValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download industry placement validation errors. Method: GetAssessmentValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = IndustryPlacementContent.UploadUnsuccessful.Industry_Placements_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadAssesssmentErrorsFailed, $"Not a valid guid to read file.Method: DownloadIndustryPlacementErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }
    }
}
