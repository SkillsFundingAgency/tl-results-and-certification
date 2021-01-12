using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class ResultRepository : GenericRepository<TqPathwayAssessment>, IResultRepository
    {
        private ILogger<ResultRepository> _logger;

        public ResultRepository(ILogger<ResultRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TqRegistrationPathway>> GetBulkResultsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers)
        {
            var registrations = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.AssessmentSeries)                   
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)                  
                   .Where(p => uniqueLearnerNumbers.Contains(p.TqRegistrationProfile.UniqueLearnerNumber) &&
                          p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                          (p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn))
                   .ToListAsync();

            if (registrations == null) return null;

            var latestRegistratons = registrations
                    .GroupBy(x => x.TqRegistrationProfileId)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                    .ToList();

            foreach (var reg in latestRegistratons)
            {
                Func<TqPathwayAssessment, bool> pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
                if (reg.Status == RegistrationPathwayStatus.Withdrawn)
                    pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;
                reg.TqPathwayAssessments = reg.TqPathwayAssessments.Where(pathwayAssessmentPredicate).ToList();                
            }

            return latestRegistratons;
        }
    }
}
