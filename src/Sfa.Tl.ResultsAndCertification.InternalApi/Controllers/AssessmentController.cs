using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IBulkAssessmentLoader _bulkAssementProcess;

        public AssessmentController(IBulkAssessmentLoader bulkAssementProcess)
        {
            _bulkAssementProcess = bulkAssementProcess;
        }

        [HttpPost]
        [Route("ProcessBulkAssessments")]
        public async Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest request)
        {
            return await _bulkAssementProcess.ProcessAsync(request);
        }
    }
}