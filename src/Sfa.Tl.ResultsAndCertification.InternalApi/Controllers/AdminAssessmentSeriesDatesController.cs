using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAssessmentSeriesDatesController : ControllerBase
    {
        private readonly IAdminAssessmentSeriesDatesService _adminAssessmentSeriesDatesService;
        public AdminAssessmentSeriesDatesController(IAdminAssessmentSeriesDatesService adminAssessmentSeriesDatesService)
        {
            _adminAssessmentSeriesDatesService = adminAssessmentSeriesDatesService;
        }

        [HttpGet]
        [Route("GetAssessmentSeriesDate/{assessmentId}")]
        public async Task<GetAssessmentSeriesDatesDetailsResponse> GetAssessmentSeriesDatesAsync(int assessmentId)
            => await _adminAssessmentSeriesDatesService.GetAssessmentSeriesDateAsync(assessmentId);

        [HttpPost]
        [Route("SearchAssessmentSeriesDates")]
        public async Task<IEnumerable<GetAssessmentSeriesDatesDetailsResponse>> SearchAssessmentSeriesDatesAsync(SearchAssessmentSeriesDatesRequest request)
            => await _adminAssessmentSeriesDatesService.SearchAssessmentSeriesDatesAsync(request);
    }
}
