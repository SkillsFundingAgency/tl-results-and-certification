using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class OverallResultCalculationRepository : IOverallResultCalculationRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public OverallResultCalculationRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculation(int academicYearFrom, int academicYearTo)
        {
            var activeLearners = await GetLearnesByStatusAsync(RegistrationPathwayStatus.Active, academicYearFrom, academicYearTo);
            var allWithdrawnLearners = await GetLearnesByStatusAsync(RegistrationPathwayStatus.Withdrawn, academicYearFrom, academicYearTo);

            // allWithdrawnLearners may contain activeLearnes as well, if so exclude them.
            var withdrawnLearners = allWithdrawnLearners.Where(w => !activeLearners.Select(x => x.TqRegistrationProfile.UniqueLearnerNumber)
                            .Contains(w.TqRegistrationProfile.UniqueLearnerNumber));

            return activeLearners.Concat(withdrawnLearners).ToList();
        }

        private async Task<IList<TqRegistrationPathway>> GetLearnesByStatusAsync(RegistrationPathwayStatus status, int academicYearFrom, int academicYearTo)
        {
            var learners = await _dbContext.TqRegistrationPathway
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                    .ThenInclude(x => x.TqPathwayResults.Where(pr => pr.IsOptedin && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pr.EndDate != null : pr.EndDate == null))
                .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? s.EndDate != null : s.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                        .ThenInclude(x => x.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sr.EndDate != null : sr.EndDate == null))
                .Include(x => x.OverallResults.Where(o => o.EndDate == null))
                .Where(pw => (pw.Status == status) &&
                             pw.AcademicYear >= academicYearFrom && pw.AcademicYear <= academicYearTo &&
                             (!pw.OverallResults.Any() || pw.OverallResults.Any(o => o.EndDate == null && (pw.IndustryPlacements.Any(ip => ip.CreatedOn > o.CreatedOn || ip.ModifiedOn > o.CreatedOn) ||
                             pw.TqPathwayAssessments.SelectMany(pa => pa.TqPathwayResults).Any(pr => pr.CreatedOn > o.CreatedOn || pr.ModifiedOn > o.CreatedOn) ||
                             pw.TqRegistrationSpecialisms.SelectMany(s => s.TqSpecialismAssessments.SelectMany(sa => sa.TqSpecialismResults)).Any(sr => sr.CreatedOn > o.CreatedOn || sr.ModifiedOn > o.CreatedOn)
                             ))))
                .GroupBy(x => x.TqRegistrationProfile.UniqueLearnerNumber)
                .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                .ToListAsync();

            return learners;
        }
    }
}
