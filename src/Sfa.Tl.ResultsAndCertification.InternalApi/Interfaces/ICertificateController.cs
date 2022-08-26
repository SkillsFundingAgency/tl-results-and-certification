using Sfa.Tl.ResultsAndCertification.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ICertificateController
    {
        Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync();
    }
}