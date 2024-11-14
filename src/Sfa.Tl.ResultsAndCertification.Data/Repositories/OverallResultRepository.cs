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
    public class OverallResultRepository : GenericRepository<OverallResult>, IOverallResultRepository
    {
        public OverallResultRepository(ILogger<OverallResultRepository> logger, ResultsAndCertificationDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IList<OverallResult>> GetOverallResults(long providerUkprn, DateTime resultPublishDate)
        {
            var results = await _dbContext.OverallResult
                            .Include(r => r.TqRegistrationPathway.TqRegistrationProfile)
                            .Include(r => r.TqRegistrationPathway.TqPathwayAssessments.Where(a => a.IsOptedin))
                                .ThenInclude(r => r.AssessmentSeries)
                            .Include(r => r.TqRegistrationPathway.TqProvider)
                                .ThenInclude(r => r.TlProvider)
                            .Include(r => r.TqRegistrationPathway.TqRegistrationSpecialisms.Where(r => r.IsOptedin))
                                .ThenInclude(r => r.TqSpecialismAssessments.Where(r => r.IsOptedin))
                                .ThenInclude(r => r.AssessmentSeries)
                            .Include(r => r.TqRegistrationPathway.TqRegistrationSpecialisms.Where(p => p.IsOptedin))
                                .ThenInclude(s => s.TlSpecialism)
                                .ThenInclude(s => s.TlDualSpecialismToSpecialisms)
                                .ThenInclude(s => s.DualSpecialism)
                             .Where(x => x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                         x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == providerUkprn &&
                                         x.PublishDate == resultPublishDate && DateTime.Today >= resultPublishDate &&
                                         x.IsOptedin && !x.EndDate.HasValue)
                            .OrderBy(x => x.TqRegistrationPathway.TqRegistrationProfile.Lastname)
                            .ToListAsync();
            return results;
        }

        public async Task<OverallResult> GetLearnerOverallResults(long providerUkprn, long profileId)
        {
            var results = await _dbContext.OverallResult
                            .Include(r => r.TqRegistrationPathway.TqRegistrationProfile)
                            .Include(r => r.TqRegistrationPathway.TqPathwayAssessments.Where(a => a.IsOptedin))
                                .ThenInclude(r => r.AssessmentSeries)
                            .Include(r => r.TqRegistrationPathway.TqProvider)
                                .ThenInclude(r => r.TlProvider)
                            .Include(r => r.TqRegistrationPathway.TqRegistrationSpecialisms.Where(r => r.IsOptedin))
                                .ThenInclude(r => r.TqSpecialismAssessments.Where(r => r.IsOptedin))
                                .ThenInclude(r => r.AssessmentSeries)
                            .Include(r => r.TqRegistrationPathway.TqRegistrationSpecialisms.Where(p => p.IsOptedin))
                                .ThenInclude(s => s.TlSpecialism)
                                .ThenInclude(s => s.TlDualSpecialismToSpecialisms)
                                .ThenInclude(s => s.DualSpecialism)
                             .Where(x => x.TqRegistrationPathway.TqRegistrationProfile.Id == profileId &&
                                         x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                         x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn == providerUkprn &&
                                         x.IsOptedin && !x.EndDate.HasValue && DateTime.Today >= x.PublishDate)
                             .FirstOrDefaultAsync();
            return results;
        }
    }
}