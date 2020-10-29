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
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkAssessmentLoader : BulkBaseLoader, IBulkProcessLoader
    {
        private readonly ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> _csvService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BulkAssessmentLoader> _logger;

        public BulkAssessmentLoader(ICsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> csvService,
            //IRegistrationService registrationService, 
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, 
            ILogger<BulkAssessmentLoader> logger) : base(csvService, blobStorageService, documentUploadHistoryService)
        {
            _csvService = csvService;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        public async Task<BulkRegistrationResponse> ProcessAsync(BulkRegistrationRequest request)
        {
            var response = new BulkRegistrationResponse();
            try
            {
                CsvResponseModel<AssessmentCsvRecordResponse> stage2AssessmentsResponse = null;

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
                    stage2AssessmentsResponse = await _csvService.ReadAndParseFileAsync(new AssessmentCsvRecordRequest { FileStream = fileStream });

                    // TODO: check duplicate function. 
                    if (!stage2AssessmentsResponse.IsDirty)
                        CheckUlnDuplicates(stage2AssessmentsResponse.Rows);
                }

                // Step 2: Stage 2 validations 
                if (stage2AssessmentsResponse.IsDirty || !stage2AssessmentsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2AssessmentsResponse);
                    return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                }

                // TODO: Stage 3

                // Temp response;
                return new BulkRegistrationResponse 
                {
                    IsSuccess = false,
                    Stats = new BulkUploadStats { TotalRecordsCount = 11 }
                };
            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk assessments. Method: ProcessBulkAssessmentsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkAssessmentProcessFailed, ex, errorMessage);
                await DeleteFileFromProcessingFolderAsync(request);
            }

            return response;
        }

        private IList<RegistrationValidationError> ExtractAllValidationErrors(CsvResponseModel<AssessmentCsvRecordResponse> stage2Response = null, IList<AssessmentCsvRecordResponse> stage3Response = null)
        {
            if (stage2Response != null && stage2Response.IsDirty)
                return new List<RegistrationValidationError> { new RegistrationValidationError { ErrorMessage = stage2Response.ErrorMessage } };

            var errors = new List<RegistrationValidationError>();

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
                record.ValidationErrors.Add(new RegistrationValidationError
                {
                    RowNum = record.RowNum.ToString(),
                    Uln = record.Uln != 0 ? record.Uln.ToString() : string.Empty,
                    ErrorMessage = ValidationMessages.DuplicateRecord
                });
            }
        }

        private async Task<BulkRegistrationResponse> SaveErrorsAndUpdateResponse(BulkRegistrationRequest request, BulkRegistrationResponse response, IList<RegistrationValidationError> validationErrors)
        {
            // note: method can't be moved to base. Response type may be different in future. 
            var errorFile = await CreateErrorFileAsync(validationErrors);
            await UploadErrorsFileToBlobStorage(request, errorFile);
            await MoveFileFromProcessingToFailedAsync(request);
            await CreateDocumentUploadHistory(request, DocumentUploadStatus.Failed);

            response.IsSuccess = false;
            response.BlobUniqueReference = request.BlobUniqueReference;
            response.ErrorFileSize = Math.Round((errorFile.Length / 1024D), 2);

            return response;
        }
    }
}
