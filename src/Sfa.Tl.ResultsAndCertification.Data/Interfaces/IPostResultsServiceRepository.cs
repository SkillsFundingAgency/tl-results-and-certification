using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IPostResultsServiceRepository
    {
        Task<TqRegistrationPathway> FindPrsLearnerRecordAsync(long aoUkprn, long uln);
    }
}
