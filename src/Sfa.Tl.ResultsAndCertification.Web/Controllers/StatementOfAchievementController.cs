using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
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
        private readonly ResultsAndCertificationConfiguration _configuration;
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

            await _cacheService.RemoveAsync<RequestSoaUniqueLearnerNumberViewModel>(CacheKey);
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
                await _cacheService.SetAsync(CacheKey, new RequestSoaUlnNotFoundViewModel { Uln = model.SearchUln }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RequestSoaUlnNotFound);
            }
            else if (soaLearnerRecord.IsNotWithdrawn)
            {
                await _cacheService.SetAsync(CacheKey, new RequestSoaUlnNotWithdrawnViewModel { Uln = soaLearnerRecord.Uln, LearnerName = soaLearnerRecord.LearnerName, DateofBirth = soaLearnerRecord.DateofBirth, ProviderName = soaLearnerRecord.ProviderName, TLevelTitle = soaLearnerRecord.TlevelTitle }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RequestSoaUlnNotWithdrawn);
            }
            else if (!soaLearnerRecord.IsIndustryPlacementAdded)
            {
                await _cacheService.SetAsync(CacheKey, new RequestSoaNotAvailableNoIpStatusViewModel { Uln = soaLearnerRecord.Uln, LearnerName = soaLearnerRecord.LearnerName, DateofBirth = soaLearnerRecord.DateofBirth, ProviderName = soaLearnerRecord.ProviderName, TLevelTitle = soaLearnerRecord.TlevelTitle }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RequestSoaNotAvailableNoIpStatus);
            }
            else if (soaLearnerRecord.HasPathwayResult == false && !soaLearnerRecord.IsIndustryPlacementCompleted)
            {
                await _cacheService.SetAsync(CacheKey, new RequestSoaNotAvailableNoResultsViewModel { ProfileId = soaLearnerRecord.ProfileId, Uln = soaLearnerRecord.Uln, LearnerName = soaLearnerRecord.LearnerName, DateofBirth = soaLearnerRecord.DateofBirth, ProviderName = soaLearnerRecord.ProviderName, TLevelTitle = soaLearnerRecord.TlevelTitle }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RequestSoaNotAvailableNoResults);
            }
            else
            {
                await _cacheService.RemoveAsync<FindSoaLearnerRecord>(CacheKey);
                return RedirectToRoute(RouteConstants.RequestSoaCheckAndSubmit, new { profileId = soaLearnerRecord.ProfileId });
            }
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

        [HttpGet]
        [Route("/request-statement-of-achievement-not-available-no-ip-status", Name = RouteConstants.RequestSoaNotAvailableNoIpStatus)]
        public async Task<IActionResult> RequestSoaNotAvailableNoIpStatusAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<RequestSoaNotAvailableNoIpStatusViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaNotAvailableNoIpStatusViewModel from redis cache in request soa not available no ip status page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(cacheModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-not-available-no-results", Name = RouteConstants.RequestSoaNotAvailableNoResults)]
        public async Task<IActionResult> RequestSoaNotAvailableNoResultsAsync()
        {
            var cacheModel = await _cacheService.GetAndRemoveAsync<RequestSoaNotAvailableNoResultsViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read RequestSoaNotAvailableNoResultsViewModel from redis cache in request soa not available no results page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(cacheModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-check-and-submit/{profileId}", Name = RouteConstants.RequestSoaCheckAndSubmit)]
        public async Task<IActionResult> RequestSoaCheckAndSubmitAsync(int profileId)
        {
            var viewModel = await _statementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(User.GetUkPrn(), profileId);
            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (viewModel.IsRequestedAlready)
                return RedirectToRoute(RouteConstants.RequestSoaSubmittedAlready, new { ProfileId = profileId, PathwayId = viewModel.RegistrationPathwayId });

            return View(viewModel);
        }

        [HttpPost]
        [Route("request-statement-of-achievement-check-and-submit", Name = RouteConstants.SubmitRequestSoaCheckAndSubmit)]
        public async Task<IActionResult> SubmitRequestSoaCheckAndSubmitAsync(SoaLearnerRecordDetailsViewModel viewModel)
        {
            var soaDetails = await _statementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(User.GetUkPrn(), viewModel.ProfileId);
            if (soaDetails == null || !soaDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var response = await _statementOfAchievementLoader.CreateSoaPrintingRequestAsync(User.GetUkPrn(), soaDetails);

            if (response.IsSuccess)
            {
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.RequestSoaConfirmation), new SoaConfirmationViewModel { Uln = response.Uln, Name = response.LearnerName }, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.RequestSoaConfirmation);
            }
            else
            {
                _logger.LogWarning(LogEvent.AddLearnerRecordFailed, $"Unable to request statement of achievement for UniqueLearnerNumber: {viewModel.Uln}. Method: SubmitRequestSoaCheckAndSubmitAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.Error, new { StatusCode = 500 });
            }
        }

        [HttpGet]
        [Route("request-statement-of-achievement-confirmation", Name = RouteConstants.RequestSoaConfirmation)]
        public async Task<IActionResult> RequestSoaConfirmationAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<SoaConfirmationViewModel>(string.Concat(CacheKey, Constants.RequestSoaConfirmation));

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.ConfirmationPageFailed, $"Unable to read SoaConfirmationViewModel from redis cache in request soa confirmation page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-cancel/{profileId}", Name = RouteConstants.RequestSoaCancel)]
        public async Task<IActionResult> RequestSoaCancelAsync(int profileId)
        {
            var learnerDetails = await _statementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(User.GetUkPrn(), profileId);
            if (learnerDetails == null || !learnerDetails.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new RequestSoaCancelViewModel { ProfileId = learnerDetails.ProfileId, LearnerName = learnerDetails.LearnerName };
            return View(viewModel);
        }

        [HttpPost]
        [Route("request-statement-of-achievement-cancel", Name = RouteConstants.SubmitRequestSoaCancel)]
        public IActionResult RequestSoaCancel(RequestSoaCancelViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            return viewModel.CancelRequest.Value
                ? RedirectToRoute(RouteConstants.Home)
                : RedirectToRoute(RouteConstants.RequestSoaCheckAndSubmit, new { profileId = viewModel.ProfileId });
        }

        [HttpGet]
        [Route("request-statement-of-achievement-change-learner-components/{profileId}", Name = RouteConstants.RequestSoaChangeComponentAchievements)]
        public IActionResult ChangeComponentAchievement(int profileId)
        {
            var viewModel = new ChangeComponentAchievementsViewModel { ProfileId = profileId };
            return View(viewModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-change-postal-address/{profileId}", Name = RouteConstants.RequestSoaChangePostalAddress)]
        public IActionResult ChangePostalAddress(int profileId)
        {
            var viewModel = new ChangePostalAddressViewModel { ProfileId = profileId };
            return View(viewModel);
        }

        [HttpGet]
        [Route("request-statement-of-achievement-already-requested/{profileId}/{pathwayId}", Name = RouteConstants.RequestSoaSubmittedAlready)]
        public async Task<IActionResult> RequestSoaSubmittedAlreadyAsync(int profileId, int pathwayId)
        {
            var viewModel = await _statementOfAchievementLoader.GetPrintRequestSnapshotAsync(User.GetUkPrn(), profileId, pathwayId);
            return View(viewModel);
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