using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ITrainingProviderService
    {
        Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln);
    }
}
