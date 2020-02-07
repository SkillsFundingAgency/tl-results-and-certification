using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Services.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
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
        [Route("GetAllTlevels")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByAwardingOrganisationIdAsync()
        {
            // TODO: following statement to be updated?
            var id = !string.IsNullOrEmpty(User.GetUkPrn()) ? int.Parse(User.GetUkPrn()) : 10009696;
           
            return await _awardingOrganisationService.GetAllTlevelsByAwardingOrganisationIdAsync(id);
        }
    }
}