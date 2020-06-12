using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.IO;
using System.Text;

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
        public IActionResult UploadRegistrationsFile(UploadRegistrationsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            //_registrationLoader.ProcessBulkRegistrationsAsync(viewModel);

            // loader call - common response
            return RedirectToRoute(RouteConstants.RegistrationsUploadSuccessful);
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
            var viewmodel = new UploadUnsuccessfulViewModel { FileSize = 3, FileType = "CSV" };
            return View(viewmodel);
        }

        [HttpGet]
        [Route("download-registration-errors", Name = RouteConstants.DownloadRegistrationErrors)]
        public IActionResult DownloadRegistrationErrors()
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("Test File"));
            return new FileStreamResult(stream, "text/csv")
            {
                FileDownloadName = "UploadErrors.csv"
            };
        }
    }
}