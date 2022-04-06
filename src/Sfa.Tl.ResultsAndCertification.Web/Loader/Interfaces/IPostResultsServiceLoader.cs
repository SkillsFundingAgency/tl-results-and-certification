using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IPostResultsServiceLoader
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId, ComponentType componentType);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeViewModel model);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsAddRommOutcomeKnownViewModel model);
        Task<bool> PrsRommActivityAsync(long aoUkprn, PrsRommCheckAndSubmitViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAddAppealOutcomeKnownViewModel model);
        Task<bool> PrsAppealActivityAsync(long aoUkprn, PrsAppealCheckAndSubmitViewModel model);
        Task<bool> AppealCoreGradeAsync(long aoUkprn, PrsAddAppealViewModel model);
        Task<bool> AppealCoreGradeAsync(long aoUkprn, PrsPathwayGradeCheckAndSubmitViewModel model);
        Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequestViewModel model);
        T TransformLearnerDetailsTo<T>(FindPrsLearnerRecord prsLearnerRecord);
        Task<bool> AppealGradeAfterDeadlineRequestAsync(AppealGradeAfterDeadlineConfirmViewModel model);
    }
}