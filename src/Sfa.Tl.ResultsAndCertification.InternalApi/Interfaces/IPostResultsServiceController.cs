using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IPostResultsServiceController
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln);
        Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkPrn, int profileId);
    }
}