using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<IList<AdminFilter>> GetAwardingOrganisationFiltersAsync();

        Task<IList<AdminFilter>> GetAcademicYearFiltersAsync(DateTime searchDate);

        Task<AdminLearnerRecord> GetLearnerRecordAsync(int profileId);
    }
}