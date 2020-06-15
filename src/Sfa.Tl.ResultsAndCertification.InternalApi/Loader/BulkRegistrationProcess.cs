using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkRegistrationProcess : IBulkRegistrationProcess
    {
        private readonly ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> _csvService;
        private readonly IRegistrationService _registrationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDocumentUploadHistoryService _documentUploadHistoryService;
        private readonly ILogger<BulkRegistrationProcess> _logger;

        public BulkRegistrationProcess(ICsvHelperService<RegistrationCsvRecordRequest,
            CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> csvService,
            IRegistrationService registrationService, IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, ILogger<BulkRegistrationProcess> logger)
        {
            _csvService = csvService;
            _registrationService = registrationService;
            _blobStorageService = blobStorageService;
            _documentUploadHistoryService = documentUploadHistoryService;
            _logger = logger;
        }

        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            var response = new BulkRegistrationResponse();

            CsvResponseModel<RegistrationCsvRecordResponse> csvResponse = null;

            // Step: 1 Read file from Blob
            using (var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                BlobFileName = request.BlobFileName,
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                UserName = request.PerformedBy
            }))
            {
                if (fileStream == null)
                {
                    _logger.LogInformation(LogEvent.FileStreamNotFound, $"No FileStream found to process bluk registrations. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}");
                    return response; // need to handle response when null
                }

                // Stage 2 validation
                csvResponse = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = fileStream });
            }

            if (csvResponse.IsDirty || csvResponse.Rows.Any(x => !x.IsValid))
            {
                byte[] errorFile = await CreateErrorFileAsync(csvResponse);
                await UploadErrorsFileToBlobStorage(request, errorFile);
                await MoveFileFromProcessingToFailedAsync(request);
                await CreateDocumentUploadHistory(request, DocumentUploadStatus.Failed);
                return response;
            }

            // Stage 3 valiation. 
            await _registrationService.ValidateRegistrationTlevelsAsync(csvResponse.Rows.Where(x => x.IsValid));
            if (csvResponse.Rows.Any(x => !x.IsValid))
            {
                byte[] errorFile = await CreateErrorFileAsync(csvResponse);
                // Todo: blob operation
                return response;
            }

            // Step: Map data to DB model type.
            var tqRegistrations = _registrationService.TransformRegistrationModel();

            // Step: Process DB operation
            var result = await _registrationService.CompareAndProcessRegistrations();

            return response;
        }

        private async Task<Stream> CreateErrorFileStreamAsync(CsvResponseModel<RegistrationCsvRecordResponse> csvResponse)
        {
            var validationErrors = ExtractAllValidationErrors(csvResponse);
            var errorFile = await _csvService.WriteFileAsync(validationErrors);
            return errorFile;
        }

        private List<ValidationError> ExtractAllValidationErrors(CsvResponseModel<RegistrationCsvRecordResponse> csvResponse)
        {
            if (csvResponse.IsDirty)
                return new List<ValidationError> { new ValidationError { ErrorMessage = csvResponse.ErrorMessage } };

            var result = new List<ValidationError>();
            var invalidReg = csvResponse.Rows?.Where(x => !x.IsValid).ToList();
            invalidReg.ForEach(x => { result.AddRange(x.ValidationErrors); });
            return result;
        }

        private async Task<bool> CreateDocumentUploadHistory(BulkRegistrationRequest request, DocumentUploadStatus status = DocumentUploadStatus.Processed)
        {
            if (request == null) return false;

            var model = new DocumentUploadHistoryDetails
            {
                TlAwardingOrganisationId = 1, // TODO: Get TlAwardingOrganisationId
                BlobFileName = request.BlobFileName,
                DocumentType = (int)request.DocumentType,
                FileType = (int)request.FileType,
                Status = (int)status,
                CreatedBy = request.PerformedBy
            };
            return await _documentUploadHistoryService.CreateDocumentUploadHistory(model);
        }

        private async Task<bool> UploadErrorsFileToBlobStorage(BulkRegistrationRequest request, byte[] errorFile)
        {
            if (errorFile == null || errorFile.Length == 0) return false;
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                BlobFileName = request.BlobFileName,
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.ValidationErrors}",
                UserName = request.PerformedBy,
                FileData = errorFile
            });
            return true;
        }

        private async Task<bool> MoveFileFromProcessingToProcessedAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.MoveFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                BlobFileName = request.BlobFileName,
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                DestinationFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processed}"
            });
            return true;
        }

        private async Task<bool> MoveFileFromProcessingToFailedAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.MoveFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                BlobFileName = request.BlobFileName,
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                DestinationFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Failed}"
            });
            return true;
        }
    }
}
