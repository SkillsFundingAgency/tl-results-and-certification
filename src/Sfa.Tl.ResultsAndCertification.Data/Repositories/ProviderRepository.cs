using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class ProviderRepository : GenericRepository<TqProvider>, IProviderRepository
    {
        public ProviderRepository(ILogger<ProviderRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext) { }

        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            var isValidAo = await _dbContext.TlAwardingOrganisation.AnyAsync(o => o.UkPrn == aoUkprn);
            if (!isValidAo) return null;

            return await (from tlprov in _dbContext.TlProvider
                          where tlprov.Id == providerId
                          select new ProviderTlevels
                          {
                              Id = tlprov.Id,
                              DisplayName = tlprov.DisplayName,
                              Ukprn = tlprov.UkPrn,
                              Tlevels = (from tqao in _dbContext.TqAwardingOrganisation
                                         join tqprov in _dbContext.TqProvider on new { a = tqao.Id, b = providerId } equals new { a = tqprov.TqAwardingOrganisationId, b = tqprov.TlProviderId } into tlevels
                                         from result in tlevels.DefaultIfEmpty()
                                         join tlao in _dbContext.TlAwardingOrganisation on new { a = tqao.TlAwardingOrganisatonId, b = aoUkprn } equals new { a = tlao.Id, b = tlao.UkPrn }
                                         select new ProviderTlevel
                                         {
                                             TqAwardingOrganisationId = tqao.Id,
                                             TlProviderId = tlprov.Id,
                                             TlevelTitle = tqao.TlPathway.TlevelTitle,
                                             TqProviderId = result.Id
                                         }).OrderBy(o => o.TlevelTitle).ToList()
                          }).FirstOrDefaultAsync();
        }

        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long ukprn, int providerId)
        {
            var isValidAo = await _dbContext.TlAwardingOrganisation.AnyAsync(o => o.UkPrn == ukprn);
            if (!isValidAo) return null;

            return await (from tlprov in _dbContext.TlProvider
                          where tlprov.Id == providerId
                          select new ProviderTlevels
                          {
                              Id = tlprov.Id,
                              DisplayName = tlprov.DisplayName,
                              Ukprn = tlprov.UkPrn,
                              Tlevels = (from tqao in _dbContext.TqAwardingOrganisation
                                         join tqprov in _dbContext.TqProvider on new { a = tqao.Id, b = providerId } equals new { a = tqprov.TqAwardingOrganisationId, b = tqprov.TlProviderId } into tlevels
                                         from tlevelsResult in tlevels.DefaultIfEmpty()
                                         join tlao in _dbContext.TlAwardingOrganisation on new { a = tqao.TlAwardingOrganisatonId, b = ukprn } equals new { a = tlao.Id, b = tlao.UkPrn }
                                         where tlevelsResult == null
                                         select new ProviderTlevel
                                         {
                                             TqAwardingOrganisationId = tqao.Id,
                                             TlProviderId = tlprov.Id,
                                             TlevelTitle = tqao.TlPathway.TlevelTitle
                                         }).OrderBy(o => o.TlevelTitle).ToList()
                          }).FirstOrDefaultAsync();
        }
    }
}
