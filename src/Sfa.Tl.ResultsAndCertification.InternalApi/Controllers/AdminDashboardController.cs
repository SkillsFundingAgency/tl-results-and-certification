﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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

        [HttpGet]
        [Route("GetLearnerRecord/{profileid}", Name = RouteConstants.LearnerRecord)]
        public async Task<AdminLearnerRecord> GetLearnerRecordAsync(int profileId)
        {
            return await _adminDashboardService.GetLearnerRecordAsync(profileId);
        }



        [HttpPost]
        [Route("GetAdminSearchLearnerDetails")]
        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            return _adminDashboardService.GetAdminSearchLearnerDetailsAsync(request);
        }
    }
}