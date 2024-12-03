using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBannerController : ControllerBase
    {
        private readonly IAdminBannerService _adminBannerService;

        public AdminBannerController(IAdminBannerService adminBannerService)
        {
            _adminBannerService = adminBannerService;
        }

        [HttpPost]
        [Route("SearchBanners")]
        public Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request)
            => _adminBannerService.SearchBannersAsync(request);

        [HttpGet]
        [Route("GetBanner/{bannerId}")]
        public Task<GetBannerResponse> GetBannerAsync(int bannerId)
             => _adminBannerService.GetBannerAsync(bannerId);

        [HttpPost]
        [Route("AddBanner")]
        public Task<AddBannerResponse> AddBannerAsync(AddBannerRequest request)
            => _adminBannerService.AddBannerAsync(request);

        [HttpPut]
        [Route("UpdateBanner")]
        public Task<bool> UpdateBannerAsync(UpdateBannerRequest request)
            => _adminBannerService.UpdateBannerAsync(request, () => DateTime.UtcNow);
    }
}