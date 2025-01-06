using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardBannerController
    {
        private readonly IDashboardBannerService _dashboardBannerService;
        private readonly Func<DateTime> _getToday = () => DateTime.Today;

        public DashboardBannerController(IDashboardBannerService dashboardBannerService)
        {
            _dashboardBannerService = dashboardBannerService;
        }

        [HttpGet]
        [Route("GetAwardingOrganisationBanners")]
        public Task<IEnumerable<string>> GetAwardingOrganisationBanners()
           => _dashboardBannerService.GetAwardingOrganisationBanners(_getToday);

        [HttpGet]
        [Route("GetProviderBanners")]
        public Task<IEnumerable<string>> GetProviderBanners()
            => _dashboardBannerService.GetProviderBanners(_getToday);
    }
}