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
        private readonly IDbContextBuilder _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqProvider>> _logger;

        public ProviderService(
            IProviderRepository providerRespository,
            IDbContextBuilder dbContext,
            IMapper mapper,
            ILogger<IRepository<TqProvider>> logger)
        {
            _tqProviderRepository = providerRespository;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            using (var dbContext = _dbContext.Create())
            {
                var setupCount = await dbContext.TqProvider
                    .CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn);
                
                return setupCount > 0;
            }
        }

        public async Task<IEnumerable<string>> FindProviderNameAsync(string name)
        {
            using (var context = _dbContext.Create())
            {
                var providerNames = await context.TlProvider
                    .Where(x => x.DisplayName.ToLower().Contains(name.ToLower()))
                    .Select(x => x.DisplayName).ToListAsync();

                return providerNames;
            }
        }

        public Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var result = _tqProviderRepository.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
            return result;
        }
    }
}
