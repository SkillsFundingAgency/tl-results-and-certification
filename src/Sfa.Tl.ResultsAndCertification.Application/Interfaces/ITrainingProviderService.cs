using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {
        Task<bool> FindLearnerRecordAsync(long providerUkprn, long uln);
    }
}
