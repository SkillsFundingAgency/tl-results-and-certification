using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
            var registrations = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecordRequest { FileStream = null });
            var isFileDirty = false;
            if (isFileDirty)
            {
                // Todo: Blob operations
                return response;
            }

            // Stage 3 valiation. 
            await _registrationService.ValidateRegistrationTlevelsAsync(null);
            if (registrations.Rows.Any(x => !x.IsValid))
            {
                // Todo: blob operation
                return response;
            }

            // Step: Map data to DB model type.
            var tqRegistrations = _registrationService.TransformRegistrationModel();

            // Step: Process DB operation
            var result = await _registrationService.CompareAndProcessRegistrations();

            return response;
        }
    }
}
