using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
        Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln);
        Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
        Task<bool> UpdateRegistrationAsync(ManageRegistration model);
        Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId);
        Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model);
        Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model);
        Task<bool> ReregistrationAsync(ReregistrationRequest model);

        IList<TqRegistrationProfile> TransformRegistrationModel(IList<RegistrationRecordResponse> registrationsData, string performedBy);
        Task<RegistrationProcessResponse> CompareAndProcessRegistrationsAsync(IList<TqRegistrationProfile> registrations);
        Task<IList<RegistrationRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IEnumerable<RegistrationCsvRecordResponse> registrationsData);
    }
}
