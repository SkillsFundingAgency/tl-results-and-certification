using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase, IRegistrationController
    {
        private readonly IRegistrationService _registrationService;
        private readonly IBulkRegistrationLoader _bulkRegistrationProcess;
        private readonly ILogger<ProviderController> _logger;

        public RegistrationController(IRegistrationService registrationService, IBulkRegistrationLoader bulkRegistrationProcess, ILogger<ProviderController> logger)
        {
            _registrationService = registrationService;
            _bulkRegistrationProcess = bulkRegistrationProcess; 
            _logger = logger;
        }

        [HttpPost]
        [Route("ProcessBulkRegistrations")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            return await _bulkRegistrationProcess.ProcessBulkRegistrationsAsync(request);
        }

        [HttpGet]
        [Route("GetRegisteredProviderCoreDetails/{aoUkprn}/{providerUkprn}")]
        public async Task<IList<PathwayDetails>> GetRegisteredProviderCoreDetailsAsync(long aoUkprn, long providerUkprn)
        {
            return await _registrationService.GetRegisteredProviderCoreDetailsAsync(aoUkprn, providerUkprn);
        }
    }
}