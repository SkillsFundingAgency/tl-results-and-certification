using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IRegistrationController
    {
        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request);
        Task<bool> AddRegistrationAsync(RegistrationRequest model);
    }
}
