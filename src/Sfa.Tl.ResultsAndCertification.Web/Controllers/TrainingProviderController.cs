using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;

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
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.EnterUniqueLearnerNumber)]
        public IActionResult EnterUniqueLearnerReference()
        {
            var viewModel = new EnterUlnViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.SubmitEnterUniqueLearnerNumber)]
        public IActionResult EnterUniqueLearnerReference(EnterUlnViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return View();
        }
    }
}
