using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AssessmentRepository : GenericRepository<TqPathwayAssessment>, IAssessmentRepository
    {
        private ILogger<AssessmentRepository> _logger;

        public AssessmentRepository(ILogger<AssessmentRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }
    }
}
