using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet]
        [Route("IsAnyProviderSetupCompleted/{ukprn}")]
        public async Task<bool> IsAnyProviderSetupCompletedAsync(long ukprn)
        {
            return await _providerService.IsAnyProviderSetupCompletedAsync(ukprn);
        }

        [HttpGet]
        [Route("FindProvider/{name}/{isExactMatch}")]
        public async Task<IEnumerable<ProviderMetadata>> FindProviderAsync(string name, bool isExactMatch)
        {
            return await _providerService.FindProviderAsync(name, isExactMatch);
        }

        [HttpGet]
        [Route("GetSelectProviderTlevels/{aoUkprn}/{providerId}")]
        public async Task<ProviderTlevels> GetSelectProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _providerService.GetSelectProviderTlevelsAsync(aoUkprn, providerId);
        }

        [HttpPost]
        [Route("AddProviderTlevels")]
        public async Task<IActionResult> AddProviderTlevelsAsync(IList<ProviderTlevelDetails> model)
        {
            var result = await _providerService.AddProviderTlevelsAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllProviderTlevels/{aoUkprn}/{providerId}")]
        public async Task<ProviderTlevels> GetAllProviderTlevelsAsync(long aoUkprn, int providerId)
        {
            return await _providerService.GetAllProviderTlevelsAsync(aoUkprn, providerId);
        }


        [HttpGet]
        [Route("GetTqAoProviderDetails/{aoUkprn}")]
        public async Task<IList<ProviderDetails>> GetTqAoProviderDetailsAsync(long aoUkprn)
        {
            return await _providerService.GetTqAoProviderDetailsAsync(aoUkprn);
        }
    }
}