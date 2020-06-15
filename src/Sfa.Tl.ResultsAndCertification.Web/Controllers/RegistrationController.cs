using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireRegistrationsEditorAccess)]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationLoader _registrationLoader;
        private readonly ILogger _logger;

        public RegistrationController(IRegistrationLoader registrationLoader, ILogger<RegistrationController> logger)
        {
            _registrationLoader = registrationLoader;
            _logger = logger;
        }

        [HttpGet]
        [Route("registrations", Name = RouteConstants.RegistrationDashboard)]
        public IActionResult Index()
        {
            var viewmodel = new DashboardViewModel();
            return View(viewmodel);
        }

        [HttpGet]
        [Route("upload-registrations-file", Name = RouteConstants.UploadRegistrationsFile)]
        public IActionResult UploadRegistrationsFile()
        {
            return View(new UploadRegistrationsRequestViewModel());
        }

        [HttpPost]
        [Route("upload-registrations-file", Name = RouteConstants.SubmitUploadRegistrationsFile)]
        public async Task<IActionResult> UploadRegistrationsFile(UploadRegistrationsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.BlobFileName = $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}";
            viewModel.BlobUniqueReference = Guid.NewGuid();
            viewModel.AoUkprn = (int)User.GetUkPrn();
            var response = await _registrationLoader.ProcessBulkRegistrationsAsync(viewModel);

            if (response.IsSuccess)
            {
                return RedirectToRoute(RouteConstants.RegistrationsUploadSuccessful);
            }
            else
            {
                var unsuccessfulViewModel = new UploadUnsuccessfulViewModel { FileSize = response.ErrorFileSize, FileType = FileType.Csv.ToString().ToUpperInvariant() };
                TempData[Constants.UploadUnsuccessfulViewModel] = JsonConvert.SerializeObject(unsuccessfulViewModel);
                return RedirectToRoute(RouteConstants.RegistrationsUploadUnsuccessful);
            }
        }

        [HttpGet]
        [Route("registrations-upload-successful", Name = RouteConstants.RegistrationsUploadSuccessful)]
        public IActionResult UploadSuccessful()
        {
            return View();
        }

        [HttpGet]
        [Route("registrations-upload-unsuccessful", Name = RouteConstants.RegistrationsUploadUnsuccessful)]
        public IActionResult UploadUnsuccessful()
        {
            if (TempData[Constants.UploadUnsuccessfulViewModel] == null)
            {
                _logger.LogWarning(LogEvent.UploadUnsuccessfulPageFailed,
                    $"Unable to read upload registration response from temp data. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = JsonConvert.DeserializeObject<UploadRegistrationsResponseViewModel>(TempData[Constants.UploadUnsuccessfulViewModel] as string);
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-registration-errors", Name = RouteConstants.DownloadRegistrationErrors)]
        public IActionResult DownloadRegistrationErrors(string id)
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("Test File"));
            return new FileStreamResult(stream, "text/csv")
            {
                FileDownloadName = "Registrations error report.csv"
            };
        }
    }
}