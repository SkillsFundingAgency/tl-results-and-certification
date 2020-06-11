using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationDataValidator _csvDataValidator;
        private readonly ILogger<ProviderController> _logger;

        public RegistrationController(IRegistrationDataValidator csvDataValidator, ILogger<ProviderController> logger)
        {
            _logger = logger;
            _csvDataValidator = csvDataValidator;
        }

        [HttpPost]
        [Route("bulk-upload")]
        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            var response = new BulkRegistrationResponse();

            // Step: Read file from the blob.

            // Step: CsvService 
            //await _csvDataValidator.ValidateAsync(request)

            // Step: Update database.

            return response;
        }
    }
}