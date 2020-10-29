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
        private readonly IDocumentUploadHistoryService _documentUploadHistoryService;
        private readonly ILogger<BulkAssessmentLoader> _logger;

        public BulkAssessmentLoader(ICsvHelperService<AssessmentCsvRecordRequest,
            CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse> csvService,
            //IRegistrationService registrationService, 
            IBlobStorageService blobStorageService,
            IDocumentUploadHistoryService documentUploadHistoryService, ILogger<BulkAssessmentLoader> logger)
        {
            _csvService = csvService;
            _blobStorageService = blobStorageService;
            _documentUploadHistoryService = documentUploadHistoryService;
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

                    // Stage 2 validation - TODO
                    //stage2AssessmentsResponse = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = fileStream });

                    // TODO: check duplicate function. 
                    if (!stage2AssessmentsResponse.IsDirty)
                        CheckUlnDuplicates(stage2AssessmentsResponse.Rows);
                }

                // Step 2: Stage 2 validations 
                if (stage2AssessmentsResponse.IsDirty || !stage2AssessmentsResponse.Rows.Any(x => x.IsValid))
                {
                    var validationErrors = ExtractAllValidationErrors(stage2AssessmentsResponse);
                    //return await SaveErrorsAndUpdateResponse(request, response, validationErrors);
                    return response;
                }

            }
            catch (Exception ex)
            {
                var errorMessage = $"Something went wrong while processing bluk assessments. Method: ProcessBulkAssessmentsAsync(BulkRegistrationRequest : {JsonConvert.SerializeObject(request)}), User: {request.PerformedBy}";
                _logger.LogError(LogEvent.BulkAssessmentProcessFailed, ex, errorMessage);
                // await DeleteFileFromProcessingFolderAsync(request); TODO: move this into common.
            }

            // Read from the Blob
            // Parse
            // return if any erros
            // validate Stage2 

            // return if any errors
            // then save to DB and return.

            return await Task.Run(() => response);
        }

        private IList<RegistrationValidationError> ExtractAllValidationErrors(CsvResponseModel<AssessmentCsvRecordResponse> stage2RegistrationsResponse = null, IList<AssessmentCsvRecordResponse> stage3RegistrationsResponse = null)
        {
            // TODO: Can this be a generic method to move?
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

            if (stage3RegistrationsResponse != null)
            {
                foreach (var invalidRegistration in stage3RegistrationsResponse.Where(x => !x.IsValid))
                {
                    errors.AddRange(invalidRegistration.ValidationErrors);
                }
            }

            return errors;
        }

        private static void CheckUlnDuplicates(IList<AssessmentCsvRecordResponse> registrations)
        {
            var duplicateRegistrations = registrations.Where(r => r.Uln != 0).GroupBy(r => r.Uln).Where(g => g.Count() > 1).Select(x => x);

            foreach (var record in duplicateRegistrations.SelectMany(duplicateRegistration => duplicateRegistration))
            {
                record.ValidationErrors.Add(new RegistrationValidationError
                {
                    RowNum = record.RowNum.ToString(),
                    Uln = record.Uln != 0 ? record.Uln.ToString() : string.Empty,
                    ErrorMessage = ValidationMessages.DuplicateRecord  // TODO: check content is same?
                });
            }
        }
    }
}
