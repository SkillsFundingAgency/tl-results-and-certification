using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IPostResultsServiceLoader
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId);
        Task<bool> AppealCoreGradeAsync(long aoUkprn, AppealCoreGradeViewModel model);
        Task<bool> AppealCoreGradeAsync(long aoUkprn, PrsPathwayGradeCheckAndSubmitViewModel model); 
        T TransformLearnerDetailsTo<T>(FindPrsLearnerRecord prsLearnerRecord);
        Task<bool> WithdrawAppealCoreGradeAsync(long aoUkprn, AppealOutcomePathwayGradeViewModel model);
    }
}