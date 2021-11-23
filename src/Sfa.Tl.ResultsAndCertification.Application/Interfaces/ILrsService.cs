using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ILrsService
    {
        Task<IList<RegisteredLearnerDetails>> GetPendingGenderLearnersAsync();
        Task<IList<RegisteredLearnerDetails>> GetPendingVerificationAndLearningEventsLearnersAsync();
        Task<LrsLearnerVerificationAndLearningEventsResponse> ProcessLearnerRecordsAsync(List<LrsLearnerRecordDetails> learnerRecords);
        Task<LrsLearnerGenderResponse> ProcessLearnerGenderAsync(List<LrsLearnerRecordDetails> learnerRecords);
    }
}
