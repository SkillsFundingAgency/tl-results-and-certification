using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System.Threading.Tasks;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

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
        public async Task<IActionResult> UploadRegistrationsFileAsync(UploadRegistrationsRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.AoUkprn = User.GetUkPrn();
            var response = await _registrationLoader.ProcessBulkRegistrationsAsync(viewModel);

            if (response.IsSuccess)
            {
                return RedirectToRoute(RouteConstants.RegistrationsUploadSuccessful);
            }
            else
            {
                var unsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = response.BlobUniqueReference, FileSize = response.ErrorFileSize, FileType = FileType.Csv.ToString().ToUpperInvariant() };
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

            var viewModel = JsonConvert.DeserializeObject<UploadUnsuccessfulViewModel>(TempData[Constants.UploadUnsuccessfulViewModel] as string);
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-registration-errors", Name = RouteConstants.DownloadRegistrationErrors)]
        public async Task<IActionResult> DownloadRegistrationErrors(string id)
        {
            if (id.IsGuid())
            {
                var fileStream = await _registrationLoader.GetRegistrationValidationErrorsFileAsync(User.GetUkPrn(), id.ToGuid());
                if (fileStream == null)
                {
                    _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download registration validation errors. Method: GetRegistrationValidationErrorsFileAsync(AoUkprn: {User.GetUkPrn()}, BlobUniqueReference = {id})");
                    return RedirectToRoute(RouteConstants.PageNotFound);
                }

                fileStream.Position = 0;
                return new FileStreamResult(fileStream, "text/csv")
                {
                    FileDownloadName = RegistrationContent.UploadUnsuccessful.Registrations_Error_Report_File_Name_Text
                };
            }
            else
            {
                _logger.LogWarning(LogEvent.DownloadRegistrationErrorsFailed, $"Not a valid guid to read file.Method: DownloadRegistrationErrors(Id = { id}), Ukprn: { User.GetUkPrn()}, User: { User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }
    }
}