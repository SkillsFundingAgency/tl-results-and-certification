using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ICommonController
    {
        IEnumerable<LookupData> GetLookupDataAsync(LookupCategory lookupCategory);
    }
}
