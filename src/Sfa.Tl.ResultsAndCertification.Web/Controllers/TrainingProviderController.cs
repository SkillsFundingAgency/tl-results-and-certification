using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class TrainingProviderController : Controller
    {
        private readonly ITrainingProviderLoader _trainingProviderLoader;

        public TrainingProviderController(ITrainingProviderLoader trainingProviderLoader)
        {
            _trainingProviderLoader = trainingProviderLoader;
        }

        [HttpGet]
        [Route("manage-learner-records", Name = RouteConstants.ManageLearnerRecordsDashboard)]
        public IActionResult Index()
        {
            return View(new DashboardViewModel());
        }

        [HttpGet]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.EnterUln)]
        public IActionResult EnterUniqueLearnerReference()
        {
            return View();
        }
    }
}
