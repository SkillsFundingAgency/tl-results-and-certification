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

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculation(int resultCalculationYear)
        {
            var registrationPathways = await _dbContext.TqRegistrationPathway
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                    .ThenInclude(x => x.TqPathwayResults.Where(r => r.IsOptedin && r.EndDate == null))
                .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismResults.Where(r => r.IsOptedin && r.EndDate == null))
                .Include(x => x.OverallResults)
                .Where(pw => (pw.Status == RegistrationPathwayStatus.Active || pw.Status == RegistrationPathwayStatus.Withdrawn) &&
                             (pw.AcademicYear <= resultCalculationYear && pw.AcademicYear > resultCalculationYear - 4))
                .ToListAsync();
            
            return registrationPathways;
        }
    }
}
