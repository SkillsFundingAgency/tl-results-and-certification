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

        public AwardingOrganisationController(IAwardingOrganisationService awardingOrganisationService)
        {
            _awardingOrganisationService = awardingOrganisationService;
        }

        [HttpGet]
        [Route("GetAllTlevels")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync()
        {
            // TODO: following statement to be updated?
            var id = !string.IsNullOrEmpty(User.GetUkPrn()) ? long.Parse(User.GetUkPrn()) : 10011881;
            return await _awardingOrganisationService.GetAllTlevelsByAwardingOrganisationIdAsync(id);
        }
    }
}