using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface ICertificatePrintingService
    {
        Task<CertificatePrintingResponse> ProcessPrintingRequestAsync();
        Task<CertificatePrintingResponse> ProcessBatchSummaryAsync();
        Task<CertificatePrintingResponse> ProcessTrackBatchAsync();
    }
}
