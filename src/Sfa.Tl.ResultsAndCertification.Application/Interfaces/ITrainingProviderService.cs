using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {
        Task<SearchLearnerFilters> GetSearchLearnerFiltersAsync(long providerUkprn); 
        Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request);
        Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln);
        Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId);
        Task<bool> UpdateLearnerSubjectAsync(UpdateLearnerSubjectRequest request);
    }
}