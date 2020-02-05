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
        private readonly ILogger<GenericRepository<TqAwardingOrganisation>> _logger;

        public AwardingOrganisationService(IDbContextBuilder builder, 
            ILogger<GenericRepository<TqAwardingOrganisation>> logger)
        {
            _builder = builder;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetAllTlevelsByAwardingOrganisationIdAsync(int id)
        {
            using (var context = _builder.Create())
            {
                var repository = new GenericRepository<TqAwardingOrganisation>(_logger, context);
                var aoTlevels =  repository.GetManyAsync(x => x.Id == id);

                // TODO: Map to common Model;
            }
            
            var result =  new string[] {"Hello", "World"};
            return await Task.Run(() => result);
        }
    }
}
