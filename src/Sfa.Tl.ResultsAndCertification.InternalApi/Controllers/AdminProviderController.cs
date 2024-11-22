using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminProviderController : ControllerBase
    {
        private readonly IAdminProviderService _adminProviderService;

        public AdminProviderController(IAdminProviderService adminProviderService)
        {
            _adminProviderService = adminProviderService;
        }

        [HttpGet]
        [Route("GetProvider/{providerId}")]
        public Task<GetProviderResponse> GetProviderAsync(int providerId)
            => _adminProviderService.GetProviderAsync(providerId);

        [HttpPut]
        [Route("UpdateProvider")]
        public Task<UpdateProviderResponse> UpdateProviderAsync(UpdateProviderRequest request)
            => _adminProviderService.UpdateProviderAsync(request, () => DateTime.UtcNow);

        [HttpPost]
        [Route("AddProvider")]
        public Task<int> AddProviderAsync(AddProviderRequest request)
            => _adminProviderService.AddProviderAsync(request);
    }
}