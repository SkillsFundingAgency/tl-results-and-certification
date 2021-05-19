using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class StatementOfAchievementController : Controller
    {
        private readonly IProviderAddressLoader _providerAddress;
        ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        public StatementOfAchievementController(IProviderAddressLoader providerAddress, ResultsAndCertificationConfiguration configuration, ILogger<StatementOfAchievementController> logger)
        {
            _providerAddress = providerAddress;
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
            var viewModel = new RequestStatementOfAchievementViewModel { SoaAvailableDate = _configuration.SoaAvailableDate };

            if (!viewModel.IsSoaAvailable)
            {
                return RedirectToRoute(RouteConstants.StatementsOfAchievementNotAvailable);
            }
            else
            {
                var address = await _providerAddress.GetAddressAsync<Address>(User.GetUkPrn());
                if (address == null)
                    return RedirectToRoute(RouteConstants.PostalAddressMissing);
            }
            return View();
        }

        [HttpGet]
        [Route("postal-address-missing", Name = RouteConstants.PostalAddressMissing)]
        public IActionResult PostalAddressMissingAsync()
        {
            var viewModel = new PostalAddressMissingViewModel();
            return View(viewModel);
        }
    }
}