using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IProviderLoader
    {
        Task<IEnumerable<object>> GetAllProvidersByUkprnAsync(long ukprn);
        Task<IEnumerable<object>> SearchByTokenAsync(string term);
    }
}
