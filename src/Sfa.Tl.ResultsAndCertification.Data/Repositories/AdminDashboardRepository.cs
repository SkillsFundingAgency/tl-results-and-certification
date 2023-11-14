using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AdminDashboardRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<FilterLookupData>> GetAwardingOrganisationFiltersAsync()
        {
            return await _dbContext.TlAwardingOrganisation
                    .Select(x => new FilterLookupData { Id = x.Id, Name = x.Name, IsSelected = false })
                    .ToListAsync();
        }

        public async Task<IList<FilterLookupData>> GetAcademicYearFiltersAsync(DateTime searchDate)
        {
            return await _dbContext.AcademicYear
                    .Where(x => searchDate >= x.EndDate || (searchDate >= x.StartDate && searchDate <= x.EndDate))
                    .OrderBy(x => x.Year)
                    .Take(4)
                    .Select(x => new FilterLookupData { Id = x.Year, Name = $"{x.Year} to {x.Year + 1}", IsSelected = false })
                    .ToListAsync();
        }
    }
}