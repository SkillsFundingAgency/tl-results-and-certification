﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAdminDashboardController
    {
        Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync();

        Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request);
    }
}