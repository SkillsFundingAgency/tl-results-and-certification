using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IPostResultsServiceService
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null);
        Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkPrn, int profileId, int assessmentId);
        Task<bool> AppealGradeAsync(AppealGradeRequest request);
        Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request);
        Task<bool> AppealGradeAfterDeadlineRequestAsync(AppealGradeAfterDeadlineRequest request);
    }
}