using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IIndustryPlacementService
    {
        Task<IList<IndustryPlacementRecordResponse>> ValidateIndustryPlacementsAsync(long providerUkprn, IEnumerable<IndustryPlacementCsvRecordResponse> csvIndustryPlacements);
        Task<bool> ProcessIndustryPlacementDetailsAsync(IndustryPlacementRequest request);
        IList<Domain.Models.IndustryPlacement> TransformIndustryPlacementsModel(IList<IndustryPlacementRecordResponse> industryPlacementsData, string performedBy);
        Task<IndustryPlacementProcessResponse> CompareAndProcessIndustryPlacementsAsync(IList<Domain.Models.IndustryPlacement> industryPlacementsToProcess);
        Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId);
        Task<FunctionResponse> ProcessIndustryPlacementExtractionsAsync();
    }
}