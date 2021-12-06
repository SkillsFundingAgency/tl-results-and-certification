using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
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
                   .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.AssessmentSeries)
                    .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.TqPathwayResults)
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TlProvider)
                   .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TlSpecialism)
                            .ThenInclude(x => x.TlPathwaySpecialismCombinations)
                   .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TqSpecialismAssessments)
                            .ThenInclude(x => x.AssessmentSeries)
                    .Include(x => x.IndustryPlacements)
                    .OrderByDescending(o => o.CreatedOn)
                    .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId &&
                           p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                           (
                                p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn
                           ));


            if (regPathway == null) return null;

            Func<TqPathwayAssessment, bool> pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
            if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;
            regPathway.TqPathwayAssessments = regPathway.TqPathwayAssessments.Where(pathwayAssessmentPredicate).ToList();

            // PathwaySpecialism
            Func<TqRegistrationSpecialism, bool> specialismPredicate = e => e.IsOptedin && e.EndDate == null;
            if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                specialismPredicate = e => e.IsOptedin && e.EndDate != null;
            regPathway.TqRegistrationSpecialisms = regPathway.TqRegistrationSpecialisms.Where(specialismPredicate).ToList();

            foreach (var specialism in regPathway.TqRegistrationSpecialisms)
            {
                // SpecialismAssessment
                Func<TqSpecialismAssessment, bool> specialismAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
                if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                    specialismAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;

                specialism.TqSpecialismAssessments = specialism.TqSpecialismAssessments.Where(specialismAssessmentPredicate).ToList();
            }

            foreach (var pathwayAssessment in regPathway.TqPathwayAssessments)
            {
                // TqPathwayResults
                Func<TqPathwayResult, bool> pathwayResultPredicate = e => e.IsOptedin && e.EndDate == null;
                if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                    pathwayResultPredicate = e => e.IsOptedin && e.EndDate != null;

                pathwayAssessment.TqPathwayResults = pathwayAssessment.TqPathwayResults.Where(pathwayResultPredicate).ToList();
            }

            return regPathway;
        }
    }
}
