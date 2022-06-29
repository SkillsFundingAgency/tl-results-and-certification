using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IOverallResultCalculationService _overallResultCalculatoinService;

        public TestController(IOverallResultCalculationService overallResultCalculatoinService)
        {
            _overallResultCalculatoinService = overallResultCalculatoinService;
        }

        [HttpGet]
        [Route("ping")]
        public string Ping(DateTime runDate)
        {
            return "Hello World";
        }

        # region OverallResult

        [HttpGet]
        [Route("OverallResult/GetLearners/{runDate}")]
        public async Task<string> GetLearnersForOverallGradeCalculationAsync(DateTime runDate)
        {
            var result = await _overallResultCalculatoinService.GetLearnersForOverallGradeCalculationAsync(runDate);
            string json = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
            //return JsonConvert.DeserializeObject<IList<TqRegistrationPathway>>(json);
        }

        #endregion
    }
}
