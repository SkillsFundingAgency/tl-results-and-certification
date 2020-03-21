using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IProviderController
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<object>> GetAllProvidersByAoUkprnAsync(long ukprn);
        Task<IEnumerable<string>> FindProviderNameUriAsync(string name, bool isExactMatch);
    }
}
