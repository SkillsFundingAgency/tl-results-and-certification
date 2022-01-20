using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class LearnerRepository : ILearnerRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public LearnerRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TqRegistrationPathway> GetLearnerRecordAsync(long aoUkprn, int profileId)
        {
            var regPathway = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                       .ThenInclude(x => x.AssessmentSeries)
                   .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                       .ThenInclude(x => x.TqPathwayResults.Where(pr => pr.IsOptedin && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pr.EndDate != null : pr.EndDate == null))
                           .ThenInclude(x => x.TlLookup)
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TlProvider)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TlSpecialism)
                           .ThenInclude(x => x.TlPathwaySpecialismCombinations)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                           .ThenInclude(x => x.AssessmentSeries)
                   .Include(x => x.IndustryPlacements)
                   .OrderByDescending(o => o.CreatedOn)
                   .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId &&
                           p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                           (
                                p.Status == RegistrationPathwayStatus.Active ||
                                p.Status == RegistrationPathwayStatus.Withdrawn
                           ));

            return regPathway;
        }
    }
}
