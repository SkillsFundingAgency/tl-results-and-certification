using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {        
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln);
        Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request);
        Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest model);
    }
}