using AutoMapper;
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
            return (setupCount > 0);
        }

        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            var providerNames = await _tlProviderRepository
                .GetManyAsync(x => isExactMatch ? 
                                    x.DisplayName.ToLower().Equals(name.ToLower()) : 
                                    x.DisplayName.ToLower().StartsWith(name.ToLower()))
                .OrderBy(o => o.DisplayName)
                .Select(x => new ProviderMetadata { Id = x.Id, DisplayName = x.DisplayName })
                .ToListAsync();

            return providerNames;
        }

        public Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var result = _tqProviderRepository.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
            return result;
        }

        public async Task<bool> AddProviderTlevelsAsync(List<SelectProviderTlevel> model)
        {           
            if (model == null || !model.Any()) return false;
            var newTlevels = _mapper.Map<List<TqProvider>>(model);            
            return await _tqProviderRepository.CreateManyAsync(newTlevels) > 0;
        }
    }
}
