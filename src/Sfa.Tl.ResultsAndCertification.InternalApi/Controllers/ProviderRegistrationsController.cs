using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderRegistrations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderRegistrationsController : ControllerBase
    {
        private readonly IProviderRegistrationsService _providerRegistrationsService;

        public ProviderRegistrationsController(IProviderRegistrationsService providerRegistrationsService)
        {
            _providerRegistrationsService = providerRegistrationsService;
        }

        [HttpGet]
        [Route("GetAvailableStartYears")]
        public Task<IList<int>> GetAvailableStartYearsAsync()
            => _providerRegistrationsService.GetAvailableStartYearsAsync(() => DateTime.UtcNow.Date);

        [HttpPost]
        [Route("GetRegistrations")]
        public Task<DataExportResponse> GetRegistrationsAsync(GetProviderRegistrationsRequest request)
            => _providerRegistrationsService.GetRegistrationsAsync(request.ProviderUkprn, request.StartYear, request.RequestedBy, () => Guid.NewGuid());
    }
}