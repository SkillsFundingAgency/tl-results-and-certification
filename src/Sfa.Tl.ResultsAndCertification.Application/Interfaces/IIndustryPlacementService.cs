using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IIndustryPlacementService
    {
        Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId);
    }
}
