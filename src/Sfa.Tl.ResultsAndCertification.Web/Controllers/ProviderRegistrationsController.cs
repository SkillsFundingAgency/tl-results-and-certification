using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class ProviderRegistrationsController : Controller
    {
        private readonly IProviderRegistrationsLoader _loader;
        private readonly ILogger _logger;

        public ProviderRegistrationsController(
            IProviderRegistrationsLoader loader,
            ILogger<ProviderRegistrationsController> logger)
        {
            _loader = loader;
            _logger = logger;
        }

        [HttpGet]
        [Route("download-registrations-data", Name = RouteConstants.DownloadRegistrationsData)]
        public async Task<IActionResult> DownloadRegistrationsDataAsync()
        {
            IList<AvailableStartYearViewModel> availableStartYearViewModels = await _loader.GetAvailableStartYearsAsync();
            DownloadRegistrationsDataViewModel viewModel = new()
            {
                AvailableStartYearViewModels = availableStartYearViewModels
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("download-registrations-data-for", Name = RouteConstants.DownloadRegistrationsDataFor)]
        public async Task<IActionResult> DownloadRegistrationsDataForAsync(int startYear)
        {
            long ukprn = User.GetUkPrn();
            string email = User.GetUserEmail();

            DataExportResponse dataExportResponse = await _loader.GetRegistrationsDataExportAsync(ukprn, startYear, email);

            if (dataExportResponse == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            if (!dataExportResponse.IsDataFound)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"There are no registrations found for the data export. Method: GetRegistrationsDataExportAsync({ukprn}, {email})");
                return RedirectToRoute(RouteConstants.RegistrationsNoRecordsFound);
            }

            DownloadRegistrationsDataForViewModel viewModel = _loader.GetDownloadRegistrationsDataForViewModel(dataExportResponse, startYear);
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-registrations-data-for/{id}", Name = RouteConstants.DownloadRegistrationsDataForLink)]
        public async Task<IActionResult> RegistrationsDownloadDataLinkAsync(string id)
        {
            if (!id.IsGuid())
            {
                _logger.LogWarning(LogEvent.DocumentDownloadFailed, $"Not a valid guid to read file.Method: RegistrationsDownloadDataLinkAsync (Id = {id}), Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }

            return await _loader.GetRegistrationsFileAsync(User.GetUkPrn(), id.ToGuid());
        }
    }
}