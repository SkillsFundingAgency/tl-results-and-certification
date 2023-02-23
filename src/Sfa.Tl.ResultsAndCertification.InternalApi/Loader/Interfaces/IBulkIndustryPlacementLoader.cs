using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IBulkIndustryPlacementLoader
    {
        Task<BulkIndustryPlacementResponse> ProcessAsync(BulkProcessRequest request);
    }
}