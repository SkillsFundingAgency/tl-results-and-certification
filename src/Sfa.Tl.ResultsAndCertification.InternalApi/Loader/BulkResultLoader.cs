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
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkResultLoader : BulkBaseLoader, IBulkResultLoader
    {
        private readonly ICsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse> _csvService;
        protected IResultService _resultService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkResultLoader> _logger;

        public BulkResultLoader(ICsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse> csvService,
            IResultService resultService,
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService,
            ILogger<BulkResultLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _resultService = resultService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkResultResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkResultResponse();
            try
            {
                CsvResponseModel<ResultCsvRecordResponse> stage2Response = null;

                // Step 1:  Read file from Blob
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
                        var blobReadError = $"No FileStream found to process bluk results. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation
                    stage2Response = await _csvService.ReadAndParseFileAsync(new ResultCsvRecordRequest { FileStream = fileStream });

                    if (!stage2Response.IsDirty)
                        CheckUlnDuplicates(stage2Response.Rows);
                }

                // Step 2: Stage 2 validations 
                if (stage2Response.IsDirty || !stage2Response.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2Response);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                var resultsProcessResult = new ResultProcessResponse();
                resultsProcessResult.IsSuccess = true;

                return await ProcessResultsResponse(request, response, resultsProcessResult);

            }
            catch(Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk results. Method: ProcessBulkResultsAsync(BulkProcessRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkResultProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }

            return response;
        }

        private async Task<BulkResultResponse> SaveErrorsAndUpdateResponse(BulkProcessRequest request, BulkResultResponse response, IList<BulkProcessValidationError> validationErrors)
        {
            var errorFile = await CreateErrorFileAsync(validationErrors);
            await UploadErrorsFileToBlobStorage(request, errorFile);
            await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, DocumentUploadStatus.Failed);

            response.IsSuccess = false;
            response.BlobUniqueReference = request.BlobUniqueReference;
            response.ErrorFileSize = Math.Round((errorFile.Length / 1024D), 2);

            return response;
        }

        private async Task<BulkResultResponse> ProcessResultsResponse(BulkProcessRequest request, BulkResultResponse response, ResultProcessResponse resultProcessResult)
        {
            _ = resultProcessResult.IsSuccess ? await MoveFileFromProcessingToProcessedAsync(request) : await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, resultProcessResult.IsSuccess ? DocumentUploadStatus.Processed : DocumentUploadStatus.Failed);
            response.IsSuccess = resultProcessResult.IsSuccess;
            response.Stats = resultProcessResult.BulkUploadStats;
            return response;
        }

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<ResultCsvRecordResponse> stage2Response = null, IList<ResultRecordResponse> stage3Response = null)
        {
            if (stage2Response != null && stage2Response.IsDirty)
            {
                var errorMessage = stage2Response.ErrorCode == CsvFileErrorCode.NoRecordsFound ? ValidationMessages.AtleastOneEntryRequired : stage2Response.ErrorMessage;
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = errorMessage } };
            }

            var errors = new List<BulkProcessValidationError>();

            if (stage2Response != null)
            {
                foreach (var invalidRegistration in stage2Response.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            if (stage3Response != null)
            {
                foreach (var invalidAssessment in stage3Response.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidAssessment.ValidationErrors);
                }
            }
            return errors;
        }

        private static void CheckUlnDuplicates(IList<ResultCsvRecordResponse> results)
        {
            var duplicateAssessments = results.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

            foreach (var record in duplicateAssessments.SelectMany(assessemt => assessemt))
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
    }
}
