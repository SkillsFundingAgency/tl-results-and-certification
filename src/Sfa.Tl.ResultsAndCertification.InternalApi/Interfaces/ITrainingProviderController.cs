using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ITrainingProviderController
    {
        Task<bool> FindLearnerRecordAsync(long providerUkprn, long uln);
    }
}
