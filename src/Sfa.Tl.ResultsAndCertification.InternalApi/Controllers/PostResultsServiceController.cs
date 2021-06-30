using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostResultsServiceController : ControllerBase, IPostResultsServiceController
    {
        protected IPostResultsServiceService _postResultsServiceService;

        public PostResultsServiceController(IPostResultsServiceService postResultsServiceService)
        {
            _postResultsServiceService = postResultsServiceService;
        }

        [HttpGet]
        [Route("FindPrsLearnerRecord/{aoUkprn}/{uln}")]
        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _postResultsServiceService.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        [HttpGet]
        [Route("GetPrsLearnerDetails/{aoUkprn}/{profileId}/{assessmentId}")]
        public async Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkPrn, int profileId, int assessmentId)
        {
            return await _postResultsServiceService.GetPrsLearnerDetailsAsync(aoUkPrn, profileId, assessmentId);
        }

        [HttpPost]
        [Route("AppealGrade")]
        public async Task<bool> AppealGradeAsync(AppealGradeRequest request)
        {
            return await _postResultsServiceService.AppealGradeAsync(request);
        }
    }
}
