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

        public AdminChangeLogService(IAdminChangeLogRepository adminChangeLogRepository)
        {
            _adminChangeLogRepository = adminChangeLogRepository;
        }

        public Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request)
            => _adminChangeLogRepository.SearchChangeLogsAsync(request);
    }
}