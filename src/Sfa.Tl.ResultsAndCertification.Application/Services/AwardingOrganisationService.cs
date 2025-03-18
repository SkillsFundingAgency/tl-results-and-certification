using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AwardingOrganisationService : IAwardingOrganisationService
    {
        private readonly IRepository<TlAwardingOrganisation> _repository;
        private readonly IMapper _mapper;

        public AwardingOrganisationService(IRepository<TlAwardingOrganisation> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AwardingOrganisationMetadata>> GetAllAwardingOrganisationsAsync()
        {
            return await _repository.GetManyAsync()
                .OrderBy(o => o.DisplayName)
                .ProjectTo<AwardingOrganisationMetadata>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<AwardingOrganisationMetadata> GetAwardingOrganisationByUkprnAsync(long ukprn)
        {
            TlAwardingOrganisation tlAwardingOrganisation = await _repository.GetSingleOrDefaultAsync(p => p.UkPrn == ukprn);
            return _mapper.Map<AwardingOrganisationMetadata>(tlAwardingOrganisation);
        }
    }
}