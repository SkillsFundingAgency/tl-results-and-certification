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
            {
                throw new ApplicationException("Current Academic years are not found. Method: GetCurrentAcademicYearsAsync()");
            }

            var pathwayQueryable = _dbContext.TqRegistrationPathway
                        .Include(x => x.TqProvider)
                            .ThenInclude(x => x.TqAwardingOrganisation)
                            .ThenInclude(x => x.TlPathway)
                        .Include(x => x.TqRegistrationProfile)
                        .Include(x => x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                            .ThenInclude(x => x.TqSpecialismAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        .Include(x => x.TqRegistrationSpecialisms)
                            .ThenInclude(x => x.TlSpecialism)
                        .Where(x => x.Status == RegistrationPathwayStatus.Active && x.EndDate == null &&
                                    x.AcademicYear == currentAcademicYears.FirstOrDefault().Year - 1)
                        .AsQueryable();

            if (inclResults)
            {
                pathwayQueryable = pathwayQueryable
                    .Include(x => x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        .ThenInclude(x => x.TqPathwayResults.Where(r => r.IsOptedin && r.EndDate == null))
                            .ThenInclude(x => x.TlLookup)
                    .Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                        .ThenInclude(x => x.TqSpecialismAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                            .ThenInclude(x => x.TqSpecialismResults.Where(r => r.IsOptedin && r.EndDate == null))
                                .ThenInclude(x => x.TlLookup);
            }

            return await pathwayQueryable.ToListAsync();
        }
    }
}
