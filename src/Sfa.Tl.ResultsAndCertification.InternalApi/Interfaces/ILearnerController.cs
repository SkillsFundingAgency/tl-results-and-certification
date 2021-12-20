using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ILearnerController
    {
        Task<LearnerRecord> GetLearnerRecordAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
    }
}
