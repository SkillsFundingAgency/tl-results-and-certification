using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PathwayService : IPathwayService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly IMapper _mapper;

        public PathwayService(IRepository<TlPathway> pathwayRepository, IMapper mapper)
        {
            _pathwayRepository = pathwayRepository;
            _mapper = mapper;
        }

        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long aoUkprn, int pathwayId)
        {
            var tlevel = await _pathwayRepository.GetFirstOrDefaultAsync(p => p.Id == pathwayId && 
                                                                         p.TqAwardingOrganisations.Any(x => x.TlPathwayId == p.Id && x.TlAwardingOrganisaton.UkPrn == aoUkprn),
                                                                         navigationPropertyPath: new Expression<Func<TlPathway, object>>[] { r => r.TlRoute, s => s.TlSpecialisms, s => s.TqAwardingOrganisations });
            return _mapper.Map<TlevelPathwayDetails>(tlevel);
        }

        public async Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            var pathway = await _pathwayRepository.GetFirstOrDefaultAsync(p => p.LarId == pathwayLarId  &&
                                                                         p.TqAwardingOrganisations.Any(x => x.TlPathwayId == p.Id && x.TlAwardingOrganisaton.UkPrn == aoUkprn),
                                                                         navigationPropertyPath: new Expression<Func<TlPathway, object>>[] { s => s.TlSpecialisms });
            return _mapper.Map<PathwaySpecialisms>(pathway);
        }
    }
}
