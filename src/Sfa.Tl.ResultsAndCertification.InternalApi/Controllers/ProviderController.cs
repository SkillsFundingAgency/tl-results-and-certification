using System.Collections.Generic;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase, IProviderController
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProviderController> _logger;
        public ProviderController(IProviderService providerService, ILogger<ProviderController> logger)
        {
            _providerService = providerService;
            _logger = logger;
        }

        [Route("IsAnyProviderSetupCompleted/{ukprn}")]
        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            return await _providerService.IsAnyProviderSetupCompletedAsync(ukprn);
        }

        [Route("FindProviderNameUri/{name}/{isExactMatch}")]
        public async Task<IEnumerable<string>> FindProviderNameUriAsync(string name, bool isExactMatch)
        {
            return await _providerService.FindProviderNameUriAsync(name, isExactMatch);
        }

        public async Task<IEnumerable<object>> GetAllProvidersByAoUkprnAsync(long ukprn)
        {
            return await _providerService.GetAllProvidersByAoUkprnAsync(ukprn);
        }

        [Route("GetSelectProviderTlevels/{aoUkprn}/{providerId}")]
        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _providerService.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
        }
    }
}