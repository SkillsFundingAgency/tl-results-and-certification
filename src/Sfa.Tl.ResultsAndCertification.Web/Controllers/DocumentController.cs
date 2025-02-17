﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Document;
using System.IO;
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
            return new FileStreamResult(fileStream, Constants.TextXlsx)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-registration-data-template", Name = RouteConstants.DownloadRegistrationDataTemplate)]
        public async Task<IActionResult> DownloadRegistrationDataTemplateAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Registration_Data_Template_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadRegistrationsTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload registration tech spec document. Method: GetBulkUploadRegistrationsTechSpecFileAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextCsv)
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
            return new FileStreamResult(fileStream, Constants.TextXlsx)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-assessment-entries-template", Name = RouteConstants.DownloadAssessmentEntriesTemplate)]
        public async Task<IActionResult> DownloadAssessmentEntriesTemplateAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Assessment_Entry_Data_Template_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadAssessmentEntriesTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload assessment entries tech spec document. Method: GetBulkUploadAssessmentEntriesTechSpecFileAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextCsv)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-results-data-format-and-rules", Name = RouteConstants.DownloadResultsDataFormatAndRulesGuide)]
        public Task<IActionResult> DownloadResultsDataFormatAndRulesGuideAsync()
            => DownloadTechSpecFileAsync(DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Results_Data_Format_And_Rules_File_Name, Constants.TextXlsx);

        [HttpGet]
        [Route("tlevels-results-template", Name = RouteConstants.DownloadResultsTemplate)]
        public Task<IActionResult> DownloadResultsTemplateAsync()
            => DownloadTechSpecFileAsync(DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Results_Template_File_Name, Constants.TextCsv);

        private async Task<IActionResult> DownloadTechSpecFileAsync(string fileName, string contentType)
        {
            Stream fileStream = await _documentLoader.GetTechSpecFileAsync(BlobStorageConstants.ResultsFolderName, fileName);

            if (fileStream == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevel-data-format-and-rules-guides", Name = RouteConstants.TlevelDataFormatAndRulesGuide)]
        public IActionResult TlevelDataFormatAndRulesGuide()
        {
            return View(new TlevelDataFormatAndRulesGuideViewModel());
        }


        [HttpGet]
        [Route("tlevels-withdrawals-data-format-and-rules", Name = RouteConstants.DownloadWithdrawalsDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadWithdrawalDataFormatAndRulesGuideAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Withdrawals_Data_Format_And_Rules_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadWithdrawalsTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload withdrawal tech spec document. Method: DownloadWithdrawlDataFormatAndRulesGuideAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextXlsx)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-withdrawal-data-template", Name = RouteConstants.DownloadWithdrawalsDataTemplate)]
        public async Task<IActionResult> DownloadWithdrawalnDataTemplateAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Withdrawal_Data_Template_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadWithdrawalsTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload withdrawal tech spec document. Method: DownloadWithdrawlnDataTemplateAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextCsv)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-romm-data-format-and-rules", Name = RouteConstants.DownloadRommDataFormatAndRulesGuide)]
        public async Task<IActionResult> DownloadRommDataFormatAndRulesGuideAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Romm_Data_Format_And_Rules_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadRommTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload romm tech spec document. Method: DownloadRommDataFormatAndRulesGuideAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextXlsx)
            {
                FileDownloadName = fileName
            };
        }

        [HttpGet]
        [Route("tlevels-romm-data-template", Name = RouteConstants.DownloadRommDataTemplate)]
        public async Task<IActionResult> DownloadRommDataTemplateAsync()
        {
            var fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Romm_Data_Template_File_Name;
            var fileStream = await _documentLoader.GetBulkUploadRommTechSpecFileAsync(fileName);
            if (fileStream == null)
            {
                _logger.LogWarning(LogEvent.FileStreamNotFound, $"No FileStream found to download bulk upload romm tech spec document. Method: DownloadRommDataTemplateAsync(FileName: {fileName})");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            fileStream.Position = 0;
            return new FileStreamResult(fileStream, Constants.TextCsv)
            {
                FileDownloadName = fileName
            };
        }
    }
}