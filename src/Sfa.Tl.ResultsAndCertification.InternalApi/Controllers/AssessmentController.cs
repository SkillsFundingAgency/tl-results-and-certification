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
        private readonly IBulkProcessLoader _bulkAssementProcess;

        public AssessmentController(IBulkProcessLoader bulkAssementProcess)
        {
            _bulkAssementProcess = bulkAssementProcess;
        }

        [HttpPost]
        [Route("ProcessBulkAssessments")]
        public async Task<BulkRegistrationResponse> ProcessBulkAssessmentsAsync(BulkRegistrationRequest request)
        {
            return await _bulkAssementProcess.StartBulkProcessAsync(request);
        }
    }
}