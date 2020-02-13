using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AwardingOrganisationController : ControllerBase, IAwardingOrganisationController
    {
        private readonly IAwardingOrganisationService _awardingOrganisationService;
        private readonly IPathwayService _pathwayService;

        public AwardingOrganisationController(IAwardingOrganisationService awardingOrganisationService,
            IPathwayService pathwayService)
        {
            _awardingOrganisationService = awardingOrganisationService;
            _pathwayService = pathwayService;
        }

        [HttpGet]
        [Route("GetAllTlevels")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync()
        {
            // TODO: following statement to be updated?
            var id = !string.IsNullOrEmpty(User.GetUkPrn()) ? long.Parse(User.GetUkPrn()) : 10011881;
            return await _awardingOrganisationService.GetAllTlevelsByAwardingOrganisationIdAsync(id);
        }

        [HttpGet]
        [Route("Tlevel/{id}")]
        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(int id)
        {
            // TODO: Security validation cross-cutting functionality?
            return await _pathwayService.GetTlevelDetailsByPathwayIdAsync(id);
        }
    }
}