﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAdminDashboardController
    {
        Task<SearchLearnerFilters> GetFiltersAsync();
    }
}