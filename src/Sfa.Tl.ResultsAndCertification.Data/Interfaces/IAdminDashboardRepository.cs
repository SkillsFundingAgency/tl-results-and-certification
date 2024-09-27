using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<IList<FilterLookupData>> GetAwardingOrganisationFiltersAsync();

        Task<IList<FilterLookupData>> GetAcademicYearFiltersAsync(DateTime searchDate);

        Task<TqRegistrationPathway> GetLearnerRecordAsync(int registrationPathwayId);

        Task<PagedResponse<AdminSearchLearnerDetail>> SearchLearnerDetailsAsync(AdminSearchLearnerRequest request);

        Task<IList<int>> GetAllowedChangeAcademicYearsAsync(Func<DateTime> getToday, int learnerAcademicYear, int pathwayStartYear);
    }
}