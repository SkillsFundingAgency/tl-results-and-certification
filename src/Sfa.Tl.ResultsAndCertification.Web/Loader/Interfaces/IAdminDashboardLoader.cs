using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminDashboardLoader
    {
        Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync();

        Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria);

        Task<TLearnerRecordViewModel> GetAdminLearnerRecordAsync<TLearnerRecordViewModel>(int registrationPathwayId);

        public Task<AdminCoreAssessmentViewModel> GetAdminLearnerRecordWithCoreAssesments(int registrationPathwayId);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel);

        Task<bool> ProcessChangeIndustryPlacementAsync(AdminReviewChangesIndustryPlacementViewModel adminChangeIpViewModel);
    }
}