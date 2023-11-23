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

        public async Task<IList<AdminFilter>> GetAwardingOrganisationFiltersAsync()
        {
            return await _dbContext.TlAwardingOrganisation
                .OrderBy(x => x.DisplayName)
                .Select(x => new AdminFilter { Id = x.Id, Name = x.DisplayName, IsSelected = false })
                .ToListAsync();
        }

        public async Task<IList<AdminFilter>> GetAcademicYearFiltersAsync(DateTime searchDate)
        {
            return await _dbContext.AcademicYear
                .Where(x => searchDate >= x.EndDate || (searchDate >= x.StartDate && searchDate <= x.EndDate))
                .OrderByDescending(x => x.Year)
                .Take(5)
                .Select(x => new AdminFilter { Id = x.Year, Name = $"{x.Year} to {x.Year + 1}", IsSelected = false })
                .ToListAsync();
        }
    }
}