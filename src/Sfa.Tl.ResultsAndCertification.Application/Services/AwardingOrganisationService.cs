using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AwardingOrganisationService : IAwardingOrganisationService
    {
        private readonly IDbContextBuilder _builder;
        private readonly ILogger<IRepository<TqAwardingOrganisation>> _logger;
        private readonly IRepository<TqAwardingOrganisation> _awardingOrganisationRepository;

        public AwardingOrganisationService(IDbContextBuilder builder, 
            ILogger<IRepository<TqAwardingOrganisation>> logger, IRepository<TqAwardingOrganisation> _repository)
        {
            _builder = builder;
            _logger = logger;
            _awardingOrganisationRepository = _repository;
        }

        public async Task<IEnumerable<string>> GetAllTlevelsByAwardingOrganisationIdAsync(int id)
        {
            _awardingOrganisationRepository.GetManyAsync(x => x.Id == 1);
            var result =  new string[] {"Hello", "World"};
            return await Task.Run(() => result);
        }
    }
}
