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
            return await _bulkAssementProcess.ProcessAsync(request);
        }

        [HttpGet]
        [Route("ProcessBulkAssessments")]
        public async Task<BulkRegistrationResponse> ProcessBulkAssessmentsAsync()
        {
            // Test method.
            var request = new BulkRegistrationRequest
            {
                AoUkprn = 10009696,
                BlobFileName = "132369659480218237.csv",
                PerformedBy = "Ravi",
                DocumentType = Common.Enum.DocumentType.Registrations
            };
            return await _bulkAssementProcess.ProcessAsync(request);
        }
    }
}