using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger _logger;
        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("home", Name = RouteConstants.Home)]
        public IActionResult Index()
        {
            var loggedInUserType = HttpContext.User.GetLoggedInUserType();

            if (!HttpContext.User.HasAccessToService() || !loggedInUserType.HasValue)
            {
                _logger.LogWarning(LogEvent.ServiceAccessDenied, $"Service access denied, User: {User?.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ServiceAccessDenied);
            }

            var viewModel = new DashboardViewModel { IsAoUser = loggedInUserType == LoginUserType.AwardingOrganisation, IsTrainingProviderUser = loggedInUserType == LoginUserType.TrainingProvider };
            return View(viewModel);
        }
    }
}
