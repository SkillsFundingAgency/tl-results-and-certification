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
                .Where(pw => (pw.Status == RegistrationPathwayStatus.Active || pw.Status == RegistrationPathwayStatus.Withdrawn) &&
                             pw.AcademicYear >= resultCalculationYear - 4).ToListAsync();
            
            return registrationPathways;

            //var query = from tqPathway in _dbContext.TqRegistrationPathway
            //            join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
            //            join ip in _dbContext.IndustryPlacement on tqPathway.TqRegistrationProfileId equals ip.Id
            //            join pwAssessment in _dbContext.TqPathwayAssessment on tqPathway.Id equals pwAssessment.TqRegistrationPathwayId
            //            join pwResult in _dbContext.TqPathwayResult on pwAssessment.Id equals pwResult.TqPathwayAssessmentId
            //            join tqSpl in _dbContext.TqRegistrationSpecialism on tqPathway.Id equals tqSpl.TqRegistrationPathwayId
            //            join splAssessment in _dbContext.TqSpecialismAssessment on tqSpl.Id equals splAssessment.TqRegistrationSpecialismId
            //            join splResult in _dbContext.TqSpecialismResult on splAssessment.Id equals splResult.TqSpecialismAssessmentId
            //            where
            //                pwAssessment.IsOptedin == true && pwAssessment.EndDate == null &&
            //                pwResult.IsOptedin == true && pwResult.EndDate == null &&
            //                tqSpl.IsOptedin == true && tqSpl.EndDate == null &&
            //                splAssessment.IsOptedin == true && splAssessment.EndDate == null &&
            //                splResult.IsOptedin == true && splResult.EndDate == null && 

            //           select new LearnerForOverallGradeCalculation  {   };

            //return new List<LearnerForOverallGradeCalculation>();
        }
    }
}
