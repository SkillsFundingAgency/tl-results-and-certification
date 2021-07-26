using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IPrintingService
    {
        Task<IList<PrintRequest>> GetPendingPrintRequestsAsync();
        Task<CertificatePrintingResponse> UpdatePrintReqeustResponsesAsync(List<PrintRequestResponse> printRequestResponses);
    }
}
