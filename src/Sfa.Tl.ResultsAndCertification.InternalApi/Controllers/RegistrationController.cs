using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IBulkRegistrationLoader _bulkRegistrationProcess;
        private readonly ILogger<ProviderController> _logger;

        public RegistrationController(IBulkRegistrationLoader bulkRegistrationProcess, ILogger<ProviderController> logger)
        {
            _bulkRegistrationProcess = bulkRegistrationProcess; 
            _logger = logger;
        }

        [HttpPost]
        [Route("ProcessBulkRegistrations")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            return await _bulkRegistrationProcess.ProcessBulkRegistrationsAsync(request);
        }
    }
}