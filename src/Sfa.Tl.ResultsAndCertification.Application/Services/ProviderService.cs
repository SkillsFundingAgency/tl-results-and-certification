using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    // TODO: Please note dev in progress this class need to be refactored.
    public class ProviderService : IProviderService
    {
        private readonly ResultsAndCertificationConfiguration _config;
        private readonly IProviderRepository _tqProviderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqProvider>> _logger;

        public ProviderService(
            ResultsAndCertificationConfiguration config,
            IProviderRepository providerRespository,
            IMapper mapper,
            ILogger<IRepository<TqProvider>> logger)
        {
            _config = config;
            _tqProviderRepository = providerRespository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var count = await _tqProviderRepository.CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn);
            return (count > 0);
        }

        public async Task<IEnumerable<string>> FindProviderNameAsync(string name, bool isExactMatch)
        {
            // TODO: check how will fit into our generic repository framework. 
            using (var context = CreateDbContext())
            {
                var providerNames = await context.TlProvider
                    .Where(x => isExactMatch ? x.DisplayName.ToLower().Equals(name.ToLower())
                            : x.DisplayName.ToLower().Contains(name.ToLower())) // TODO: should be StartWith
                    .Select(x => x.DisplayName).ToListAsync();

                return providerNames;
            }
        }

        public Task<IEnumerable<object>> GetAllProvidersByAoUkprnAsync(long ukprn)
        {
            throw new NotImplementedException();
        }

        public Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var result = _tqProviderRepository.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
            return result;
        }

        private ResultsAndCertificationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>();
            optionsBuilder.UseSqlServer(_config.SqlConnectionString);

            return new ResultsAndCertificationDbContext(optionsBuilder.Options);
        }
    }
}
