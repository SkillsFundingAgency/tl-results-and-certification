﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync();
       
        Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request);

        Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request);

        Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request);

        Task<bool> RemovePathwayAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest request);

        Task<bool> RemoveSpecialismAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest request);
        
    }
}