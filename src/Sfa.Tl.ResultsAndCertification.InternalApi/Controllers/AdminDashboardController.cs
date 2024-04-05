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
        [Route("ProcessAddCoreAssessmentRequest")]
        public async Task<bool> ProcessAddCoreAssessmentRequestAsync(ReviewAddCoreAssessmentRequest request)
        {
            return await _adminDashboardService.ProcessAddCoreAssessmentAsync(request);
        }

        [HttpPost]
        [Route("ProcessAddSpecialismAssessmentRequest")]
        public async Task<bool> ProcessAddSpecialismAssessmentRequestAsync(ReviewAddSpecialismAssessmentRequest request)
        {
            return await _adminDashboardService.ProcessAddSpecialismAssessmentAsync(request);
        }

        [HttpPost]
        [Route("ReviewRemoveCoreAssessmentEntry")]
        public async Task<bool> RemoveCoreAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryRequest model)
            => await _adminDashboardService.ProcessRemovePathwayAssessmentEntryAsync(model);

        [HttpPost]
        [Route("ReviewRemoveSpecialismAssessmentEntry")]
        public async Task<bool> RemoveSpecialismAssessmentEntryAsync(ReviewRemoveSpecialismAssessmentEntryRequest model)
            => await _adminDashboardService.ProcessRemoveSpecialismAssessmentEntryAsync(model);

        [HttpPost]
        [Route("ProcessAdminAddPathwayResult")]
        public Task<bool> ProcessAdminAddPathwayResultAsync(AddPathwayResultRequest request)
            => _adminDashboardService.ProcessAdminAddPathwayResultAsync(request);

        [HttpPost]
        [Route("ProcessAdminAddSpecialismResult")]
        public Task<bool> ProcessAdminAddSpecialismResultAsync(AddSpecialismResultRequest request)
            => _adminDashboardService.ProcessAdminAddSpecialismResultAsync(request);

        [HttpPost]
        [Route("ProcessAdminChangePathwayResult")]
        public Task<bool> ProcessAdminChangePathwayResultAsync(ChangePathwayResultRequest request)
          => _adminDashboardService.ProcessAdminChangePathwayResultAsync(request);

        [HttpPost]
        [Route("ProcessAdminChangeSpecialismResult")]
        public Task<bool> ProcessAdminChangeSpecialismResultAsync(ChangeSpecialismResultRequest request)
         => _adminDashboardService.ProcessAdminChangeSpecialismResultAsync(request);
    }
}