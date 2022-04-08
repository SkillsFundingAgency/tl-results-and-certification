using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IPostResultsServiceController
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln);
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordByProfileIdAsync(long aoUkprn, int profileId);
        Task<bool> PrsActivityAsync(PrsActivityRequest request);
        Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request);
    }
}