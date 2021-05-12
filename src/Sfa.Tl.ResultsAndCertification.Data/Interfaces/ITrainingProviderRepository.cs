using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ITrainingProviderRepository
    {
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln);
        Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null);
        Task<bool> IsSendConfirmationRequiredAsync(int profileId);
    }
}
