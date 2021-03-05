using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class TrainingProviderRepository : ITrainingProviderRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln)
        {
            var latestPathway = await _dbContext.TqRegistrationPathway
                            .Include(x => x.TqProvider)
                                .ThenInclude(x => x.TlProvider)
                            .Where(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln)
                            .OrderByDescending(o => o.CreatedOn)
                            .FirstOrDefaultAsync();

            if (latestPathway == null)
                return false;

            return latestPathway.TqProvider.TlProvider.UkPrn == providerUkprn;
        }
    }
}
