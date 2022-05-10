using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IIndustryPlacementController
    {
        Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null);
        Task<IpTempFlexNavigation> GetTempFlexNavigationAsync(int pathwayId, int academicYear);
        Task<bool> ProcessIndustryPlacementDetailsAsync(IndustryPlacementRequest request);
    }
}
