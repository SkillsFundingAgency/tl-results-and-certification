using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface ITrainingProviderController
    {
        Task<FindUlnResponse> FindProvidersUlnAsync(long providerUkprn, long uln);
    }
}
