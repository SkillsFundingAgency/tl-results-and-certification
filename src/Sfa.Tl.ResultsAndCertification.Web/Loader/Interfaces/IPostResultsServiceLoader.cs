using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IPostResultsServiceLoader
    {
        Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln);
        Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentId);
    }
}