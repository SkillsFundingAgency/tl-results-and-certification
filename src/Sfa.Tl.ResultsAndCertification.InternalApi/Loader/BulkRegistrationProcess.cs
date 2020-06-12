using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkRegistrationProcess : IBulkRegistrationProcess
    {
        private readonly ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> _csvService;
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<BulkRegistrationProcess> _logger;

        public BulkRegistrationProcess(ICsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse> csvService,
            IRegistrationService registrationService,
            ILogger<BulkRegistrationProcess> logger)
        {
            _csvService = csvService;
            _registrationService = registrationService;
            _logger = logger;
        }

        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            var response = new BulkRegistrationResponse();

            // Step: Todo - Read file from Blob

            // Stage 2 validation
            var csvResponse = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = null });
            if (csvResponse.IsDirty)
            {
                // Todo: Blob operations
                var errorFile = await _csvService.WriteErrorFile(new List<ValidationError> { new ValidationError { ErrorMessage = csvResponse.ErrorMessage } });
                return response;
            }

            // Stage 3 valiation. 
            await _registrationService.ValidateRegistrationTlevelsAsync(csvResponse.Rows.Where(x => x.IsValid));
            if (csvResponse.Rows.Any(x => !x.IsValid))
            {
                // Todo: blob operation
                var validationErrors = FilterValidationErrors(csvResponse.Rows);
                var errorFile = await _csvService.WriteErrorFile<ValidationError>(validationErrors);
                return response;
            }

            // Step: Map data to DB model type.
            var tqRegistrations = _registrationService.TransformRegistrationModel();

            // Step: Process DB operation
            var result = await _registrationService.CompareAndProcessRegistrations();

            return response;
        }

        private List<ValidationError> FilterValidationErrors(IList<RegistrationCsvRecordResponse> registrations)
        {
            var result = new List<ValidationError>();
            var invalidReg = registrations.Where(x => !x.IsValid).ToList();
            invalidReg.ForEach(x => { result.AddRange(x.ValidationErrors); });
            return result;
        }
    }
}
