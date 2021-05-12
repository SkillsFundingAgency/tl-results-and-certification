using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IProviderAddressLoader
    {
        Task<AddAddressSelectViewModel> GetAddressesByPostcodeAsync(string postcode);
        Task<AddressViewModel> GetAddressByUprnAsync(long uprn);
        Task<bool> AddAddressAsync(long providerUkprn, AddAddressViewModel viewModel);
    }
}
