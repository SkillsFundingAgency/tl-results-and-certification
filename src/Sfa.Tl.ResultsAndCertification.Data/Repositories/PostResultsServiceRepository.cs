using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class PostResultsServiceRepository : IPostResultsServiceRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public PostResultsServiceRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            await Task.CompletedTask;
            return new FindPrsLearnerRecord { Uln = 1234567890, Status = Common.Enum.RegistrationPathwayStatus.Active };
        }
    }
}
