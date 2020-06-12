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
        private readonly IBulkRegistrationProcess _csvDataProcess;
        private readonly ILogger<ProviderController> _logger;

        public RegistrationController(IBulkRegistrationProcess csvDataValidator, ILogger<ProviderController> logger)
        {

            _csvDataProcess = csvDataValidator; 
            _logger = logger;
        }

        [HttpPost]
        [Route("bulk-upload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            return await _csvDataProcess.ProcessBulkRegistrationsAsync(request);
        }
    }
}