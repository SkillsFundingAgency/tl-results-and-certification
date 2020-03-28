using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IProviderRepository : IRepository<TqProvider>
    {
        Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long ukprn, int providerId);
        Task<IEnumerable<TqProvider>> GetAllTqProvidersAsync(long ukprn, int providerId);
    }
}
