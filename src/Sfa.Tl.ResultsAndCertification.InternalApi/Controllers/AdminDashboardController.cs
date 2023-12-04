using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase, IAdminDashboardController
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet]
        [Route("GetAdminSearchLearnerFilters")]
        public Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync()
        {
            return _adminDashboardService.GetAdminSearchLearnerFiltersAsync();
        }

        [HttpGet]
        [Route("GetAdminLearnerRecord/{profileId}")]
        public Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int profileId)
        {
            return _adminDashboardService.GetAdminLearnerRecordAsync(profileId);
        }

    }
}