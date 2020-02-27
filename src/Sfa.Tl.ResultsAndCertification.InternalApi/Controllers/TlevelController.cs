using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TlevelController : ControllerBase, ITlevelController
    {
        private readonly IAwardingOrganisationService _awardingOrganisationService;
        private readonly IPathwayService _pathwayService;

        public TlevelController(IAwardingOrganisationService awardingOrganisationService,
            IPathwayService pathwayService)
        {
            _awardingOrganisationService = awardingOrganisationService;
            _pathwayService = pathwayService;
        }

        [HttpGet]
        [Route("GetAllTlevels/{ukprn}")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await _awardingOrganisationService.GetAllTlevelsByUkprnAsync(ukprn);
        }

        [HttpGet]
        [Route("{ukprn}/GetTlevelsByStatus/{statusId}")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            return await _awardingOrganisationService.GetTlevelsByStatusIdAsync(ukprn, statusId);
        }

        [HttpGet]
        [Route("{ukprn}/TlevelDetails/{id}")]
        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int id)
        {
            var tlevelDetails = await _pathwayService.GetTlevelDetailsByPathwayIdAsync(ukprn, id);
            return tlevelDetails;
        }

        [HttpPut]
        [Route("ConfirmTlevel/{tqAoId}/{reviewStatusId}")]
        public async Task<IActionResult> ConfirmTlevel(int tqAoId, int reviewStatusId)
        {
            var result = await _awardingOrganisationService.ConfirmTlevelAsync(tqAoId, reviewStatusId);
            return Ok(result);
        }
    }
}