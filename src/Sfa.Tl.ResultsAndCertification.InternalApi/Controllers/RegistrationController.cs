using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase, IRegistrationController
    {
        private readonly IRegistrationService _registrationService;
        private readonly IBulkProcessLoader _bulkRegistrationProcess;
        private readonly IBulkWithdrawalLoader _bulkWithdrawalProcess;

        public RegistrationController(IRegistrationService registrationService, IBulkProcessLoader bulkRegistrationProcess, IBulkWithdrawalLoader bulkWithdrawalProcess)
        {
            _registrationService = registrationService;
            _bulkRegistrationProcess = bulkRegistrationProcess;
            _bulkWithdrawalProcess = bulkWithdrawalProcess;
        }

        [HttpPost]
        [Route("ProcessBulkRegistrations")]
        public async Task<BulkProcessResponse> ProcessBulkRegistrationsAsync(BulkProcessRequest request)
        {
            return await _bulkRegistrationProcess.ProcessAsync(request);
        }

        [HttpPost]
        [Route("ProcessBulkWithdrawals")]
        public async Task<BulkProcessResponse> ProcessBulkWithdrawalsAsync(BulkProcessRequest request)
        {
            return await _bulkWithdrawalProcess.ProcessAsync(request);
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
        [Route("GetRegistrationDetails/{aoUkprn}/{profileId}/{status:int?}")]
        public async Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            return await _registrationService.GetRegistrationDetailsAsync(aoUkprn, profileId, status);
        }

        [HttpDelete]
        [Route("DeleteRegistration/{aoUkprn}/{profileId}")]
        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            return await _registrationService.DeleteRegistrationAsync(aoUkprn, profileId);
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
        [Route("RejoinRegistration")]
        public async Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model)
        {
            return await _registrationService.RejoinRegistrationAsync(model);
        }

        [HttpPost]
        [Route("Reregistration")]
        public async Task<bool> ReregistrationAsync(ReregistrationRequest model)
        {
            return await _registrationService.ReregistrationAsync(model);
        }

        [HttpPut]
        [Route("SetRegistrationAsPendingWithdrawal")]
        public async Task<bool> SetRegistrationAsPendingWithdrawalAsync(SetRegistrationAsPendingWithdrawalRequest model)
        {
            return await _registrationService.SetRegistrationAsPendingWithdrawalAsync(model);
        }

        [HttpPut]
        [Route("ReinstateRegistrationFromPendingWithdrawal")]
        public async Task<bool> ReinstateRegistrationFromPendingWithdrawalAsync(ReinstateRegistrationFromPendingWithdrawalRequest model)
        {
            return await _registrationService.ReinstateRegistrationFromPendingWithdrawalAsync(model);
        }

        [HttpPost]
        [Route("ProcessChangeAcademicYear/{profileId}")]
        public async Task<bool> ProcessChangeAcademicYearAsync(ChangeAcademicYearRequest model, int profileId)
        {
            return await _registrationService.ProcessChangeAcademicYearAsync(model);
        }
    }
}