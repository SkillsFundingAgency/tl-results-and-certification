using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class OverallResultCalculationRepository : IOverallResultCalculationRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;
        private readonly int _numberOfAcademicYearsToProcess;


        public OverallResultCalculationRepository(ResultsAndCertificationDbContext dbContext, ResultsAndCertificationConfiguration configuration)
        {
            _dbContext = dbContext;
            _numberOfAcademicYearsToProcess = configuration.OverallResultBatchSettings.NoOfAcademicYearsToProcess;
        }

        public async Task<IList<TqRegistrationPathway>> GetLearnersForOverallGradeCalculation(AssessmentSeries assessmentSeries)
        {
            int academicYearFrom = (assessmentSeries.ResultCalculationYear ?? 0) - (_numberOfAcademicYearsToProcess <= 0 ?
                Constants.OverallResultDefaultNoOfAcademicYearsToProcess : _numberOfAcademicYearsToProcess) + 1;

            int academicYearTo = assessmentSeries.ResultCalculationYear ?? 0;

            IQueryable<TqRegistrationPathway> registrations = _dbContext.TqRegistrationPathway
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.IndustryPlacements)
                .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && !pa.EndDate.HasValue))
                    .ThenInclude(x => x.TqPathwayResults.Where(pr => pr.IsOptedin && !pr.EndDate.HasValue))
                    .ThenInclude(x => x.TlLookup)
                .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && !pa.EndDate.HasValue))
                    .ThenInclude(x => x.AssessmentSeries)
                .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && !s.EndDate.HasValue))
                    .ThenInclude(x => x.TlSpecialism)
                .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && !s.EndDate.HasValue))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && !sa.EndDate.HasValue))
                    .ThenInclude(x => x.TqSpecialismResults.Where(sr => sr.IsOptedin && !sr.EndDate.HasValue))
                    .ThenInclude(x => x.TlLookup)
                .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && !s.EndDate.HasValue))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && !sa.EndDate.HasValue))
                    .ThenInclude(x => x.AssessmentSeries)
                .Include(x => x.OverallResults.Where(o => o.IsOptedin && !o.EndDate.HasValue))
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                    .ThenInclude(x => x.TlPathway);

            // The registration is active.
            registrations = registrations.Where(r => r.Status == RegistrationPathwayStatus.Active);

            // The registration academic year between bounds.
            registrations = registrations.Where(r => r.AcademicYear >= academicYearFrom && r.AcademicYear <= academicYearTo);

            // The registration has overall results or something has changed after the overall result calculation (industry placement, core result or specialism result).
            registrations = registrations.Where(r => !r.OverallResults.Any()
                            || r.OverallResults.Any(ovr => ovr.IsOptedin && !ovr.EndDate.HasValue
                               && (r.IndustryPlacements.Any(ip => ip.CreatedOn > ovr.CreatedOn || ip.ModifiedOn > ovr.CreatedOn)
                                  || r.TqPathwayAssessments.Where(a => a.IsOptedin && !a.EndDate.HasValue)
                                                           .SelectMany(pa => pa.TqPathwayResults.Where(r => r.IsOptedin && !r.EndDate.HasValue))
                                                           .Any(pr => pr.CreatedOn > ovr.CreatedOn || pr.ModifiedOn > ovr.CreatedOn)
                                  || r.TqRegistrationSpecialisms.Where(r => r.IsOptedin && !r.EndDate.HasValue)
                                                                .SelectMany(s => s.TqSpecialismAssessments.Where(r => r.IsOptedin && !r.EndDate.HasValue)
                                                                .SelectMany(sa => sa.TqSpecialismResults.Where(r => r.IsOptedin && !r.EndDate.HasValue)))
                                                                .Any(sr => sr.CreatedOn > ovr.CreatedOn || sr.ModifiedOn > ovr.CreatedOn))));

            // The registration does not have a core assessment after the current one.
            registrations = registrations.Where(r => !r.TqPathwayAssessments.Where(a => a.IsOptedin && !a.EndDate.HasValue)
                                                       .Any(a => a.AssessmentSeries.Name != assessmentSeries.Name && a.AssessmentSeriesId > assessmentSeries.Id));

            // The registration does not have a specialism assessment after the current one.
            registrations = registrations.Where(r => !r.TqRegistrationSpecialisms.Where(r => r.IsOptedin && !r.EndDate.HasValue)
                                                       .SelectMany(a => a.TqSpecialismAssessments.Where(a => a.IsOptedin && !a.EndDate.HasValue))
                                                       .Any(a => a.AssessmentSeries.Name != assessmentSeries.Name && a.AssessmentSeriesId > assessmentSeries.Id));

            return await registrations.ToListAsync();
        }
    }
}
