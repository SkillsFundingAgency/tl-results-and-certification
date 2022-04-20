using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {        
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln, bool? evaluateSendConfirmation = false);
        Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request);
        Task<bool> UpdateLearnerRecordAsync(UpdateLearnerRecordRequest model);
        Task<bool> UpdateLearnerSubjectRecordAsync(UpdateLearnerSubjectRecordRequest request);
    }
}