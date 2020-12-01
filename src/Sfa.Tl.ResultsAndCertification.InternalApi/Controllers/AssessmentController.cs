using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Assessment;
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
        [Route("AddAssessmentSeries")]
        public async Task<AddAssessmentSeriesResponse> AddAssessmentSeriesAsync(AddAssessmentSeriesRequest request)
        {
            return await _assessmentService.AddAssessmentSeriesAsync(request);
        }

        [HttpGet]
        [Route("GetActiveAssessmentEntryDetails/{aoUkprn}/{profileId}/{assessmentEntryType}")]
        public async Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int pathwayAssessmentId, AssessmentEntryType assessmentEntryType)
        {
            return assessmentEntryType switch
            {
                AssessmentEntryType.Core => await _assessmentService.GetActivePathwayAssessmentEntryDetailsAsync(aoUkprn, pathwayAssessmentId),
                AssessmentEntryType.Specialism => null,
                AssessmentEntryType.NotSpecified => null,
                _ => null
            };
        }        
    }
}