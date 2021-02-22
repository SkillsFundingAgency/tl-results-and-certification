using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class DocumentLoader : IDocumentLoader
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<DocumentLoader> _logger;

        public DocumentLoader(ILogger<DocumentLoader> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        public async Task<Stream> GetBulkUploadRegistrationsTechSpecFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Documents.ToString(),
                BlobFileName = fileName,
                SourceFilePath = $"{BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.RegistrationsFolderName}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download bulkupload registration tech spec. Method: DownloadFileAsync(ContainerName: {DocumentType.Documents}, BlobFileName = {fileName}, SourceFilePath = {BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.RegistrationsFolderName})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }

        public async Task<Stream> GetBulkUploadAssessmentEntriesTechSpecFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Documents.ToString(),
                BlobFileName = fileName,
                SourceFilePath = $"{BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.AssessmentsFolderName}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download bulkupload assessment entries tech spec. Method: DownloadFileAsync(ContainerName: {DocumentType.Documents}, BlobFileName = {fileName}, SourceFilePath = {BlobStorageConstants.TechSpecFolderName}/{BlobStorageConstants.RegistrationsFolderName})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }

        public async Task<Stream> GetTechSpecFileAsync(string folderName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folderName) || string.IsNullOrWhiteSpace(fileName))
                return null;

            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Documents.ToString(),
                BlobFileName = fileName,
                SourceFilePath = $"{BlobStorageConstants.TechSpecFolderName}/{folderName}"
            });

            if (fileStream == null)
            {
                var blobReadError = $"No FileStream found to download tech spec. Method: DownloadFileAsync(ContainerName: {DocumentType.Documents}, BlobFileName = {fileName}, SourceFilePath = {BlobStorageConstants.TechSpecFolderName}/{folderName})";
                _logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
            }
            return fileStream;
        }
    }
}
