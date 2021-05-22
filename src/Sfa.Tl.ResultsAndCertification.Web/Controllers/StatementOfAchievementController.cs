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
        private readonly IStatementOfAchievementLoader _statementOfAchievementLoader;
        private readonly IProviderAddressLoader _providerAddress;
        ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        public StatementOfAchievementController(IStatementOfAchievementLoader statementOfAchievementLoader, IProviderAddressLoader providerAddress, ResultsAndCertificationConfiguration configuration, ILogger<StatementOfAchievementController> logger)
        {
            _statementOfAchievementLoader = statementOfAchievementLoader;
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

            return View(new RequestSoaUniqueLearnerNumberViewModel());
        }

        [HttpPost]
        [Route("request-statement-of-achievement-unique-learner-number", Name = RouteConstants.SubmitRequestSoaUniqueLearnerNumber)]
        public async Task<IActionResult> RequestSoaUniqueLearnerNumberAsync(RequestSoaUniqueLearnerNumberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var viewModel = await _statementOfAchievementLoader.FindSoaLearnerRecordAsync(User.GetUkPrn(), 123456789);

            // TODO : Save RequestSoaUniqueLearnerNumberViewModel to cache

            if (viewModel == null)
            {
                // TODO: Save RequestSoaUlnNotFoundViewModel to cache
                var mdoel = new RequestSoaUlnNotFoundViewModel { Uln = model.SearchUln };
                // save to cache
            }
            else if (!viewModel.IsNotWithdrawn)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            else if (viewModel.IsNotWithdrawn)
            {
                // TODO: Save RequestSoaUlnNotWithdrawnViewModel to cache
                return RedirectToRoute(RouteConstants.RequestSoaUlnNotWithdrawn);
            }

            await Task.CompletedTask;
            return RedirectToRoute(RouteConstants.RequestSoaUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-ULN-not-registered", Name = RouteConstants.RequestSoaUlnNotFound)]
        public async Task<IActionResult> RequestSoaUlnNotFoundAsync()
        {
            //var cacheModel = await _cacheService.GetAsync<RequestSoaUlnNotFoundViewModel>(CacheKey);
            //if (cacheModel == null)
            //{
            //    _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaUlnNotFoundViewModel from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
            //    return RedirectToRoute(RouteConstants.PageNotFound);
            //}

            //return View(cacheModel);

            await Task.CompletedTask;
            return View(new RequestSoaUlnNotFoundViewModel { Uln = "1234567890" });
        }

        [HttpGet]
        [Route("request-statement-of-achievement-ULN-not-withdrawn", Name = RouteConstants.RequestSoaUlnNotWithdrawn)]
        public async Task<IActionResult> RequestSoaUlnNotWithdrawnAsync()
        {
            await Task.CompletedTask;
            return View(new RequestSoaUlnNotWithdrawnViewModel());
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