using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
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
        private readonly ICacheService _cacheService;
        ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        private string CacheKey { get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.SoaCacheKey); } }

        public StatementOfAchievementController(IStatementOfAchievementLoader statementOfAchievementLoader, IProviderAddressLoader providerAddress, ICacheService cacheService, ResultsAndCertificationConfiguration configuration, ILogger<StatementOfAchievementController> logger)
        {
            _statementOfAchievementLoader = statementOfAchievementLoader;
            _providerAddress = providerAddress;
            _cacheService = cacheService;
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

            var cacheModel = await _cacheService.GetAsync<RequestSoaUniqueLearnerNumberViewModel>(CacheKey);
            var viewModel = cacheModel ?? new RequestSoaUniqueLearnerNumberViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Route("request-statement-of-achievement-unique-learner-number", Name = RouteConstants.SubmitRequestSoaUniqueLearnerNumber)]
        public async Task<IActionResult> RequestSoaUniqueLearnerNumberAsync(RequestSoaUniqueLearnerNumberViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var soaLearnerRecord = await _statementOfAchievementLoader.FindSoaLearnerRecordAsync(User.GetUkPrn(), model.SearchUln.ToLong());
            await _cacheService.SetAsync(CacheKey, model);
            
            if (soaLearnerRecord == null || !soaLearnerRecord.IsLearnerRegistered)
            {
                await _cacheService.SetAsync(CacheKey, new RequestSoaUlnNotFoundViewModel { Uln = model.SearchUln });
                return RedirectToRoute(RouteConstants.RequestSoaUlnNotFound);
            }
            else if (soaLearnerRecord.IsNotWithdrawn)
            {
                await _cacheService.SetAsync(CacheKey, new RequestSoaUlnNotWithdrawnViewModel { Uln = soaLearnerRecord.Uln, LearnerName = soaLearnerRecord.LearnerName, DateofBirth = soaLearnerRecord.DateofBirth, ProviderName = soaLearnerRecord.ProviderName, TLevelTitle = soaLearnerRecord.TlevelTitle });
                return RedirectToRoute(RouteConstants.RequestSoaUlnNotWithdrawn);
            }
            return RedirectToRoute(RouteConstants.RequestSoaUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-ULN-not-registered", Name = RouteConstants.RequestSoaUlnNotFound)]
        public async Task<IActionResult> RequestSoaUlnNotFoundAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<RequestSoaUlnNotFoundViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaUlnNotFoundViewModel from redis cache in request soa uln not registered page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(cacheModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-ULN-not-withdrawn", Name = RouteConstants.RequestSoaUlnNotWithdrawn)]
        public async Task<IActionResult> RequestSoaUlnNotWithdrawnAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<RequestSoaUlnNotWithdrawnViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaUlnNotWithdrawnViewModel from redis cache in request soa uln not withdrawn page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
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