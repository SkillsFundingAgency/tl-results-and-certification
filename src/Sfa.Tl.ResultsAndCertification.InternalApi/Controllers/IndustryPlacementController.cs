using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustryPlacementController : ControllerBase, IIndustryPlacementController
    {
        protected IIndustryPlacementService _industryPlacementService;

        public IndustryPlacementController(IIndustryPlacementService industryPlacementService)
        {
            _industryPlacementService = industryPlacementService;
        }

        [HttpGet]
        [Route("GetIpLookupData/{ipLookupType}/{pathwayId:int?}")]
        public async Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null)
        {
            return await _industryPlacementService.GetIpLookupDataAsync(ipLookupType, pathwayId);
        }
    }
}