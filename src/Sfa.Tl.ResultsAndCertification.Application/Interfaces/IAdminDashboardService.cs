﻿using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync();

        Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request);

        Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId);

        Task<IList<int>> GetAllowedChangeAcademicYearsAsync(int learnerAcademicYear, int pathwayStartYear);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request);

        Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request);

        Task<bool> ProcessChangeMathsStatusAsync(ReviewChangeMathsStatusRequest request);

        Task<bool> ProcessChangeEnglishStatusAsync(ReviewChangeEnglishStatusRequest request);

        Task<bool> ProcessAddCoreAssessmentAsync(ReviewAddCoreAssessmentRequest request);

        Task<bool> ProcessAddSpecialismAssessmentAsync(ReviewAddSpecialismAssessmentRequest request);

        Task<bool> ProcessRemovePathwayAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryRequest request);             

        Task<bool> ProcessRemoveSpecialismAssessmentEntryAsync(ReviewRemoveSpecialismAssessmentEntryRequest request);       
        
        Task<bool> ProcessAdminAddPathwayResultAsync(AddPathwayResultRequest request);

        Task<bool> ProcessAdminAddSpecialismResultAsync(AddSpecialismResultRequest request);

        Task<bool> ProcessAdminChangePathwayResultAsync(ChangePathwayResultRequest request);

        Task<bool> ProcessAdminChangeSpecialismResultAsync(ChangeSpecialismResultRequest request);

        Task<bool> CreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request);
    }
}