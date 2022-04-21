using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ITrainingProviderLoader
    {
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln, bool? evaluateSendConfirmation = false);
        Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null);
        Task<AddLearnerRecordResponse> AddLearnerRecordAsync(long providerUkprn, AddLearnerRecordViewModel viewModel);
        Task<UpdateLearnerRecordResponseViewModel> ProcessIndustryPlacementQuestionUpdateAsync(long providerUkprn, UpdateIndustryPlacementQuestionViewModel viewModel);
        Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddMathsStatusViewModel model);
        Task<bool> UpdateLearnerSubjectAsync(long providerUkprn, AddEnglishStatusViewModel model);
    }
}
