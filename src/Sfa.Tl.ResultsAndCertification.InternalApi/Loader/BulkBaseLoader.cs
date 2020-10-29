using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkBaseLoader : IBulkBaseLoader
    {
        private readonly ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> _csvService; 
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDocumentUploadHistoryService _documentUploadHistoryService;

        public BulkBaseLoader(ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> csvService, 
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService)
        {
            _documentUploadHistoryService = documentUploadHistoryService;
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> DeleteFileFromProcessingFolderAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.DeleteFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkProcessStatus.Processing}",
                BlobFileName = request.BlobFileName
            });
            return true;
        }

        public async Task<byte[]> CreateErrorFileAsync(IList<RegistrationValidationError> validationErrors)
        {
            return await _csvService.WriteFileAsync(validationErrors);
        }

        public async Task<bool> UploadErrorsFileToBlobStorage(BulkRegistrationRequest request, byte[] errorFile)
        {
            if (errorFile == null || errorFile.Length == 0) return false;
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkProcessStatus.ValidationErrors}",
                BlobFileName = request.BlobFileName,
                UserName = request.PerformedBy,
                FileData = errorFile
            });

            return true;
        }

        public async Task<bool> MoveFileFromProcessingToFailedAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.MoveFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkProcessStatus.Processing}",
                BlobFileName = request.BlobFileName,
                DestinationFilePath = $"{request.AoUkprn}/{BulkProcessStatus.Failed}"
            });
            return true;
        }

        public async Task<bool> CreateDocumentUploadHistory(BulkRegistrationRequest request, DocumentUploadStatus status = DocumentUploadStatus.Processed)
        {
            if (request == null) return false;

            var model = new DocumentUploadHistoryDetails
            {
                AoUkprn = request.AoUkprn,
                BlobFileName = request.BlobFileName,
                BlobUniqueReference = request.BlobUniqueReference,
                DocumentType = (int)request.DocumentType,
                FileType = (int)request.FileType,
                Status = (int)status,
                CreatedBy = request.PerformedBy
            };

            return await _documentUploadHistoryService.CreateDocumentUploadHistory(model);
        }
    }
}
