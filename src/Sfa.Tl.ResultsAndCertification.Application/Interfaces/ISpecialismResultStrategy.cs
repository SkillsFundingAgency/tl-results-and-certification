using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ISpecialismResultStrategy
    {
        TlLookup GetResult(ICollection<TqRegistrationSpecialism> specialisms);
    }
}