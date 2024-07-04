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
    public class BulkWithdrawalLoader : BulkBaseLoader, IBulkWithdrawalLoader
    {
        private readonly ICsvHelperService<WithdrawalCsvRecordRequest, CsvResponseModel<WithdrawalCsvRecordResponse>, WithdrawalCsvRecordResponse> _csvService;
        private readonly IWithdrawalService _withdrawalService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkWithdrawalLoader> _logger;

        public BulkWithdrawalLoader(ICsvHelperService<WithdrawalCsvRecordRequest, CsvResponseModel<WithdrawalCsvRecordResponse>, WithdrawalCsvRecordResponse> csvService,
            IWithdrawalService withdrawalService,
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService,
            ILogger<BulkWithdrawalLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _withdrawalService = withdrawalService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkProcessResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkProcessResponse();
            try
            {
                CsvResponseModel<WithdrawalCsvRecordResponse> stage2WithdrawalsResponse = null;

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
                        var blobReadError = $"No FileStream found to process bluk withdrawals. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation
                    stage2WithdrawalsResponse = await _csvService.ReadAndParseFileAsync(new WithdrawalCsvRecordRequest { FileStream = fileStream });

                    if (!stage2WithdrawalsResponse.IsDirty)
                        CheckUlnDuplicates(stage2WithdrawalsResponse.Rows);
                }

                if (stage2WithdrawalsResponse.IsDirty || !stage2WithdrawalsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawalsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 3 validation - Learner validation
                var stage3WithdrawalsResponse = await _withdrawalService.ValidateWithdrawalLearnersAsync(request.AoUkprn, stage2WithdrawalsResponse.Rows.Where(x => x.IsValid));

                if (stage2WithdrawalsResponse.Rows.Any(x => !x.IsValid) || stage3WithdrawalsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawalsResponse, stage3WithdrawalsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 4 validation.
                var stage4WithdrawalsResponse = await _withdrawalService.ValidateWithdrawalTlevelsAsync(request.AoUkprn, stage2WithdrawalsResponse.Rows.Where(x => x.IsValid));

                if (stage2WithdrawalsResponse.Rows.Any(x => !x.IsValid) || stage3WithdrawalsResponse.Any(x => !x.IsValid) || stage4WithdrawalsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2WithdrawalsResponse, stage3WithdrawalsResponse, stage4WithdrawalsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var tqRegistrationProfiles = _withdrawalService.TransformWithdrawalModel(stage4WithdrawalsResponse, request.PerformedBy);

                // Step: Process Stage 4 validation and DB operation                
                var registrationProcessResult = await _withdrawalService.ProcessWithdrawalsAsync(request.AoUkprn, tqRegistrationProfiles, request.PerformedBy);

                return registrationProcessResult.IsValid ?
                    await ProcessWithdrawalResponse(request, response, registrationProcessResult) :
                    await SaveErrorsAndUpdateResponse(request, response, registrationProcessResult.ValidationErrors);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk withdrawals. Method: ProcessBulkWithdrawalsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkRegistrationProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }
            return response;
        }

        private async Task<BulkProcessResponse> ProcessWithdrawalResponse(BulkProcessRequest request, BulkProcessResponse response, WithdrawalProcessResponse registrationProcessResult)
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

        private static void CheckUlnDuplicates(IList<WithdrawalCsvRecordResponse> registrations)
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

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<WithdrawalCsvRecordResponse> stage2WithdrawalsResponse = null, IList<WithdrawalRecordResponse> stage3WithdrawalsResponse = null, IList<WithdrawalRecordResponse> stage4WithdrawalsResponse = null)
        {
            if (stage2WithdrawalsResponse != null && stage2WithdrawalsResponse.IsDirty)
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = stage2WithdrawalsResponse.ErrorMessage } };

            var errors = new List<BulkProcessValidationError>();

            if (stage2WithdrawalsResponse != null)
            {
                foreach (var invalidRegistration in stage2WithdrawalsResponse.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage3WithdrawalsResponse != null)
            {
                foreach (var invalidRegistration in stage3WithdrawalsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage4WithdrawalsResponse != null)
            {
                foreach (var invalidRegistration in stage4WithdrawalsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }

    }
}
