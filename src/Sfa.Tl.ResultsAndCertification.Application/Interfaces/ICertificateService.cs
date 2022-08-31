using Sfa.Tl.ResultsAndCertification.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ICertificateService
    {
        Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync();
    }
}