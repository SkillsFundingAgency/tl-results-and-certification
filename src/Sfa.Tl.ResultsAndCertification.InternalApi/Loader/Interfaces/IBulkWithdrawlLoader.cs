using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IBulkWithdrawlLoader
    {
        Task<BulkProcessResponse> ProcessAsync(BulkProcessRequest request);
    }
}
