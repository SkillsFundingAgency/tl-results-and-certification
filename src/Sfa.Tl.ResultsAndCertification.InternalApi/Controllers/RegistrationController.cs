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
        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            return await _registrationService.DeleteRegistrationAsync(aoUkprn, profileId);
        }

        [HttpGet]
        [Route("GetRegistration/{aoUkprn}/{profileId}")]
        public async Task<ManageRegistration> GetRegistrationAsync(long aoUkprn, int profileId)
        {
            return await _registrationService.GetRegistrationAsync(aoUkprn, profileId);
        }

        [HttpPut]
        [Route("UpdateRegistration")]
        public async Task<bool> UpdateRegistrationAsync(ManageRegistration model)
        {
            return await _registrationService.UpdateRegistrationAsync(model);
        }

        [HttpPut]
        [Route("WithdrawRegistration")]
        public async Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model)
        {
            return await _registrationService.WithdrawRegistrationAsync(model);
        }

        [HttpPut]
        [Route("ReJoinRegistration")]
        public async Task<bool> ReJoinRegistrationAsync(ReJoinRegistrationRequest model)
        {
            return await _registrationService.ReJoinRegistrationAsync(model);
        }
    }
}