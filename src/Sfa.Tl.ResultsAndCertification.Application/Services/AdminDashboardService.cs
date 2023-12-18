using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ISystemProvider _systemProvider;

        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository, ISystemProvider systemProvider)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _systemProvider = systemProvider;
        }

        public async Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync()
        {
            return new AdminSearchLearnerFilters
            {
                AwardingOrganisations = await _adminDashboardRepository.GetAwardingOrganisationFiltersAsync(),
                AcademicYears = await _adminDashboardRepository.GetAcademicYearFiltersAsync(_systemProvider.UtcToday)
            };
        }

        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            return _adminDashboardRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int pathwayId)
        {
            return await _adminDashboardRepository.GetAdminLearnerRecordAsync(pathwayId);
        }
    }
}