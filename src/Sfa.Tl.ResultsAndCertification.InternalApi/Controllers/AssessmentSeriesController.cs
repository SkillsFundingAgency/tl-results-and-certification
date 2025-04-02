using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
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
        private readonly IAssessmentSeriesRepository _assessmentSeriesRepository;
        private readonly IMapper _mapper;

        public AssessmentSeriesController(IAssessmentSeriesRepository assessmentSeriesRepository, IMapper mapper)
        {
            _assessmentSeriesRepository = assessmentSeriesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetResultCalculationAssessment")]
        public async Task<AssessmentSeriesDetails> GetResultCalculationAssessmentAsync()
        {
            AssessmentSeries assessmentSeries = await _assessmentSeriesRepository.GetPreviousAssessmentSeriesAsync(DateTime.Now);
            return _mapper.Map<AssessmentSeriesDetails>(assessmentSeries);
        }
    }
}
