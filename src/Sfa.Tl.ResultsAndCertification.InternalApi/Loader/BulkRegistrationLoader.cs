﻿using Microsoft.Extensions.Logging;
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
    public class BulkRegistrationLoader : BulkBaseLoader, IBulkProcessLoader
    {
        private readonly ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> _csvService;
        private readonly IRegistrationService _registrationService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkRegistrationLoader> _logger;

        public BulkRegistrationLoader(ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> csvService,
            IRegistrationService registrationService, 
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, 
            ILogger<BulkRegistrationLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _registrationService = registrationService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkProcessResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkProcessResponse();
            try
            {
                CsvResponseModel<RegistrationCsvRecordResponse> stage2RegistrationsResponse = null;

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
                        var blobReadError = $"No FileStream found to process bluk registrations. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
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

                // Stage 3 validation. 
                var stage3RegistrationsResponse = await _registrationService.ValidateRegistrationTlevelsAsync(request.AoUkprn, stage2RegistrationsResponse.Rows.Where(x => x.IsValid));

                if (stage2RegistrationsResponse.Rows.Any(x => !x.IsValid) || stage3RegistrationsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RegistrationsResponse, stage3RegistrationsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var tqRegistrationProfiles = _registrationService.TransformRegistrationModel(stage3RegistrationsResponse, request.PerformedBy);

                // Step: Process Stage 4 validation and DB operation                
                var registrationProcessResult = await _registrationService.CompareAndProcessRegistrationsAsync(tqRegistrationProfiles);

                return registrationProcessResult.IsValid ? 
                    await ProcessRegistrationResponse(request, response, registrationProcessResult) : 
                    await SaveErrorsAndUpdateResponse(request, response, registrationProcessResult.ValidationErrors);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk registrations. Method: ProcessBulkRegistrationsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkRegistrationProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }
            return response;
        }

        private async Task<BulkProcessResponse> ProcessRegistrationResponse(BulkProcessRequest request, BulkProcessResponse response, RegistrationProcessResponse registrationProcessResult)
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

        private static void CheckUlnDuplicates(IList<RegistrationCsvRecordResponse> registrations)
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

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<RegistrationCsvRecordResponse> stage2RegistrationsResponse = null, IList<RegistrationRecordResponse> stage3RegistrationsResponse = null)
        {
            if (stage2RegistrationsResponse != null && stage2RegistrationsResponse.IsDirty)
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = stage2RegistrationsResponse.ErrorMessage } };

            var errors = new List<BulkProcessValidationError>();

            if (stage2RegistrationsResponse != null)
            {
                foreach (var invalidRegistration in stage2RegistrationsResponse.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage3RegistrationsResponse != null)
            {
                foreach (var invalidRegistration in stage3RegistrationsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }        
    }
}
