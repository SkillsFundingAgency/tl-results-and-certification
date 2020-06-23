using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkRegistrationLoader : IBulkRegistrationLoader
    {
        private readonly ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> _csvService;
        private readonly IRegistrationService _registrationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IDocumentUploadHistoryService _documentUploadHistoryService;
        private readonly ILogger<BulkRegistrationLoader> _logger;

        public BulkRegistrationLoader(ICsvHelperService<RegistrationCsvRecordRequest,
            CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> csvService,
            IRegistrationService registrationService, IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, ILogger<BulkRegistrationLoader> logger)
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
            try
            {
                CsvResponseModel<RegistrationCsvRecordResponse> stage2RegistrationsResponse = null;

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
                        var blobReadError = $"No FileStream found to process bluk registrations. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation
                    stage2RegistrationsResponse = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = fileStream });

                    if (!stage2RegistrationsResponse.IsDirty)
                        CheckUlnDuplicates(stage2RegistrationsResponse.Rows);
                }

                if (stage2RegistrationsResponse.IsDirty || !stage2RegistrationsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RegistrationsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 3 valiation. 
                var stage3RegistrationsResponse = await _registrationService.ValidateRegistrationTlevelsAsync(request.AoUkprn, stage2RegistrationsResponse.Rows.Where(x => x.IsValid));

                if (stage2RegistrationsResponse.Rows.Any(x => !x.IsValid) || stage3RegistrationsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RegistrationsResponse, stage3RegistrationsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var tqRegistrations = _registrationService.TransformRegistrationModel();

                // Step: Process DB operation
                var result = await _registrationService.CompareAndProcessRegistrations();

                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk registrations. Method: ProcessBulkRegistrationsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkRegistrationProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }
            return response;
        }

        private async Task<BulkRegistrationResponse> SaveErrorsAndUpdateResponse(BulkRegistrationRequest request, BulkRegistrationResponse response, IList<RegistrationValidationError> registrationValidationErrors)
        {
            var errorFile = await CreateErrorFileAsync(registrationValidationErrors);
            await UploadErrorsFileToBlobStorage(request, errorFile);
            await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, DocumentUploadStatus.Failed);

            response.IsSuccess = false;
            response.BlobUniqueReference = request.BlobUniqueReference;
            response.ErrorFileSize = Math.Round((errorFile.Length / 1024D), 2);

            return response;
        }

        private static void CheckUlnDuplicates(IList<RegistrationCsvRecordResponse> registrations)
        {
            var duplicateRegistrations = registrations.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);
            
            foreach (var record in duplicateRegistrations.SelectMany(duplicateRegistration => duplicateRegistration))
            {
                record.ValidationErrors.Add(new RegistrationValidationError
                {
                    RowNum = record.RowNum.ToString(),
                    Uln = record.Uln != 0 ? record.Uln.ToString() : string.Empty,
                    ErrorMessage = ValidationMessages.DuplicateRecord
                });
            }
        }

        private async Task<byte[]> CreateErrorFileAsync(IList<RegistrationValidationError> validationErrors)
        {
            return await _csvService.WriteFileAsync(validationErrors);
        }

        private IList<RegistrationValidationError> ExtractAllValidationErrors(CsvResponseModel<RegistrationCsvRecordResponse> stage2RegistrationsResponse = null, IList<RegistrationRecordResponse> stage3RegistrationsResponse = null)
        {
            if (stage2RegistrationsResponse != null && stage2RegistrationsResponse.IsDirty)
                return new List<RegistrationValidationError> { new RegistrationValidationError { ErrorMessage = stage2RegistrationsResponse.ErrorMessage } };

            var errors = new List<RegistrationValidationError>();

            if (stage2RegistrationsResponse != null)
            {
                foreach (var invalidRegistration in stage2RegistrationsResponse.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if(stage3RegistrationsResponse != null)
            {
                foreach (var invalidRegistration in stage3RegistrationsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }

        private async Task<bool> CreateDocumentUploadHistory(BulkRegistrationRequest request, DocumentUploadStatus status = DocumentUploadStatus.Processed)
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

        private async Task<bool> UploadErrorsFileToBlobStorage(BulkRegistrationRequest request, byte[] errorFile)
        {
            if (errorFile == null || errorFile.Length == 0) return false;
            await _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.ValidationErrors}",
                BlobFileName = request.BlobFileName,
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
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                BlobFileName = request.BlobFileName,
                DestinationFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Failed}"
            });
            return true;
        }

        private async Task<bool> DeleteFileFromProcessingFolderAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.DeleteFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                BlobFileName = request.BlobFileName
            });
            return true;
        }
    }
}
