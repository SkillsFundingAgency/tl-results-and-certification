using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class UcasRepository : IUcasRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ICommonRepository _commonRepository;

        public UcasRepository(ResultsAndCertificationDbContext dbContext, ICommonRepository commonRepository)
        {
            _dbContext = dbContext;
            _commonRepository = commonRepository;
        }

        public async Task<IList<TqRegistrationPathway>> GetUcasDataRecordsAsync(bool inclResults)
        {
            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();
            if (currentAcademicYears == null || !currentAcademicYears.Any())
                return null; // Todo: discuss exception or null?

            var pathwayQueryable = _dbContext.TqRegistrationPathway
                        .Include(x => x.TqProvider)
                            .ThenInclude(x => x.TqAwardingOrganisation)
                            .ThenInclude(x => x.TlPathway)
                        .Include(x => x.TqRegistrationProfile)
                        .Include(x => x.TqPathwayAssessments)
                        .Include(x => x.TqRegistrationSpecialisms)
                            .ThenInclude(x => x.TqSpecialismAssessments)
                        .Include(x => x.TqRegistrationSpecialisms)
                            .ThenInclude(x => x.TlSpecialism)
                        .Where(x => x.Status == RegistrationPathwayStatus.Active && x.EndDate == null &&
                                    x.AcademicYear == currentAcademicYears.FirstOrDefault().Year - 1)
                        .AsQueryable();

            if (inclResults)
            {
                pathwayQueryable = pathwayQueryable
                    .Include(x => x.TqPathwayAssessments)
                    .ThenInclude(x => x.TqPathwayResults);
            }

            var regPatways = await pathwayQueryable.ToListAsync();
            foreach (var regPathway in regPatways)
            {
                BuildPathwayAssessmentsAndResultsPredicate(regPathway);
                BuildSpecialismsAssessmentsAndResultsPredicate(regPathway);
            }

            return regPatways;
        }

        private static void BuildPathwayAssessmentsAndResultsPredicate(TqRegistrationPathway regPathway)
        {
            Func<TqPathwayAssessment, bool> pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
            // Note: We may have more than one active assessment entry for each AssessmentSeries, if so latest assessment entry is considered below. 
            regPathway.TqPathwayAssessments = new List<TqPathwayAssessment> { regPathway.TqPathwayAssessments.OrderByDescending(x => x.CreatedOn).FirstOrDefault(pathwayAssessmentPredicate) };

            foreach (var pathwayAssessment in regPathway.TqPathwayAssessments)
            {
                // TODO: For Amendments we need to consider PrsStatus flag to record the previous result. 
                Func<TqPathwayResult, bool> pathwayResultPredicate = e => e.IsOptedin && e.EndDate == null;
                pathwayAssessment.TqPathwayResults = pathwayAssessment.TqPathwayResults.Where(pathwayResultPredicate).ToList();
            }
        }

        private static void BuildSpecialismsAssessmentsAndResultsPredicate(TqRegistrationPathway regPathway)
        {
            Func<TqRegistrationSpecialism, bool> specialismPredicate = e => e.IsOptedin && e.EndDate == null;
            regPathway.TqRegistrationSpecialisms = regPathway.TqRegistrationSpecialisms.Where(specialismPredicate).ToList();
            foreach (var pathwaySpecialism in regPathway.TqRegistrationSpecialisms)
            {
                Func<TqSpecialismAssessment, bool> specialismAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
                // Note: We may have more than one active assessment entry for each AssessmentSeries, if so latest assessment entry is considered below. 
                pathwaySpecialism.TqSpecialismAssessments = new List<TqSpecialismAssessment> { pathwaySpecialism.TqSpecialismAssessments.OrderByDescending(x => x.CreatedOn).FirstOrDefault(specialismAssessmentPredicate) };
            }
        }
    }
}
