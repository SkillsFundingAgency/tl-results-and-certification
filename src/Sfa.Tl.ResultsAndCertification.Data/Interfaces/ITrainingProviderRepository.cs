using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ITrainingProviderRepository
    {
        Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln);
    }
}
