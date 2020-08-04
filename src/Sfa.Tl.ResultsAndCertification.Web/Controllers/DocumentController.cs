using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using System.Threading.Tasks;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentLoader _documentLoader;
        private readonly ILogger _logger;

        public DocumentController(IDocumentLoader documentLoader, ILogger<RegistrationController> logger)
        {
            _documentLoader = documentLoader;
            _logger = logger;
        }

        [HttpGet]
        [Route("download-registration-data-format-and-rules-guide", Name = RouteConstants.RegistrationDataFormatAndRulesGuide)]
        public IActionResult RegistrationDataFormatAndRulesGuide()
        {
            var viewModel = new RegistrationDataFormatAndRulesGuideViewModel
            {
                FileType = FileType.Xlsx.ToString().ToUpperInvariant(),
                FileSize = DocumentResource.RegistrationDataFormatAndRulesGuide.FileSize_Text,
                Version = DocumentResource.RegistrationDataFormatAndRulesGuide.Version_Text,
                PublishedDate = $"{DocumentResource.RegistrationDataFormatAndRulesGuide.Published_Text} {DocumentResource.RegistrationDataFormatAndRulesGuide.PublishedDate_Text}"
            };
            return View(viewModel);
        }

        [HttpGet]
        [Route("download-registration-data-format-and-rules-guide_file", Name = RouteConstants.DownloadRegistrationDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadRegistrationDataFormatAndRulesGuide()
        {
            var fileName = DocumentResource.RegistrationDataFormatAndRulesGuide.Registrations_Data_And_Format_Rules_Guide_File_Name_Text;
            var fileStream = await _documentLoader.GetBulkUploadRegistrationsTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload registration tech spec document. Method: GetBulkUploadRegistrationsTechSpecFileAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/xlsx")
            {
                FileDownloadName = fileName
            };
        }
    }
}