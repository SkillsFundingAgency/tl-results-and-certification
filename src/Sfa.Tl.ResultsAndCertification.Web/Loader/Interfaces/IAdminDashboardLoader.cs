using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminDashboardLoader
    {
        Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync();

        Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria);

        Task<LearnerRecordViewModel> GetAdminLearnerRecordAsync<LearnerRecordViewModel>(int pathwayId);

        Task<bool> ProcessChangeStartYear(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel);

    }
}