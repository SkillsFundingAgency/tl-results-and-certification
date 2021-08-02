using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IPrintingApiClient
    {
        Task<string> GetTokenAsync();
        Task<PrintResponse> ProcessPrintRequestAsync(PrintRequest printRequest);
        Task<BatchSummaryResponse> GetBatchSummaryInfoAsync(int batchNumber);
        Task<TrackBatchResponse> GetTrackBatchInfoAsync(int batchNumber);        
    }
}
