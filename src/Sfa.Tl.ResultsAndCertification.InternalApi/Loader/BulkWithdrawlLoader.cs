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
    public class BulkWithdrawlLoader : BulkBaseLoader, IBulkWithdrawlLoader
    {
        private readonly ICsvHelperService<WithdrawlCsvRecordRequest, CsvResponseModel<WithdrawlCsvRecordResponse>, WithdrawlCsvRecordResponse> _csvService;
        private readonly IWithdrawlService _withdrawlService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkWithdrawlLoader> _logger;

        public BulkWithdrawlLoader(ICsvHelperService<WithdrawlCsvRecordRequest, CsvResponseModel<WithdrawlCsvRecordResponse>, WithdrawlCsvRecordResponse> csvService,
            IWithdrawlService withdrawlService,
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService,
            ILogger<BulkWithdrawlLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _withdrawlService = withdrawlService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkProcessResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkProcessResponse();
            try
            {
                CsvResponseModel<WithdrawlCsvRecordResponse> stage2WithdrawlsResponse = null;

                // Step: 1 Read file from Blob
                using (var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = request.DocumentType.ToString(),
                    BlobFileName = request.BlobFileName,
                    SourceFilePath = $"{request.AoUkprn}/{BulkProcessStatus.Processing}",
                    UserName = request.PerformedBy
                }))
                {
                    if (fileStream == null)
                    {
                        var blobReadError = $"No FileStream found to process bluk withdrawls. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation
                    stage2WithdrawlsResponse = await _csvService.ReadAndParseFileAsync(new WithdrawlCsvRecordRequest { FileStream = fileStream });

                    if (!stage2WithdrawlsResponse.IsDirty)
                        CheckUlnDuplicates(stage2WithdrawlsResponse.Rows);
                }

                if (stage2WithdrawlsResponse.IsDirty || !stage2WithdrawlsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawlsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 3 validation - Learner validation
                var stage3WithdrawlsResponse = await _withdrawlService.ValidateWithdrawlLearnersAsync(request.AoUkprn, stage2WithdrawlsResponse.Rows.Where(x => x.IsValid));

                if (stage2WithdrawlsResponse.Rows.Any(x => !x.IsValid) || stage3WithdrawlsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawlsResponse, stage3WithdrawlsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 4 validation.
                var stage4WithdrawlsResponse = await _withdrawlService.ValidateWithdrawlTlevelsAsync(request.AoUkprn, stage2WithdrawlsResponse.Rows.Where(x => x.IsValid));

                if (stage2WithdrawlsResponse.Rows.Any(x => !x.IsValid) || stage3WithdrawlsResponse.Any(x => !x.IsValid) || stage4WithdrawlsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawlsResponse, stage3WithdrawlsResponse, stage4WithdrawlsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var tqRegistrationProfiles = _withdrawlService.TransformWithdrawlModel(stage4WithdrawlsResponse, request.PerformedBy);

                // Step: Process Stage 4 validation and DB operation                
                var registrationProcessResult = await _withdrawlService.ProcessWithdrawlsAsync(request.AoUkprn, tqRegistrationProfiles, request.PerformedBy);

                return registrationProcessResult.IsValid ?
                    await ProcessWithdrawlResponse(request, response, registrationProcessResult) :
                    await SaveErrorsAndUpdateResponse(request, response, registrationProcessResult.ValidationErrors);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk withdrawls. Method: ProcessBulkWithdrawlsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkRegistrationProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }
            return response;
        }

        private async Task<BulkProcessResponse> ProcessWithdrawlResponse(BulkProcessRequest request, BulkProcessResponse response, WithdrawlProcessResponse registrationProcessResult)
        {
            _ = registrationProcessResult.IsSuccess ? await MoveFileFromProcessingToProcessedAsync(request) : await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, registrationProcessResult.IsSuccess ? DocumentUploadStatus.Processed : DocumentUploadStatus.Failed);
            response.IsSuccess = registrationProcessResult.IsSuccess;
            response.Stats = registrationProcessResult.BulkUploadStats;
            return response;
        }

        private async Task<BulkProcessResponse> SaveErrorsAndUpdateResponse(BulkProcessRequest request, BulkProcessResponse response, IList<BulkProcessValidationError> registrationValidationErrors)
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

        private static void CheckUlnDuplicates(IList<WithdrawlCsvRecordResponse> registrations)
        {
            var duplicateRegistrations = registrations.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

            foreach (var record in duplicateRegistrations.SelectMany(duplicateRegistration => duplicateRegistration))
            {
                record.ValidationErrors.Add(new BulkProcessValidationError
                {
                    RowNum = record.RowNum.ToString(),
                    Uln = record.Uln != 0 ? record.Uln.ToString() : string.Empty,
                    ErrorMessage = ValidationMessages.DuplicateRecord
                });
            }
        }

        private async Task<byte[]> CreateErrorFileAsync(IList<BulkProcessValidationError> validationErrors)
        {
            return await _csvService.WriteFileAsync(validationErrors);
        }

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<WithdrawlCsvRecordResponse> stage2WithdrawlsResponse = null, IList<WithdrawlRecordResponse> stage3WithdrawlsResponse = null, IList<WithdrawlRecordResponse> stage4WithdrawlsResponse = null)
        {
            if (stage2WithdrawlsResponse != null && stage2WithdrawlsResponse.IsDirty)
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = stage2WithdrawlsResponse.ErrorMessage } };

            var errors = new List<BulkProcessValidationError>();

            if (stage2WithdrawlsResponse != null)
            {
                foreach (var invalidRegistration in stage2WithdrawlsResponse.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage3WithdrawlsResponse != null)
            {
                foreach (var invalidRegistration in stage3WithdrawlsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage4WithdrawlsResponse != null)
            {
                foreach (var invalidRegistration in stage4WithdrawlsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }

    }
}
