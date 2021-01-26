using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ICommonService
    {
        IEnumerable<LookupData> GetLookupDataAsync(LookupCategory lookupCategory);
    }
}
