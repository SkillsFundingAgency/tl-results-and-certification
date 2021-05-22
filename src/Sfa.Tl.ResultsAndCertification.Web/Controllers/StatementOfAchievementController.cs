using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
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
            if (!IsSoaAvailable())
                return RedirectToRoute(RouteConstants.StatementsOfAchievementNotAvailable);

            if (!await IsAddressAvailable())
                return RedirectToRoute(RouteConstants.PostalAddressMissing);

            return View(new RequestStatementOfAchievementViewModel());
        }

        [HttpGet]
        [Route("postal-address-missing", Name = RouteConstants.PostalAddressMissing)]
        public IActionResult PostalAddressMissing()
        {
            var viewModel = new PostalAddressMissingViewModel();
            return View(viewModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-unique-learner-number", Name = RouteConstants.RequestSoaUniqueLearnerNumber)]
        public async Task<IActionResult> RequestSoaUniqueLearnerNumberAsync()
        {
            if (!IsSoaAvailable() || !await IsAddressAvailable())
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new RequestSoaUniqueLearnerNumberViewModel();
            return View(viewModel);
        }
        
        [HttpPost]
        [Route("request-statement-of-achievement-unique-learner-number", Name = RouteConstants.SubmitRequestSoaUniqueLearnerNumber)]
        public async Task<IActionResult> RequestSoaUniqueLearnerNumberAsync(RequestSoaUniqueLearnerNumberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await Task.CompletedTask;
            return RedirectToRoute(RouteConstants.RequestSoaUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-ULN-not-registered", Name = RouteConstants.RequestSoaUlnNotFound)]
        public async Task<IActionResult> RequestSoaUlnNotFoundAsync()
        {
            //var cacheModel = await _cacheService.GetAsync<RequestSoaUniqueLearnerNumberViewModel>(CacheKey);
            //if (cacheModel == null)
            //{
            //    _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaUniqueLearnerNumberViewModel from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
            //    return RedirectToRoute(RouteConstants.PageNotFound);
            //}

            //return View(new RequestSoaUlnNotFoundViewModel { Uln = cacheModel.Uln?.EnterUln?.ToString() });

            await Task.CompletedTask;
            return View(new RequestSoaUlnNotFoundViewModel { Uln = "1234567890" });
        }

        private bool IsSoaAvailable()
        {
            return _configuration.SoaAvailableDate == null || DateTime.UtcNow.Date >= _configuration.SoaAvailableDate.Value.Date;
        }

        private async Task<bool> IsAddressAvailable()
        {
            return await _providerAddress.GetAddressAsync<Address>(User.GetUkPrn()) != null;
        }
    }
}