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
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkRommLoader : BulkBaseLoader, IBulkRommLoader
    {
        private readonly ICsvHelperService<RommsCsvRecordRequest, CsvResponseModel<RommCsvRecordResponse>, RommCsvRecordResponse> _csvService;
        private readonly IRommService _rommService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkRommLoader> _logger;

        public BulkRommLoader(ICsvHelperService<RommsCsvRecordRequest, CsvResponseModel<RommCsvRecordResponse>, RommCsvRecordResponse> csvService,
            IRommService rommService,
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService,
            ILogger<BulkRommLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _rommService = rommService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkProcessResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkProcessResponse();
            try
            {
                CsvResponseModel<RommCsvRecordResponse> stage2RommsResponse = null;

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
                    stage2RommsResponse = await _csvService.ReadAndParseFileAsync(new RommsCsvRecordRequest { FileStream = fileStream });

                    if (!stage2RommsResponse.IsDirty)
                        CheckUlnDuplicates(stage2RommsResponse.Rows);
                }

                if (stage2RommsResponse.IsDirty || !stage2RommsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RommsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 3 validation - Learner validation
                var stage3RommsResponse = await _rommService.ValidateRommLearnersAsync(request.AoUkprn, stage2RommsResponse.Rows.Where(x => x.IsValid));

                if (stage2RommsResponse.Rows.Any(x => !x.IsValid) || stage3RommsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RommsResponse, stage3RommsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 4 validation.
                var stage4RommsResponse = await _rommService.ValidateRommTlevelsAsync(request.AoUkprn, stage2RommsResponse.Rows.Where(x => x.IsValid));

                if (stage2RommsResponse.Rows.Any(x => !x.IsValid) || stage3RommsResponse.Any(x => !x.IsValid) || stage4RommsResponse.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2RommsResponse, stage3RommsResponse, stage4RommsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var tqRegistrationProfiles = _rommService.TransformRommModel(stage4RommsResponse, request.PerformedBy);

                // Step: Process Stage 4 validation and DB operation                
                var rommProcessResult = await _rommService.ProcessRommsAsync(request.AoUkprn, tqRegistrationProfiles, request.PerformedBy);

                return rommProcessResult.IsValid ?
                    await ProcessRommResponse(request, response, rommProcessResult) :
                    await SaveErrorsAndUpdateResponse(request, response, rommProcessResult.ValidationErrors);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bulk withdrawals. Method: ProcessBulkRommsAsync(BulkRommsRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkRommProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }
            return response;
        }

        private async Task<BulkProcessResponse> ProcessRommResponse(BulkProcessRequest request, BulkProcessResponse response, RommsProcessResponse rommProcessResult)
        {
            _ = rommProcessResult.IsSuccess ? await MoveFileFromProcessingToProcessedAsync(request) : await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, rommProcessResult.IsSuccess ? DocumentUploadStatus.Processed : DocumentUploadStatus.Failed);
            response.IsSuccess = rommProcessResult.IsSuccess;
            response.Stats = rommProcessResult.BulkUploadStats;
            return response;
        }

        private async Task<BulkProcessResponse> SaveErrorsAndUpdateResponse(BulkProcessRequest request, BulkProcessResponse response, IList<BulkProcessValidationError> rommValidationErrors)
        {
            var errorFile = await CreateErrorFileAsync(rommValidationErrors);
            await UploadErrorsFileToBlobStorage(request, errorFile);
            await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, DocumentUploadStatus.Failed);

            response.IsSuccess = false;
            response.BlobUniqueReference = request.BlobUniqueReference;
            response.ErrorFileSize = Math.Round((errorFile.Length / 1024D), 2);

            return response;
        }

        private static void CheckUlnDuplicates(IList<RommCsvRecordResponse> romms)
        {
            var duplicateRegistrations = romms.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

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

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<RommCsvRecordResponse> stage2RommsResponse = null, IList<RommsRecordResponse> stage3RommsResponse = null, IList<RommsRecordResponse> stage4RommsResponse = null)
        {
            if (stage2RommsResponse != null && stage2RommsResponse.IsDirty)
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = stage2RommsResponse.ErrorMessage } };

            var errors = new List<BulkProcessValidationError>();

            if (stage2RommsResponse != null)
            {
                foreach (var invalidRegistration in stage2RommsResponse.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage3RommsResponse != null)
            {
                foreach (var invalidRegistration in stage3RommsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage4RommsResponse != null)
            {
                foreach (var invalidRegistration in stage4RommsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }

    }
}
