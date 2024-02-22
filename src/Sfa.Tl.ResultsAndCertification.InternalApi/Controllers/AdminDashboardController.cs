using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
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

        [HttpPost]
        [Route("GetAdminSearchLearnerDetails")]
        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            return _adminDashboardService.GetAdminSearchLearnerDetailsAsync(request);
        }

        [HttpGet]
        [Route("GetAdminLearnerRecord/{registrationPathwayId}")]
        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId)
        {
            return await _adminDashboardService.GetAdminLearnerRecordAsync(registrationPathwayId);
        }

        [HttpPost]
        [Route("ProcessChangeStartYear")]
        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request)
        {
            return await _adminDashboardService.ProcessChangeStartYearAsync(request);
        }


        [HttpPost]
        [Route("ProcessChangeIndustryPlacement")]
        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            return await _adminDashboardService.ProcessChangeIndustryPlacementAsync(request);
        }

        [HttpPost]
        [Route("ReviewRemoveAssessmentEntry")]
        public async Task<bool> RemoveAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest model)
        {
            return model.ComponentType switch
            {
                ComponentType.Core => await _adminDashboardService.RemovePathwayAssessmentEntryAsync(model),
                ComponentType.Specialism => await _adminDashboardService.RemoveSpecialismAssessmentEntryAsync(model),
                ComponentType.NotSpecified => false,
                _ => false
            };
        }
    }

}