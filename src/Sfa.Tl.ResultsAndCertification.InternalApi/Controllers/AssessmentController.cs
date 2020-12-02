using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase, IAssessmentController
    {
        private readonly IBulkAssessmentLoader _bulkAssementProcess;
        protected IAssessmentService _assessmentService;

        public AssessmentController(IBulkAssessmentLoader bulkAssementProcess, IAssessmentService assessmentService)
        {
            _bulkAssementProcess = bulkAssementProcess;
            _assessmentService = assessmentService;
        }

        [HttpPost]
        [Route("ProcessBulkAssessments")]
        public async Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest request)
        {
            return await _bulkAssementProcess.ProcessAsync(request);
        }

        [HttpGet]
        [Route("GetAssessmentDetails/{aoUkprn}/{profileId}/{status:int?}")]
        public async Task<AssessmentDetails> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            return await _assessmentService.GetAssessmentDetailsAsync(aoUkprn, profileId, status);
        }

        [HttpGet]
        [Route("GetAvailableAssessmentSeries/{aoUkprn}/{profileId}/{assessmentEntryType}")]
        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, AssessmentEntryType assessmentEntryType)
        {
            return await _assessmentService.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, assessmentEntryType);
        }

        [HttpPost]
        [Route("AddAssessmentEntry")]
        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            return await _assessmentService.AddAssessmentEntryAsync(request);
        }

        [HttpGet]
        [Route("GetActiveAssessmentEntryDetails/{aoUkprn}/{assessmentId}/{assessmentEntryType}")]
        public async Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, AssessmentEntryType assessmentEntryType)
        {
            return assessmentEntryType switch
            {
                AssessmentEntryType.Core => await _assessmentService.GetActivePathwayAssessmentEntryDetailsAsync(aoUkprn, assessmentId),
                AssessmentEntryType.Specialism => null,
                AssessmentEntryType.NotSpecified => null,
                _ => null
            };
        }        
    }
}