using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class PrintingToFileClient : IPrintingApiClient
    {
        public PrintingToFileClient()
        {
        }

        public Task<BatchSummaryResponse> GetBatchSummaryInfoAsync(int batchNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetTokenAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<TrackBatchResponse> GetTrackBatchInfoAsync(int batchNumber)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PrintResponse> ProcessPrintRequestAsync(PrintRequest printRequest)
        {
            var result = JsonConvert.SerializeObject(printRequest, Formatting.Indented);
            await File.WriteAllTextAsync($@"c:\temp\functions\PrintRequestData_{printRequest.Batch?.BatchNumber}.json", result);

            return new PrintResponse { PrintRequestResponse = new PrintRequestResponse { BatchId = 11, BatchNumber = 22, Status = "1" } };
        }
    }
}
