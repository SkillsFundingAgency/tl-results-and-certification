using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class TrainingProviderController : Controller
    {
        private readonly ITrainingProviderLoader _trainingProviderLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TrainingProviderCacheKey); }
        }

        public TrainingProviderController(ITrainingProviderLoader trainingProviderLoader, ICacheService cacheService, ILogger<TrainingProviderController> logger)
        {
            _trainingProviderLoader = trainingProviderLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("manage-learner-records", Name = RouteConstants.ManageLearnerRecordsDashboard)]
        public IActionResult Index()
        {
            return View(new DashboardViewModel());
        }

        [HttpGet]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.EnterUniqueLearnerNumber)]
        public async Task<IActionResult> EnterUniqueLearnerReference()
        {
            var defaultValue = await _cacheService.GetAndRemoveAsync<string>(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberCriteria));
            var viewModel = new EnterUlnViewModel { EnterUln = defaultValue };
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.SubmitEnterUniqueLearnerNumber)]
        public async Task<IActionResult> EnterUniqueLearnerReferenceAsync(EnterUlnViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var isFound = await _trainingProviderLoader.FindProvidersUlnAsync(User.GetUkPrn(isProvider: true), model.EnterUln.ToLong());
            if (!isFound)
            {
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberCriteria), model.EnterUln);

                var ulnNotFoundViewModel = new ProvidersUlnNotFoundViewModel { Uln = model.EnterUln.ToString() };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberNotFound), ulnNotFoundViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.EnterUniqueLearnerNumberNotFound);
            }

            return RedirectToRoute(RouteConstants.EnterUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("add-learner-record-ULN-not-registered", Name = RouteConstants.EnterUniqueLearnerNumberNotFound)]
        public async Task<IActionResult> EnterUniqueLearnerNumberNotFoundAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<ProvidersUlnNotFoundViewModel>(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberNotFound));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read EnterUniqueLearnerNumberNotFound from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }
    }
}
