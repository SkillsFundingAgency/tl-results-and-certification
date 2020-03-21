using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IProviderService
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn); 
        Task<IEnumerable<object>> GetAllProvidersByAoUkprnAsync(long ukprn);
        Task<IEnumerable<string>> FindProviderNameUriAsync(string name, bool isExactMatch);
        Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId);
    }
}
