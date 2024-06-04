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
    public class ProviderRegistrationsRepository : IProviderRegistrationsRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public ProviderRegistrationsRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<int>> GetAvailableStartYearsAsync(Func<DateTime> getToday)
        {
            DateTime today = getToday();

            IQueryable<int> availableStartYearsQuery = _dbContext.AcademicYear
                .Where(x => today >= x.EndDate || (today >= x.StartDate && today <= x.EndDate))
                .OrderByDescending(x => x.Year)
                .Take(5)
                .OrderBy(x => x.Year)
                .Select(x => x.Year);

            return await availableStartYearsQuery.ToListAsync();
        }

        public async Task<IList<TqRegistrationPathway>> GetRegistrationsAsync(long providerUkprn, int startYear)
        {
            IQueryable<TqRegistrationPathway> registrationsQuery = _dbContext.TqRegistrationPathway
               .Include(x => x.TqRegistrationProfile)
               .Include(x => x.TqProvider)
                   .ThenInclude(x => x.TqAwardingOrganisation)
                   .ThenInclude(x => x.TlPathway)
               .Include(x => x.IndustryPlacements)
               .Include(x => x.TqRegistrationSpecialisms.Where(r => r.IsOptedin && r.EndDate == null))
                    .ThenInclude(x => x.TlSpecialism)
               .Where(x => x.TqProvider.TlProvider.UkPrn == providerUkprn && x.AcademicYear == startYear && x.Status == RegistrationPathwayStatus.Active);

            return await registrationsQuery.ToListAsync();
        }
    }
}