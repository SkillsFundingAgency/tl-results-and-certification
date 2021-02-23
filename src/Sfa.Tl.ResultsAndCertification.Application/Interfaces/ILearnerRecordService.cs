using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ILearnerRecordService
    {
        Task<IList<RegistrationLearnerDetails>> GetValidRegistrationLearners();
        Task<bool> ProcessLearnerRecords(List<LearnerRecordDetails> learnerRecords);
    }
}
