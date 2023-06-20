using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ISpecialismResultStrategy
    {
        OverallSpecialismResultDetail GetResult(ICollection<TqRegistrationSpecialism> specialisms);
    }
}