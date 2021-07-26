using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IPrinitingApiClient
    {
        Task<string> GetTokenAsync();
        Task<PrintRequestResponse> ProcessPrintRequestAsync(PrintRequest printRequest);
        Task<BatchSummaryResponse> GetBatchSummaryInfoAsync(int batchNumber);
        Task<TrackBatchResponse> GetTrackBatchInfoAsync(int batchNumber);        
    }
}
