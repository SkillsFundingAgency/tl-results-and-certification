using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminDashboardLoader
    {
        Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync();

        Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria);

        Task<TLearnerRecordViewModel> GetAdminLearnerRecordAsync<TLearnerRecordViewModel>(int registrationPathwayId);

        public Task<AdminCoreComponentViewModel> GetAdminLearnerRecordWithCoreComponents(int registrationPathwayId);

        public Task<AdminOccupationalSpecialismViewModel> GetAdminLearnerRecordWithOccupationalSpecialism(int registrationPathwayId, int specialismsId);

        Task<AdminChangeStartYearViewModel> GetAdminLearnerRecordChangeYearAsync(int registrationPathwayId);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel);

        Task<bool> ProcessChangeIndustryPlacementAsync(AdminReviewChangesIndustryPlacementViewModel adminChangeIpViewModel);

        Task<AdminRemovePathwayAssessmentEntryViewModel> GetRemovePathwayAssessmentEntryAsync(int registrationPathwayId, int pathwayAssessmentId);

        Task<AdminRemoveSpecialismAssessmentEntryViewModel> GetRemoveSpecialismAssessmentEntryAsync(int registrationPathwayId, int specialismAssessmentId);

        Task<bool> ProcessAddCoreAssessmentRequestAsync(AdminReviewChangesCoreAssessmentViewModel adminReviewChangesCoreAssessmentViewModel);

        Task<bool> ProcessAddSpecialismAssessmentRequestAsync(AdminReviewChangesSpecialismAssessmentViewModel adminReviewChangesSpecialismAssessmentViewModel);

        Task<bool> ProcessRemoveAssessmentEntry(AdminReviewRemoveCoreAssessmentEntryViewModel model);

        Task<bool> ProcessRemoveSpecialismAssessmentEntryAsync(AdminReviewRemoveSpecialismAssessmentEntryViewModel model);

        Task<AdminAddPathwayResultViewModel> GetAdminAddPathwayResultAsync(int registrationPathwayId, int assessmentId);

        Task<AdminAddSpecialismResultViewModel> GetAdminAddSpecialismResultAsync(int registrationPathwayId, int assessmentId);

        Task LoadAdminAddPathwayResultGrades(AdminAddPathwayResultViewModel model);

        Task LoadAdminAddSpecialismResultGrades(AdminAddSpecialismResultViewModel model);

        AdminAddPathwayResultReviewChangesViewModel CreateAdminAddPathwayResultReviewChanges(AdminAddPathwayResultViewModel model);

        Task<bool> ProcessAddPathwayResultReviewChangesAsync(AdminAddPathwayResultReviewChangesViewModel model);

        AdminAddSpecialismResultReviewChangesViewModel CreateAdminAddSpecialismResultReviewChanges(AdminAddSpecialismResultViewModel model);

        Task<bool> ProcessAddSpecialismResultReviewChangesAsync(AdminAddSpecialismResultReviewChangesViewModel model);

        Task<AdminChangePathwayResultViewModel> GetAdminChangePathwayResultAsync(int registrationPathwayId, int assessmentId);

        Task LoadAdminChangePathwayResultGrades(AdminChangePathwayResultViewModel model);

        Task<AdminChangeSpecialismResultViewModel> GetAdminChangeSpecialismResultAsync(int registrationPathwayId, int assessmentId);

        Task LoadAdminChangeSpecialismResultGrades(AdminChangeSpecialismResultViewModel model);

        AdminChangePathwayResultReviewChangesViewModel CreateAdminChangePathwayResultReviewChanges(AdminChangePathwayResultViewModel model);

        Task<bool> ProcessChangePathwayResultReviewChangesAsync(AdminChangePathwayResultReviewChangesViewModel model);

        AdminChangeSpecialismResultReviewChangesViewModel CreateAdminChangeSpecialismResultReviewChanges(AdminChangeSpecialismResultViewModel model);
        Task<bool> ProcessChangeSpecialismResultReviewChangesAsync(AdminChangeSpecialismResultReviewChangesViewModel model);
       
    }
}