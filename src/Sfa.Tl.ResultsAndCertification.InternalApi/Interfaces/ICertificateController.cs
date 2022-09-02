using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ICertificateController
    {
        Task<Batch> GetLearnerResultsForPrintingAsync();
    }
}