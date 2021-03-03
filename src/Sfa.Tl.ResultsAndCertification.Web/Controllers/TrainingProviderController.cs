using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TrainingProviderController : Controller
    {
        private readonly ITrainingProviderLoader _trainingProviderLoader;

        public TrainingProviderController(ITrainingProviderLoader trainingProviderLoader)
        {
            _trainingProviderLoader = trainingProviderLoader;
        }

        [HttpGet]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.EnterUln)]
        public IActionResult EnterUniqueLearnerReference()
        {
            return View();
        }
    }
}
