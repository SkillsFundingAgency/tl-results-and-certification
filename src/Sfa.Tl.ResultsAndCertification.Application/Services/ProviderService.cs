using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _tqProviderRepository;
        private readonly IRepository<TlProvider> _tlProviderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqProvider>> _logger;

        public ProviderService(
            IProviderRepository providerRespository,
            IRepository<TlProvider> tlproviderRepository,
            IMapper mapper,
            ILogger<IRepository<TqProvider>> logger)
        {
            _tqProviderRepository = providerRespository;
            _tlProviderRepository = tlproviderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var setupCount = await _tqProviderRepository
                        .CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn);
            return setupCount > 0;
        }

        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            var providerNames = await _tlProviderRepository
                .GetManyAsync(p => EF.Functions.Like(p.DisplayName, isExactMatch ? $"{name.ToLower()}" : $"{name.ToLower()}%"))
                .OrderBy(o => o.DisplayName)
                .ProjectTo<ProviderMetadata>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return providerNames;
        }

        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _tqProviderRepository.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
        }

        public async Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevelDetails> model)
        {
            if (model == null || !model.Any()) return false;
            var newTlevels = _mapper.Map<IList<TqProvider>>(model);
            return await _tqProviderRepository.CreateManyAsync(newTlevels) > 0;
        }

        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var tlProviders = await _tqProviderRepository
                .GetManyAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn,
                              n => n.TlProvider).ToListAsync();

            return _mapper.Map<IList<ProviderDetails>>(tlProviders);
        }

        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var tqProviders = await _tqProviderRepository.GetAllProviderTlevelsAsync(aoUkprn, providerId);
            return  _mapper.Map<ProviderTlevels>(tqProviders); 
        }
    }
}
