using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ICertificateService
    {
        Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync();
        Task<List<CertificateResponse>> ProcessCertificatesForPrintingAsync();
    }
}