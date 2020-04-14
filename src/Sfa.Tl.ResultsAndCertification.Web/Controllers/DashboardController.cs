using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;

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
        [Route("dashboard", Name = RouteConstants.Dashboard)]
        public IActionResult Index()
        {
            if (!HttpContext.User.HasAccessToService())
            {
                _logger.LogWarning(LogEvent.ServiceAccessDenied, $"Service access denied, User: {User?.GetUserEmail()}");
                return (IActionResult)RedirectToRoute(RouteConstants.ServiceAccessDenied);
            }

            return View();
        }
    }
}
