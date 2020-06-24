using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<TqRegistrationProfile>, IRegistrationRepository
    {
        public RegistrationRepository(ILogger<RegistrationRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext) { }

        public async Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations)
        {
            var ulns = new HashSet<long>();
            registrations.ToList().ForEach(r => ulns.Add(r.UniqueLearnerNumber));

            return await _dbContext.TqRegistrationProfile.Where(x => ulns.Contains(x.UniqueLearnerNumber))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqProvider)
                        .ThenInclude(x => x.TqAwardingOrganisation)
                .ToListAsync();
        }
    }
}
