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

        /// <summary>
        /// Determines whether [is any provider setup completed asynchronous] [the specified ukprn].
        /// </summary>
        /// <param name="aoUkprn">The awarding organisation ukprn.</param>
        /// <returns></returns>
        public async Task<bool> IsAnyProviderSetupCompletedAsync(long aoUkprn)
        {
            var setupCount = await _tqProviderRepository
                        .CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn);
            return setupCount > 0;
        }

        /// <summary>
        /// Finds the provider asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isExactMatch">if set to <c>true</c> [is exact match].</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            return await _tlProviderRepository
                .GetManyAsync(p => EF.Functions.Like(p.DisplayName, isExactMatch ? $"{name.ToLower()}" : $"{name.ToLower()}%"))
                .OrderBy(o => o.DisplayName)
                .ProjectTo<ProviderMetadata>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the select provider tlevels asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _tqProviderRepository.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
        }

        /// <summary>
        /// Adds the provider tlevels asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<bool> AddProviderTlevelsAsync(IList<ProviderTlevel> model)
        {
            if (model == null || !model.Any()) return false;
            var newTlevels = _mapper.Map<IList<TqProvider>>(model);
            return await _tqProviderRepository.CreateManyAsync(newTlevels) > 0;
        }

        /// <summary>
        /// Gets the tq ao provider details asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <returns></returns>
        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            var tlProviders = await _tqProviderRepository
                .GetManyAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn, n => n.TlProvider)
                .Select(p => p.TlProvider).Distinct().OrderBy(p => p.DisplayName).ToListAsync();

            return _mapper.Map<IList<ProviderDetails>>(tlProviders);
        }

        /// <summary>
        /// Gets all provider tlevels asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _tqProviderRepository.GetAllProviderTlevelsAsync(aoUkprn, providerId);
        }

        /// <summary>
        /// Gets the tq provider tlevel details asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="tqProviderId">The tq provider identifier.</param>
        /// <returns></returns>
        public async Task<ProviderTlevelDetails> GetTqProviderTlevelDetailsAsync(long aoUkprn, int tqProviderId)
        {
            var tqProvider = await _tqProviderRepository
                .GetFirstOrDefaultAsync(x => x.Id == tqProviderId && x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn,
                                        n => n.TlProvider,
                                        n => n.TqAwardingOrganisation,
                                        n => n.TqAwardingOrganisation.TlPathway);

            return _mapper.Map<ProviderTlevelDetails>(tqProvider);
        }

        /// <summary>
        /// Removes the tq provider tlevel asynchronous.
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="tqProviderId">The tq provider identifier.</param>
        /// <returns></returns>
        public async Task<bool> RemoveTqProviderTlevelAsync(long aoUkprn, int tqProviderId)
        {
            var tqProvider = await _tqProviderRepository
                .GetFirstOrDefaultAsync(x => x.Id == tqProviderId && x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn);

            if (tqProvider == null) return false;

            return await _tqProviderRepository.DeleteAsync(tqProvider) > 0;
        }


        /// <summary>
        /// Determines whether [has any tlevel setup for provider asynchronous] [the specified ao ukprn].
        /// </summary>
        /// <param name="aoUkprn">The ao ukprn.</param>
        /// <param name="tlProviderId">The tl provider identifier.</param>
        /// <returns>
        ///   <c>true</c> if [has any tlevel setup for provider asynchronous] [the specified ao ukprn]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> HasAnyTlevelSetupForProviderAsync(long aoUkprn, int tlProviderId)
        {
            return await _tqProviderRepository.CountAsync(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn && x.TlProviderId == tlProviderId) > 0;
        }
    }
}
