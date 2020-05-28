using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegistrationLoader _registrationLoader;
        private readonly ILogger _logger;

        public RegistrationController(IRegistrationLoader registrationLoader, ILogger<ProviderController> logger)
        {
            _registrationLoader = registrationLoader;
            _logger = logger;
        }

        [HttpGet]
        [Route("student-registrations", Name = RouteConstants.RegistrationDashboard)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("upload-registrations-file", Name = RouteConstants.UploadRegistrationsFile)]
        public IActionResult UploadRegistrationsFile()
        {
            return View();
        }

        [HttpPost]
        [Route("upload-registrations-file", Name = RouteConstants.SubmitUploadRegistrationsFile)]
        public IActionResult UploadRegistrationsFile(UploadRegistrationsFileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            return View();
        }
    }
}