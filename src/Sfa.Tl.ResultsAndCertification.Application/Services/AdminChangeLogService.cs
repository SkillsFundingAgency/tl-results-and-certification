using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminChangeLogService : IAdminChangeLogService
    {
        private readonly IAdminChangeLogRepository _adminChangeLogRepository;
        private readonly IMapper _mapper;

        public AdminChangeLogService(IAdminChangeLogRepository adminChangeLogRepository, IMapper mapper)
        {
            _adminChangeLogRepository = adminChangeLogRepository;
            _mapper = mapper;
        }

        public Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request)
            => _adminChangeLogRepository.SearchChangeLogsAsync(request);

        public async Task<AdminChangeLogRecord> GetChangeLogRecordAsync(int changeLogId)
        {
            var response = await _adminChangeLogRepository.GetChangeLogRecordAsync(changeLogId);
            return _mapper.Map<AdminChangeLogRecord>(response);
        }
    }
}