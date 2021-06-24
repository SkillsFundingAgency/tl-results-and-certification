using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IPostResultsServiceRepository
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln);
        Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkprn, int profileId, int assessmentSeriesId);
    }
}
