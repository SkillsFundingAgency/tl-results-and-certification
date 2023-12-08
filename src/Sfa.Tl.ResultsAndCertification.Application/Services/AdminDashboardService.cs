using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ISystemProvider _systemProvider;
        private readonly IMapper _mapper;

        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository, ISystemProvider systemProvider, IMapper mapper)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _systemProvider = systemProvider;
            _mapper = mapper;
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
            var _academicYearToBe = new List<int>();

            var result = await _adminDashboardRepository.GetAdminLearnerRecordAsync(pathwayId);
            var _adminLearnerRecord = _mapper.Map<AdminLearnerRecord>(result);

            for (int i = result.AcademicYear - 1, j = 1; i >= result.TlevelStartYear && j <= 2; i--, j++)
                _academicYearToBe.Add(i);

            _adminLearnerRecord.AcademicStartYearsToBe = _academicYearToBe;

            return _adminLearnerRecord;
        }
    }

}