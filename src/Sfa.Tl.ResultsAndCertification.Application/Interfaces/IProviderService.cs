using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IProviderService
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn); 
        Task<IEnumerable<object>> GetAllProvidersByAoUkprnAsync(long ukprn);
        Task<IEnumerable<string>> FindProviderNameAsync(string name, bool isExactMatch);
    }
}
