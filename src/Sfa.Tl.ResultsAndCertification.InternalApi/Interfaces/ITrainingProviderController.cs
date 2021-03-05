using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ITrainingProviderController
    {
        Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln);
    }
}
