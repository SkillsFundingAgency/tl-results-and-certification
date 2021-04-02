using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITrainingProviderLoader
    {
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln);
        Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(long providerUkprn, AddLearnerRecordViewModel viewModel);
        Task<UpdateLearnerRecordResponse> ProcessIndustryPlacementQuestionChangeAsync(long providerUkprn, UpdateIndustryPlacementQuestionViewModel viewModel);
    }
}
