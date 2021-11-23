using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerController : ControllerBase, ILearnerController
    {
        protected ILearnerService _learnerService;

        public LearnerController(ILearnerService learnerService)
        {
            _learnerService = learnerService;
        }

        [HttpGet]
        [Route("GetLearnerRecord/{aoUkprn}/{profileId}/{status:int?}")]
        public async Task<LearnerRecord> GetLearnerRecordAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            return await _learnerService.GetLearnerRecordAsync(aoUkprn, profileId, status);
        }
    }
}
