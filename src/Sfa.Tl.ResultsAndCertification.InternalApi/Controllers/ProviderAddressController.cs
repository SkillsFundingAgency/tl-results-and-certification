using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderAddressController : ControllerBase, IProviderAddressController
    {
        protected IProviderAddressService _providerAddressService;

        public ProviderAddressController(IProviderAddressService providerAddressService)
        {
            _providerAddressService = providerAddressService;
        }

        [HttpPost]
        [Route("AddAddress")]
        public async Task<bool> AddAddressAsync(AddAddressRequest request)
        {
            return await _providerAddressService.AddAddressAsync(request);
        }

        [HttpGet]
        [Route("GetAddress")]
        public async Task<Address> GetAddressAsync(long providerUkprn)
        {
            return await _providerAddressService.GetAddressAsync(providerUkprn);
        }
    }
}