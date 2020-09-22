using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<IList<RegistrationRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IEnumerable<RegistrationCsvRecordResponse> registrationsData);
        IList<TqRegistrationProfile> TransformRegistrationModel(IList<RegistrationRecordResponse> registrationsData, string performedBy);
        Task<RegistrationProcessResponse> CompareAndProcessRegistrationsAsync(IList<TqRegistrationProfile> registrations);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
        Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetails> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);
        Task<ManageRegistration> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<bool> UpdateRegistrationAsync(ManageRegistration model);
        Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model);
        Task<bool> ReJoinRegistrationAsync(ReJoinRegistrationRequest model);
    }
}
