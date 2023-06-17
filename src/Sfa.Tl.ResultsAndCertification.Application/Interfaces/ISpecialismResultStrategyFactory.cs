using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ISpecialismResultStrategyFactory
    {
        Task<ISpecialismResultStrategy> GetSpecialismResultStrategyAsync(IEnumerable<TlLookup> tlLookup, IEnumerable<TqRegistrationSpecialism> specialisms);
    }
}