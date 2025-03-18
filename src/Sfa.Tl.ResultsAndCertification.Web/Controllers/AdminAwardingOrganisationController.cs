using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.FileResult;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminAwardingOrganisationController : Controller
    {
        private readonly IAdminAwardingOrganisationLoader _loader;
        private readonly IResultLoader _resultLoader;
        private readonly IPostResultsServiceLoader _postResultsServiceLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminAwardingOrganisationController> _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminAwardingOrganisationCacheKey);

        public AdminAwardingOrganisationController(
            IAdminAwardingOrganisationLoader loader,
            IResultLoader resultLoader,
            IPostResultsServiceLoader postResultsServiceLoader,
            ICacheService cacheService,
            ILogger<AdminAwardingOrganisationController> logger)
        {
            _loader = loader;
            _resultLoader = resultLoader;
            _postResultsServiceLoader = postResultsServiceLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/select-awarding-organisation-clear", Name = RouteConstants.AdminSelectAwardingOrganisationClear)]
        public async Task<IActionResult> AdminSelectAwardingOrganisationClearAsync()
        {
            await _cacheService.RemoveAsync<AdminSelectAwardingOrganisationViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminSelectAwardingOrganisation);
        }

        [HttpGet]
        [Route("admin/select-awarding-organisation", Name = RouteConstants.AdminSelectAwardingOrganisation)]
        public async Task<IActionResult> AdminSelectAwardingOrganisationAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminSelectAwardingOrganisationViewModel>(CacheKey);

            if (viewModel == null)
            {
                viewModel = await _loader.GetSelectAwardingOrganisationViewModelAsync();
                await _cacheService.SetAsync(CacheKey, viewModel);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/select-awarding-organisation", Name = RouteConstants.AdminSubmitSelectAwardingOrganisation)]
        public async Task<IActionResult> AdminSelectAwardingOrganisationAsync(AdminSelectAwardingOrganisationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminDownloadResultsRommsByAwardingOrganisation, new { awardingOrganisationUkprn = viewModel.SelectedAwardingOrganisationUkprn });
        }

        [HttpGet]
        [Route("admin/download-results-romms-awarding-organisation/{awardingOrganisationUkprn}", Name = RouteConstants.AdminDownloadResultsRommsByAwardingOrganisation)]
        public async Task<IActionResult> AdminDownloadResultsRommsByAwardingOrganisationAsync(long awardingOrganisationUkprn)
        {
            Task<string> displayNameTask = _loader.GetAwardingOrganisationDisplayName(awardingOrganisationUkprn);
            Task<IList<DataExportResponse>> resultsTask = _resultLoader.GenerateResultsExportAsync(awardingOrganisationUkprn, User.GetUserEmail());
            Task<IList<DataExportResponse>> rommsTask = _postResultsServiceLoader.GenerateRommsDataExportAsync(awardingOrganisationUkprn, User.GetUserEmail());

            await Task.WhenAll(displayNameTask, resultsTask, rommsTask);

            var viewModel = new AdminDownloadResultsRommsByAwardingOrganisationViewModel
            {
                AwardingOrganisationUkprn = awardingOrganisationUkprn,
                AwardingOrganisationDisplayName = displayNameTask.Result
            };

            IList<DataExportResponse> results = resultsTask.Result;
            IList<DataExportResponse> romms = rommsTask.Result;

            if (results.IsNullOrEmpty() || results.Any(r => r.ComponentType == ComponentType.NotSpecified) || !romms.ContainsSingle())
                return RedirectToRoute(RouteConstants.ProblemWithService);

            foreach (DataExportResponse currentResult in results.Where(r => r.IsDataFound))
            {
                var downloadViewModel = CreateDownloadLink(currentResult);

                switch (currentResult.ComponentType)
                {
                    case ComponentType.Core:
                        viewModel.CoreResultsDownloadLinkViewModel = downloadViewModel;
                        break;
                    case ComponentType.Specialism:
                        viewModel.SpecialismResultsDownloadLinkViewModel = downloadViewModel;
                        break;
                }
            }

            DataExportResponse romm = romms.Single();
            if (romm.IsDataFound)
            {
                viewModel.RommsDownloadLinkViewModel = CreateDownloadLink(romm);
            }

            return View(viewModel);
        }

        private static DownloadLinkViewModel CreateDownloadLink(DataExportResponse response) => new()
        {
            BlobUniqueReference = response.BlobUniqueReference,
            FileSize = response.FileSize,
            FileType = FileType.Csv.ToString().ToUpperInvariant()
        };

        [HttpGet]
        [Route("admin/download-core-results/{awardingOrganisationUkprn}/{fileId}", Name = RouteConstants.AdminDownloadCoreResultsDataLink)]
        public Task<IActionResult> AdminDownloadCoreResultsDataLinkAsync(long awardingOrganisationUkprn, string fileId)
            => AdminDownloadDataLinkAsync(
                awardingOrganisationUkprn,
                fileId,
                () => _resultLoader.GetResultsDataFileAsync(awardingOrganisationUkprn, fileId.ToGuid(), ComponentType.Core),
                AdminDownloadResultsRommsByAwardingOrganisation.Core_Results_Download_FileName,
                nameof(AdminDownloadCoreResultsDataLinkAsync));

        [HttpGet]
        [Route("admin/download-specialism-results/{awardingOrganisationUkprn}/{fileId}", Name = RouteConstants.AdminDownloadSpecialismResultsDataLink)]
        public Task<IActionResult> AdminDownloadSpecialismResultsDataLinkAsync(long awardingOrganisationUkprn, string fileId)
            => AdminDownloadDataLinkAsync(
                awardingOrganisationUkprn,
                fileId,
                () => _resultLoader.GetResultsDataFileAsync(awardingOrganisationUkprn, fileId.ToGuid(), ComponentType.Specialism),
                AdminDownloadResultsRommsByAwardingOrganisation.Specialism_Results_Download_FileName,
                nameof(AdminDownloadSpecialismResultsDataLinkAsync));

        [HttpGet]
        [Route("admin/download-romms-data/{awardingOrganisationUkprn}/{fileId}", Name = RouteConstants.AdminDownloadRommsDataLink)]
        public Task<IActionResult> AdminDownloadRommsDataLinkAsync(long awardingOrganisationUkprn, string fileId)
            => AdminDownloadDataLinkAsync(
                awardingOrganisationUkprn,
                fileId,
                () => _postResultsServiceLoader.GetRommsDataFileAsync(awardingOrganisationUkprn, fileId.ToGuid()),
                AdminDownloadResultsRommsByAwardingOrganisation.Romms_Data_Report_File_Name_Text,
                nameof(AdminDownloadRommsDataLinkAsync));

        private async Task<IActionResult> AdminDownloadDataLinkAsync(long awardingOrganisationUkprn, string fileId, Func<Task<Stream>> getFileStream, string fileDownloadName, string methodName)
        {
            if (!fileId.IsGuid())
            {
                _logger.LogWarning(LogEvent.DocumentDownloadFailed, $"Not a valid GUID to read file. Method: {methodName}(Id = {fileId}), AoUkprn: {awardingOrganisationUkprn}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            var fileStream = await getFileStream();
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No file stream found to download ROMM data. Method: {methodName}(AoUkprn: {awardingOrganisationUkprn}, BlobUniqueReference: {fileId})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new CsvFileStreamResult(fileStream, fileDownloadName);
        }
    }
}