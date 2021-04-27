using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ILearnerRecordService
    {
        Task<IList<RegisteredLearnerDetails>> GetPendingGenderLearnersAsync();
        Task<IList<RegisteredLearnerDetails>> GetPendingVerificationAndLearningEventsLearnersAsync();
        Task<LearnerVerificationAndLearningEventsResponse> ProcessLearnerRecordsAsync(List<LearnerRecordDetails> learnerRecords);
        Task<LearnerGenderResponse> ProcessLearnerGenderAsync(List<LearnerRecordDetails> learnerRecords);
    }
}
