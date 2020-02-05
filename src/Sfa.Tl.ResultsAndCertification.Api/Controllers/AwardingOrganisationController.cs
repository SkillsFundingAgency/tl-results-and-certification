using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;

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
        [Route("GetAllTlevels/{id}")]
        public async Task<IEnumerable<string>> GetAllTlevelsByAwardingOrganisationIdAsync(int id)
        {
            var result = await _awardingOrganisationService.GetAllTlevelsByAwardingOrganisationIdAsync(id);

            return result;
        }
    }
}