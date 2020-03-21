using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IProviderLoader
    {
        Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn);
        Task<IEnumerable<string>> FindProviderNameAsync(string name, bool isExactMatch);
    }
}
