using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class IndustryPlacementRepository : IIndustryPlacementRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ICommonRepository _commonRepository;

        public IndustryPlacementRepository(ResultsAndCertificationDbContext dbContext, ICommonRepository commonRepository)
        {
            _dbContext = dbContext;
            _commonRepository = commonRepository;
        }

        public async Task<IList<IndustryPlacement>> ExtractIndustryPlacementAsync()
        {
            var currentAcademicYears = await _commonRepository.GetCurrentAcademicYearsAsync();
            if (currentAcademicYears == null || !currentAcademicYears.Any())
            {
                throw new ApplicationException($"Current Academic years are not found. Method: {nameof(ExtractIndustryPlacementAsync)}");
            }

            var pathwayQueryable = _dbContext.IndustryPlacement
                        .Include(x => x.TqRegistrationPathway)
                            .ThenInclude(x => x.TqRegistrationProfile)
                        .Include(x => x.TqRegistrationPathway)
                            .ThenInclude(x => x.TqProvider)
                                .ThenInclude(x => x.TqAwardingOrganisation)
                                    .ThenInclude(x => x.TlAwardingOrganisaton)
                        //.Include(x=>x.pr)
                        //    .ThenInclude(x => x.TlPathway)
                        //.Include(x => x.TqRegistrationProfile)
                        //.Include(x => x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        //.Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                        //    .ThenInclude(x => x.TqSpecialismAssessments.Where(a => a.IsOptedin && a.EndDate == null))
                        //    .Include(x => x.IndustryPlacements)
                        //.Include(x => x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null))
                        //    .ThenInclude(x => x.TlSpecialism)

                        .Where(x => x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                    x.TqRegistrationPathway.EndDate == null &&
                                    x.TqRegistrationPathway.AcademicYear == currentAcademicYears.FirstOrDefault().Year - 1)
                        .AsQueryable();

            return await pathwayQueryable.ToListAsync();
        }

    }
}
