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
        private readonly IRepository<TqProvider> _tlProviderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqAwardingOrganisation>> _logger;

        public ProviderService(
            ResultsAndCertificationConfiguration config,
            IRepository<TqProvider> repository,
            IMapper mapper,
            ILogger<IRepository<TqAwardingOrganisation>> logger)
        {
            _config = config;
            _tlProviderRepository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            var count = await _tlProviderRepository.CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn);
            return (count > 0);
        }

        public async Task<IEnumerable<string>> FindProviderNameUriAsync(string name, bool isExactMatch)
        {
            // TODO: check how will fit into our generic repository framework. 
            using (var context = CreateDbContext())
            {
                var providerNames = await context.TlProvider
                    .Where(x => isExactMatch ? x.DisplayName.ToLower().Equals(name.ToLower())
                            : x.DisplayName.ToLower().StartsWith(name.ToLower()))
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
            throw new NotImplementedException();
        }

        private ResultsAndCertificationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>();
            optionsBuilder.UseSqlServer(_config.SqlConnectionString);

            return new ResultsAndCertificationDbContext(optionsBuilder.Options);
        }
    }
}
