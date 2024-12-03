using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardLoader _dashboardLoader;
        private readonly ILogger _logger;

        public DashboardController(IDashboardLoader dashboardLoader, ILogger<DashboardController> logger)
        {
            _dashboardLoader = dashboardLoader;
            _logger = logger;
        }

        [HttpGet]
        [Route("home", Name = RouteConstants.Home)]
        public async Task<IActionResult> Index()
        {
            DashboardViewModel viewModel = await _dashboardLoader.GetDashboardViewModel(HttpContext.User);

            if (!viewModel.HasAccessToService)
            {
                _logger.LogWarning(LogEvent.ServiceAccessDenied, $"Service access denied, User: {User?.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ServiceAccessDenied);
            }

            return View(viewModel);
        }
    }
}
