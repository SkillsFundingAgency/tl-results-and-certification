using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkAssessmentLoader : BulkBaseLoader, IBulkAssessmentLoader
    {
        private readonly ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> _csvService;
        protected IAssessmentService _assessmentService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkAssessmentLoader> _logger;

        public BulkAssessmentLoader(ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> csvService,
            IAssessmentService assessmentService, 
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, 
            ILogger<BulkAssessmentLoader> logger) : base(blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _assessmentService = assessmentService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkAssessmentResponse> ProcessAsync(BulkProcessRequest request)
        {
            var response = new BulkAssessmentResponse();
            try
            {
                CsvResponseModel<AssessmentCsvRecordResponse> stage2Response = null;

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
                        var blobReadError = $"No FileStream found to process bluk assessments. Method: DownloadFileAsync(ContainerName: {request.DocumentType}, BlobFileName = {request.BlobFileName}, SourceFilePath = {request.AoUkprn}/{BulkProcessStatus.Processing}, UserName = {request.PerformedBy}), User: {request.PerformedBy}";
                        _logger.LogInformation(LogEvent.FileStreamNotFound, blobReadError);
                        throw new Exception(blobReadError);
                    }

                    // Stage 2 validation - TODO
                    stage2Response = await _csvService.ReadAndParseFileAsync(new AssessmentCsvRecordRequest { FileStream = fileStream });

                    if (!stage2Response.IsDirty)
                        CheckUlnDuplicates(stage2Response.Rows);
                }

                // Step 2: Stage 2 validations 
                if (stage2Response.IsDirty || stage2Response.Rows.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2Response);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Stage 3 valiation. 
                var stage3Response = await _assessmentService.ValidateAssessmentsAsync(request.AoUkprn, stage2Response.Rows.Where(x => x.IsValid));
                if (stage2Response.Rows.Any(x => !x.IsValid) || stage3Response.Any(x => !x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2Response, stage3Response);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // Step: Map data to DB model type.
                var assessments = _assessmentService.TransformAssessmentsModel(stage3Response, request.PerformedBy);

                // Step: DB operation                
                var assessmentsProcessResult = await _assessmentService.CompareAndProcessAssessmentsAsync(assessments.Item1, assessments.Item2);

                return await ProcessAssessmentsResponse(request, response, assessmentsProcessResult);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk assessments. Method: ProcessBulkAssessmentsAsync(BulkProcessRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkAssessmentProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }

            return response;
        }

        private IList<BulkProcessValidationError> ExtractAllValidationErrors(CsvResponseModel<AssessmentCsvRecordResponse> stage2Response = null, IList<AssessmentRecordResponse> stage3Response = null)
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

        private static void CheckUlnDuplicates(IList<AssessmentCsvRecordResponse> assessments)
        {
            var duplicateAssessments = assessments.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

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

        private async Task<BulkAssessmentResponse> SaveErrorsAndUpdateResponse(BulkProcessRequest request, BulkAssessmentResponse response, IList<BulkProcessValidationError> validationErrors)
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

        private async Task<BulkAssessmentResponse> ProcessAssessmentsResponse(BulkProcessRequest request, BulkAssessmentResponse response, AssessmentProcessResponse assessmentsProcessResult)
        {
            _ = assessmentsProcessResult.IsSuccess ? await MoveFileFromProcessingToProcessedAsync(request) : await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, assessmentsProcessResult.IsSuccess ? DocumentUploadStatus.Processed : DocumentUploadStatus.Failed);
            response.IsSuccess = assessmentsProcessResult.IsSuccess;
            response.Stats = assessmentsProcessResult.BulkUploadStats;
            return response;
        }

        private async Task<byte[]> CreateErrorFileAsync(IList<BulkProcessValidationError> validationErrors)
        {
            return await _csvService.WriteFileAsync(validationErrors);
        }
    }
}
