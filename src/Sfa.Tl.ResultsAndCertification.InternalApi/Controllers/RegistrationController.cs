using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

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

        [HttpPost]
        [Route("AddRegistration")]
        public async Task<bool> AddRegistrationAsync(RegistrationRequest model)
        {
            return await _registrationService.AddRegistrationAsync(model);
        }

        [HttpGet]
        [Route("FindUln/{aoUkprn}/{uln}")]
        public async Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln)
        {
            return await _registrationService.FindUlnAsync(aoUkprn, uln);
        }

        [HttpGet]
        [Route("GetRegistrationDetails/{aoUkprn}/{profileId}")]
        public async Task<RegistrationDetails> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId)
        {
            return await _registrationService.GetRegistrationDetailsByProfileIdAsync(aoUkprn, profileId);
        }

        [HttpDelete]
        [Route("DeleteRegistration/{aoUkprn}/{profileId}")]
        public async Task<bool> DeleteRegistrationByProfileIdAsync(long aoUkprn, int profileId)
        {
            return await _registrationService.DeleteRegistrationByProfileId(aoUkprn, profileId);
        }
    }
}