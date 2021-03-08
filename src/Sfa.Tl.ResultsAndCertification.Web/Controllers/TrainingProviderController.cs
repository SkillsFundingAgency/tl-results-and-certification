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
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            var viewModel = cacheModel?.Uln != null ? cacheModel.Uln : new EnterUlnViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-unique-learner-number", Name = RouteConstants.SubmitEnterUniqueLearnerNumber)]
        public async Task<IActionResult> EnterUniqueLearnerReferenceAsync(EnterUlnViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var learnerRecord = await _trainingProviderLoader.FindLearnerRecordAsync(User.GetUkPrn(isProvider: true), model.EnterUln.ToLong());

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel?.Uln != null)
                cacheModel.Uln = model;
            else
                cacheModel = new AddLearnerRecordViewModel { LearnerRecord = learnerRecord, Uln = model };

            await _cacheService.SetAsync(CacheKey, cacheModel);

            if (learnerRecord == null || !learnerRecord.IsLearnerRegistered)
            {
                var ulnNotFoundViewModel = new LearnerRecordNotFoundViewModel { Uln = model.EnterUln.ToString() };
                await _cacheService.SetAsync(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberNotFound), ulnNotFoundViewModel, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.EnterUniqueLearnerNumberNotFound);
            }

            if (learnerRecord.IsSendQualification)
            {
                return RedirectToRoute(RouteConstants.AddIndustryPlacementQuestion);
            }
            
            return RedirectToRoute(RouteConstants.EnterUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("add-learner-record-ULN-not-registered", Name = RouteConstants.EnterUniqueLearnerNumberNotFound)]
        public async Task<IActionResult> EnterUniqueLearnerNumberNotFoundAsync()
        {
            var viewModel = await _cacheService.GetAndRemoveAsync<LearnerRecordNotFoundViewModel>(string.Concat(CacheKey, Constants.EnterUniqueLearnerNumberNotFound));
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read EnterUniqueLearnerNumberNotFound from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("has-learner-completed-industry-placement", Name = RouteConstants.AddIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.LearnerRecord.IsSendQualification == false || cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.IndustryPlacementQuestion == null ? new IndustryPlacementQuestionViewModel() : cacheModel.IndustryPlacementQuestion;
            return View(viewModel);
        }

        [HttpPost]
        [Route("has-learner-completed-industry-placement", Name = RouteConstants.SubmitIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync(IndustryPlacementQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.IndustryPlacementQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddIndustryPlacementQuestion);
        }
    }
}