using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminDashboardLoader
    {
        Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync();

        Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria);

        Task<LearnerRecordViewModel> GetAdminLearnerRecordAsync<LearnerRecordViewModel>(int pathwayId);

        Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel);

        Task<bool> ProcessChangeIndustryPlacementAsync(AdminReviewChangesIndustryPlacementViewModel adminChangeIpViewModel);
    }
        
}