using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardingOrganisationController : ControllerBase, IAwardingOrganisationController
    {
        private readonly IAwardingOrganisationService _awardingOrganisationService;

        public AwardingOrganisationController(IAwardingOrganisationService awardingOrganisationService)
        {
            _awardingOrganisationService = awardingOrganisationService;
        }

        [HttpGet]
        [Route("GetAllTlevels/{id}")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync(int id)
        {
            return await _awardingOrganisationService.GetAllTlevelsByAwardingOrganisationIdAsync(id);
        }
    }
}