using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ICertificateController
    {
        Task<IList<OverallResult>> GetLearnerResultsForPrintingAsync();
    }
}