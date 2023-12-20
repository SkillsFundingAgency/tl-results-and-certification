using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync();
       
        Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int pathwayId);

        Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request);
    }
}