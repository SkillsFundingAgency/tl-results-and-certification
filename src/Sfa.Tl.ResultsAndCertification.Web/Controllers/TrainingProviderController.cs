using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
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
        public async Task<IActionResult> EnterUniqueLearnerReferenceAsync()
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

            var isFound = await _trainingProviderLoader.FindLearnerRecordAsync(User.GetUkPrn(isProvider: true), model.EnterUln.ToLong());
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
        [Route("has-learner-completed-industry-placement", Name = RouteConstants.AddIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.IndustryPlacementQuestion == null ? new IndustryPlacementQuestionViewModel() : cacheModel.IndustryPlacementQuestion;
            return View(viewModel);
        }
    }
}
