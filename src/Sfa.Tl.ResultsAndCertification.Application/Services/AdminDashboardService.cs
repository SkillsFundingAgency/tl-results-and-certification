using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
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

        public async Task<SearchLearnerFilters> GetFiltersAsync()
        {
            return new SearchLearnerFilters
            {
                AwardingOrganisations = await _adminDashboardRepository.GetAwardingOrganisationFiltersAsync(),
                AcademicYears = await _adminDashboardRepository.GetAcademicYearFiltersAsync(_systemProvider.UtcToday)
            };
        }
    }
}