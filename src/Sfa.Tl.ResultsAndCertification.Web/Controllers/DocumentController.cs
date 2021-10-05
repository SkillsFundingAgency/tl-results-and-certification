using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using System.Threading.Tasks;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class DocumentController : Controller
    {
        private readonly IDocumentLoader _documentLoader;
        private readonly ILogger _logger;

        public DocumentController(IDocumentLoader documentLoader, ILogger<DocumentController> logger)
        {
            _documentLoader = documentLoader;
            _logger = logger;
        }        

        [HttpGet]
        [Route("tlevels-registration-data-format-and-rules", Name = RouteConstants.DownloadRegistrationDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadRegistrationDataFormatAndRulesGuideAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Registrations_Data_Format_And_Rules_File_Name;
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

        [HttpGet]
        [Route("tlevels-assessment-entries-data-format-and-rules", Name = RouteConstants.DownloadAssessmentEntriesDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadAssessmentEntriesDataFormatAndRulesGuideAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Assessment_Entry_Data_Format_And_Rules_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadAssessmentEntriesTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload assessment entries tech spec document. Method: GetBulkUploadAssessmentEntriesTechSpecFileAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/xlsx")
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-results-data-format-and-rules", Name = RouteConstants.DownloadResultsDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadResultsDataFormatAndRulesGuideAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Results_Data_Format_And_Rules_File_Name;
            var fileStream = await _documentLoader.GetTechSpecFileAsync(BlobStorageConstants.ResultsFolderName, fileName);
            if (fileStream == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "text/xlsx")
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevel-data-format-and-rules-guides", Name = RouteConstants.TlevelDataFormatAndRulesGuide)]
        public IActionResult TlevelDataFormatAndRulesGuide()
        {
            var viewModel = new TlevelDataFormatAndRulesGuideViewModel
            {
                FileType = FileType.Xlsx.ToString().ToUpperInvariant(),
                
                RegistrationsFileSize = DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_FileSize_Text,
                RegistrationsVersion = DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_Version_Text,
                RegistrationsPublishedDate = $"{DocumentResource.TlevelDataFormatAndRulesGuide.Published_Text} {DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_PublishedDate_Text}",
                
                AssessmentEntriesFileSize = DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_FileSize_Text,
                AssessmentEntriesVersion = DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_Version_Text,
                AssessmentEntriesPublishedDate = $"{DocumentResource.TlevelDataFormatAndRulesGuide.Published_Text} {DocumentResource.TlevelDataFormatAndRulesGuide.Assessment_Entries_PublishedDate_Text}",

                ResultsFileSize = DocumentResource.TlevelDataFormatAndRulesGuide.Results_FileSize_Text,
                ResultsVersion = DocumentResource.TlevelDataFormatAndRulesGuide.Results_Version_Text,
                ResultsPublishedDate = $"{DocumentResource.TlevelDataFormatAndRulesGuide.Published_Text} {DocumentResource.TlevelDataFormatAndRulesGuide.Results_PublishedDate_Text}",
            };

            return View(viewModel);
        }        
    }
}