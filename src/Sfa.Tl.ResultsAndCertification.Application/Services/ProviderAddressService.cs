using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ProviderAddressService : IProviderAddressService
    {
        private readonly IRepository<TlProvider> _tlProvider;
        private readonly IRepository<TlProviderAddress> _tlProviderAddress;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProviderAddressService(IRepository<TlProvider> tlProvider, IRepository<TlProviderAddress> tlProviderAddress, IMapper mapper, ILogger<ProviderAddressService> logger)
        {
            _tlProvider = tlProvider;
            _tlProviderAddress = tlProviderAddress;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAddressAsync(AddAddressRequest request)
        {
            var tlProvider = await _tlProvider.GetFirstOrDefaultAsync(p => p.UkPrn == request.Ukprn && p.IsActive);

            if (tlProvider == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Provider not found. ProviderUkprn = {request.Ukprn}. Method: AddAddressAsync({request})");
                return false;
            }

            var providerAddress = _mapper.Map<TlProviderAddress>(request, opt => opt.Items["providerId"] = tlProvider.Id);
            return await _tlProviderAddress.CreateAsync(providerAddress) > 0;            
        }

        public async Task<Address> GetAddressAsync(long providerUkprn)
        {
            var address = await _tlProviderAddress.GetFirstOrDefaultAsync(pa => pa.TlProvider.UkPrn == providerUkprn && pa.IsActive, pa => pa, pa => pa.CreatedOn, false);
            return _mapper.Map<Address>(address);
        }
    }
}