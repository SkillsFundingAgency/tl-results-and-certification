using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase, IResultController
    {
        private readonly IBulkResultLoader _bulkResultProcess;
        protected IResultService _resultService;
        
        public ResultController(IBulkResultLoader bulkResultProcess, IResultService resultService)
        {
            _bulkResultProcess = bulkResultProcess;
            _resultService = resultService;
        }

        [HttpPost]
        [Route("ProcessBulkResults")]
        public async Task<BulkResultResponse> ProcessBulkResultsAsync(BulkProcessRequest request)
        {
            return await _bulkResultProcess.ProcessAsync(request);
        }

        [HttpGet]
        [Route("GetResultDetails/{aoUkprn}/{profileId}/{status:int?}")]
        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            return await _resultService.GetResultDetailsAsync(aoUkprn, profileId, status);
        }

        [HttpPost]
        [Route("AddResult")]
        public async Task<AddResultResponse> AddResultAsync(AddResultRequest request)
        {
            return await _resultService.AddResultAsync(request);
        }
    }
}