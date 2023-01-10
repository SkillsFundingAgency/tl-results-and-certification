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
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkIndustryPlacementLoader : BulkBaseLoader, IBulkIndustryPlacementLoader
    {
        private readonly ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse> _csvService;
        protected IIndustryPlacementService _industryPlacementService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkIndustryPlacementLoader> _logger;
        public BulkIndustryPlacementLoader(ICsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse> csvService,
            IIndustryPlacementService industryPlacementService,
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService,
            ILogger<BulkIndustryPlacementLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _industryPlacementService = industryPlacementService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkIndustryPlacementResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkIndustryPlacementResponse();
            try
            {
                CsvResponseModel<IndustryPlacementCsvRecordResponse> stage2Response = null;

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
                        var blobReadError = $"No FileStream found to process bulkk industry placements. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation
                    stage2Response = await _csvService.ReadAndParseFileAsync(new IndustryPlacementCsvRecordRequest { FileStream = fileStream });

                    if (!stage2Response.IsDirty)
                        CheckUlnDuplicates(stage2Response.Rows);
                }

                // Step 2: Stage 2 validations 
                if (stage2Response.IsDirty || !stage2Response.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2Response);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step 3: Stage 3 valiation. 
                var stage3Response = await _industryPlacementService.ValidateIndustryPlacementsAsync(request.AoUkprn, stage2Response.Rows.Where(x => x.IsValid));
                if (stage2Response.Rows.Any(x => !x.IsValid) || stage3Response.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2Response, stage3Response);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step 4: Map data to DB model type.
                var industryPlacements = _industryPlacementService.TransformIndustryPlacementsModel(stage3Response, request.PerformedBy);

                // Step 5: Process Stage 4 validation and DB operation
                var industryPlacementsProcessResult = await _industryPlacementService.CompareAndProcessIndustryPlacementsAsync(industryPlacements);

                // update total industry placement records stats
                industryPlacementsProcessResult.BulkUploadStats = new BulkUploadStats { TotalRecordsCount = stage3Response.Count };

                return industryPlacementsProcessResult.IsValid ?
                    await ProcessIndustryPlacementsResponse(request, response, industryPlacementsProcessResult) :
                    await SaveErrorsAndUpdateResponse(request, response, industryPlacementsProcessResult.ValidationErrors);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk industry placements. Method: ProcessBulkIndustryPlacementsAsync(BulkProcessRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkIndustryPlacementProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }

            return response;
        }

        private async Task<BulkIndustryPlacementResponse> SaveErrorsAndUpdateResponse(BulkProcessRequest request, BulkIndustryPlacementResponse response, IList<BulkProcessValidationError> validationErrors)
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

        private async Task<BulkIndustryPlacementResponse> ProcessIndustryPlacementsResponse(BulkProcessRequest request, BulkIndustryPlacementResponse response, IndustryPlacementProcessResponse industryPlacementProcessResult)
        {
            _ = industryPlacementProcessResult.IsSuccess ? await MoveFileFromProcessingToProcessedAsync(request) : await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, industryPlacementProcessResult.IsSuccess ? DocumentUploadStatus.Processed : DocumentUploadStatus.Failed);
            response.IsSuccess = industryPlacementProcessResult.IsSuccess;
            response.Stats = industryPlacementProcessResult.BulkUploadStats;
            return response;
        }

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<IndustryPlacementCsvRecordResponse> stage2Response = null, IList<IndustryPlacementRecordResponse> stage3Response = null)
        {
            if (stage2Response != null && stage2Response.IsDirty)
            {
                var errorMessage = stage2Response.ErrorCode == CsvFileErrorCode.NoRecordsFound ? ValidationMessages.AtleastOneEntryRequired : stage2Response.ErrorMessage;
                return new List<BulkProcessValidationError> { new BulkProcessValidationError { ErrorMessage = errorMessage } };
            }

            var errors = new List<BulkProcessValidationError>();

            if (stage2Response != null)
            {
                foreach (var invalidIndustryPlacement in stage2Response.Rows?.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidIndustryPlacement.ValidationErrors);
                }
            }

            if (stage3Response != null)
            {
                foreach (var invalidIndustryPlacement in stage3Response.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidIndustryPlacement.ValidationErrors);
                }
            }
            return errors;
        }

        private static void CheckUlnDuplicates(IList<IndustryPlacementCsvRecordResponse> industryPlacements)
        {
            var duplicateIndustryPlacements = industryPlacements.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

            foreach (var record in duplicateIndustryPlacements.SelectMany(r => r))
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
