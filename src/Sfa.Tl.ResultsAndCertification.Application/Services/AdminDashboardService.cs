using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly ISystemProvider _systemProvider;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;

        public AdminDashboardService(
            IAdminDashboardRepository adminDashboardRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            ISystemProvider systemProvider,
            ICommonService commonService,
            IMapper mapper)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _systemProvider = systemProvider;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _commonService = commonService;
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

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId)
        {
            var tqRegistrationPathway = await _adminDashboardRepository.GetLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<AdminLearnerRecord>(tqRegistrationPathway);
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request)
        {
            var pathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);

            if (pathway == null) return false;

            pathway.AcademicYear = request.AcademicYearTo;
            var status = await _tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(pathway, u => u.AcademicYear, u => u.ModifiedBy, u => u.ModifiedOn);

            if (status > 0)
                return await _commonService.AddChangelog(CreateChangeLogRequest(request));
            return false;
        }

        private static ChangeLog CreateChangeLogRequest(ReviewChangeStartYearRequest request)
        {
            var changeLog = new ChangeLog
            {
                ChangeType = (int)ChangeType.StartYear,
                ReasonForChange = request.ChangeReason,
                DateOfRequest = Convert.ToDateTime(request.RequestDate),
                Details = JsonConvert.SerializeObject(request.ChangeStartYearDetails),
                ZendeskTicketID = request.ZendeskId,
                Name = request.ContactName,
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                CreatedBy = string.IsNullOrEmpty(request.CreatedBy) ? "System" : request.CreatedBy
            };

            return changeLog;
        }
    }
}