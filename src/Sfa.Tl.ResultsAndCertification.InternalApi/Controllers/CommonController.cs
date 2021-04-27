using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase, ICommonController
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        [Route("GetLookupData/{lookupCategory}")]
        public async Task<IEnumerable<LookupData>> GetLookupDataAsync(LookupCategory lookupCategory)
        {
            return await _commonService.GetLookupDataAsync(lookupCategory);
        }

        [HttpGet]
        [Route("GetLoggedInUserTypeInfo/{ukprn}")]
        public async Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn)
        {
            return await _commonService.GetLoggedInUserTypeInfoAsync(ukprn);
        }
    }
}
