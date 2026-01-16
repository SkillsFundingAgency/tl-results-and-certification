using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IAdminDashboardController
    {
        Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync();
        Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId);
        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request);

        Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request);
        
        Task<bool> ProcessAddCoreAssessmentRequestAsync(ReviewAddCoreAssessmentRequest request);
        Task<bool> ProcessAddSpecialismAssessmentRequestAsync(ReviewAddSpecialismAssessmentRequest request);

        Task<bool> RemoveCoreAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryRequest request);
        Task<bool> RemoveSpecialismAssessmentEntryAsync(ReviewRemoveSpecialismAssessmentEntryRequest request);

        Task<bool> ProcessAdminCreateReplacementDocumentPrintingRequestAsync(ReplacementPrintRequest request);
    }
}