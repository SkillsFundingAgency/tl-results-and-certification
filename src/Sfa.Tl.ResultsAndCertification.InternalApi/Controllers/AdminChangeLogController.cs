using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminChangeLogController : ControllerBase, IAdminChangeLogController
    {
        private readonly IAdminChangeLogService _adminChangeLogService;

        public AdminChangeLogController(IAdminChangeLogService adminChangeLogService)
        {
            _adminChangeLogService = adminChangeLogService;
        }

        [HttpPost]
        [Route("SearchChangeLogs")]
        public Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request)
            => _adminChangeLogService.SearchChangeLogsAsync(request);
    }
}