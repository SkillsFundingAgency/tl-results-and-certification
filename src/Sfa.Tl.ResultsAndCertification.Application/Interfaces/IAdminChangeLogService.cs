using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminChangeLogService
    {
        Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request);
    }
}