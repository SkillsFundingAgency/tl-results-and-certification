using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class StatementOfAchievementController : Controller
    {
        ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        public StatementOfAchievementController(ResultsAndCertificationConfiguration configuration, ILogger<StatementOfAchievementController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("statements-of-achievement-not-available", Name = RouteConstants.StatementsOfAchievementNotAvailable)]
        public IActionResult StatementsOfAchievementNotAvailable()
        {
            return View(new NotAvailableViewModel { SoaAvailableDate = _configuration.SoaAvailableDate });
        }

        [HttpGet]
        [Route("request-statement-of-achievement", Name = RouteConstants.RequestStatementOfAchievement)]
        public async Task<IActionResult> RequestStatementOfAchievementAsync()
        {
            await Task.CompletedTask;
            var viewModel = new RequestStatementOfAchievementViewModel { SoaAvailableDate = _configuration.SoaAvailableDate };

            if(!viewModel.IsSoaAvailable)
            {
                return RedirectToRoute(RouteConstants.StatementsOfAchievementNotAvailable);
            }
            return View();
        }
    }
}