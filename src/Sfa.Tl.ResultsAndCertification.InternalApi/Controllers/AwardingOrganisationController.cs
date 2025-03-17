using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardingOrganisationController : ControllerBase
    {
        private readonly IAwardingOrganisationService _awardingOrganisationService;

        public AwardingOrganisationController(IAwardingOrganisationService awardingOrganisationService)
        {
            _awardingOrganisationService = awardingOrganisationService;
        }

        [HttpGet]
        [Route("GetAllAwardingOrganisations")]
        public Task<IEnumerable<AwardingOrganisationMetadata>> GetAllAwardingOrganisationsAsync()
            => _awardingOrganisationService.GetAllAwardingOrganisationsAsync();

        [HttpGet]
        [Route("GetAwardingOrganisationByUkprn/{ukprn}")]
        public Task<AwardingOrganisationMetadata> GetAwardingOrganisationByUkprnAsync(long ukprn)
            => _awardingOrganisationService.GetAwardingOrganisationByUkprnAsync(ukprn);
    }
}