using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(AddLearnerRecordRequest request);
    }
}