using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IProviderAddressController
    {
        Task<bool> AddAddressAsync(AddAddressRequest request);
        Task<Address> GetAddressAsync(long providerUkprn);
    }
}
