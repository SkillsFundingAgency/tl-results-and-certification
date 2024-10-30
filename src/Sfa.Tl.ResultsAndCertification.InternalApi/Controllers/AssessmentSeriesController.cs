using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentSeriesController : ControllerBase, IAssessmentSeriesController
    {
        private readonly IOverallResultCalculationService _overallResultCalculationService;
        private readonly IMapper _mapper;

        public AssessmentSeriesController(IOverallResultCalculationService overallResultCalculationService, IMapper mapper)
        {
            _overallResultCalculationService = overallResultCalculationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetResultCalculationAssessment")]
        public async Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync()
        {
            var assessmentSeries = await _overallResultCalculationService.GetResultCalculationAssessmentAsync(DateTime.Now);
            return _mapper.Map<AssessmentSeriesDetails>(assessmentSeries);
        }
    }
}
