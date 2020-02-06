using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
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
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<TqAwardingOrganisation>> _logger;
        
        public AwardingOrganisationService(
            IRepository<TqAwardingOrganisation> _repository,
            IMapper mapper,
            ILogger<IRepository<TqAwardingOrganisation>> logger)
        {
            _awardingOrganisationRepository = _repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync(int id)
        {
            // TODO: GetManyAsync change this to async method?
            var tlevels = _awardingOrganisationRepository.GetManyAsync(x => x.TlAwardingOrganisatonId == id).ToList();
            var awardOrgPathwayStatus = _mapper.Map<IEnumerable<AwardingOrganisationPathwayStatus>>(tlevels);
            
            return await Task.Run(() => awardOrgPathwayStatus);
        }
    }
}
