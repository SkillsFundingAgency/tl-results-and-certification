using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IRegistrationController
    {
        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
        Task<RegistrationDetails> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);
        Task<ManageRegistration> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<bool> UpdateRegistrationAsync(ManageRegistration model);
        Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model);
    }
}
