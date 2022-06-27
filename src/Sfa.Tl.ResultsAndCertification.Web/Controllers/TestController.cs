using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class TestController : Controller
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;

        public TestController(IResultsAndCertificationInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }

        [HttpGet]
        [Route("Test/GetLearnersToCalculateResults/{runDate}", Name = "CalculateResults")]
        public async Task<IActionResult> CalculateResults(string runDate)
        {
            var result = await _internalApiClient.GetLearnersForOverallGradeCalculationAsync(runDate);
            return Json(result);
        }
    }
}
