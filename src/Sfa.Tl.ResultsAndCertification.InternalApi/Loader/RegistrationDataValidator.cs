using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class RegistrationDataValidator : IRegistrationDataValidator
    {
        private readonly ICsvHelperService<RegistrationCsvRecord, Registration> _csvService;
        private readonly IRegistrationService _registrationService;
        private readonly ILogger<RegistrationDataValidator> _logger;

        public RegistrationDataValidator(ICsvHelperService<RegistrationCsvRecord, Registration> csvService,
            IRegistrationService registrationService,
            ILogger<RegistrationDataValidator> logger)
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
            var registrations = await _csvService.ReadAndParseFileAsync(new RegistrationCsvRecord { FileStream = null });
            var isFileDirty = false;
            if (isFileDirty)
            {
                // Todo: Blob operations
                return response;
            }

            // Stage 3 valiation. 
            await _registrationService.ValidateRegistrationTlevelsAsync(null);
            if (registrations.Any(x => !x.IsValid))
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
