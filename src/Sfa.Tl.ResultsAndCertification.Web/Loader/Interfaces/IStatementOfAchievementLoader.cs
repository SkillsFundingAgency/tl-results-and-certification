using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IStatementOfAchievementLoader
    {
        Task<T> FindSoaLearnerRecordAsync<T>(long providerUkprn, long uln);
    }
}
