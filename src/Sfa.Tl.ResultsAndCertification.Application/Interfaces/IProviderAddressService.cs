using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IProviderAddressService
    {
        Task<bool> AddAddressAsync(AddAddressRequest request);
        Task<Address> GetAddressAsync(long providerUkprn);
    }
}