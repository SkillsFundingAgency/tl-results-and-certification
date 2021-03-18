using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
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

            var learnerRecord = await _trainingProviderLoader.FindLearnerRecordAsync(User.GetUkPrn(), model.EnterUln.ToLong());
            if (learnerRecord == null || !learnerRecord.IsLearnerRegistered || learnerRecord.IsLearnerRecordAdded)
            {
                await SyncCacheUln(model);
                return RedirectToRoute(learnerRecord == null || learnerRecord.IsLearnerRegistered == false ? 
                    RouteConstants.EnterUniqueLearnerNumberNotFound : RouteConstants.EnterUniqueLearnerNumberAddedAlready);
            }

            await SyncCacheUln(model, learnerRecord);
            if (!learnerRecord.HasLrsEnglishAndMaths)
                return RedirectToRoute(RouteConstants.AddEnglishAndMathsQuestion);

            if (learnerRecord.HasLrsEnglishAndMaths && !learnerRecord.HasSendQualification)
                return RedirectToRoute(RouteConstants.AddIndustryPlacementQuestion);
            
            return RedirectToRoute(RouteConstants.EnterUniqueLearnerNumber);
        }

        [HttpGet]
        [Route("add-learner-record-ULN-already-added", Name = RouteConstants.EnterUniqueLearnerNumberAddedAlready)]
        public async Task<IActionResult> EnterUniqueLearnerNumberAddedAlreadyAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddLearnerRecordViewModel from redis cache in Uln already added page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(new LearnerRecordAddedAlreadyViewModel { Uln = cacheModel.Uln?.EnterUln?.ToString() });
        }

        [HttpGet]
        [Route("add-learner-record-ULN-not-registered", Name = RouteConstants.EnterUniqueLearnerNumberNotFound)]
        public async Task<IActionResult> EnterUniqueLearnerNumberNotFoundAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to read AddLearnerRecordViewModel from redis cache in enter uln not found page. Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(new LearnerRecordNotFoundViewModel { Uln = cacheModel.Uln?.EnterUln?.ToString() });
        }

        [HttpGet]
        [Route("add-learner-record-english-and-maths-achievement", Name = RouteConstants.AddEnglishAndMathsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsQuestionAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.EnglishAndMathsQuestion == null ? new EnglishAndMathsQuestionViewModel() : cacheModel.EnglishAndMathsQuestion;
            viewModel.LearnerName = cacheModel.LearnerRecord.Name;
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-english-and-maths-achievement", Name = RouteConstants.SubmitAddEnglishAndMathsQuestion)]
        public async Task<IActionResult> AddEnglishAndMathsQuestionAsync(EnglishAndMathsQuestionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);
            if (cacheModel?.Uln == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.EnglishAndMathsQuestion = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.AddIndustryPlacementQuestion);
        }

        [HttpGet]
        [Route("has-learner-completed-industry-placement/{isChangeMode:bool?}", Name = RouteConstants.AddIndustryPlacementQuestion)]
        public async Task<IActionResult> AddIndustryPlacementQuestionAsync(bool isChangeMode)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null || cacheModel?.Uln == null || cacheModel?.LearnerRecord.IsLearnerRegistered == false || 
                (cacheModel?.LearnerRecord?.HasLrsEnglishAndMaths == false && cacheModel?.EnglishAndMathsQuestion == null))
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.IndustryPlacementQuestion == null ? new IndustryPlacementQuestionViewModel() : cacheModel.IndustryPlacementQuestion;
            viewModel.LearnerName = cacheModel.LearnerRecord.Name;
            viewModel.IsChangeMode = isChangeMode && cacheModel.IsChangeModeAllowed;
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
            
            return RedirectToRoute(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }

        [HttpGet]
        [Route("add-learner-record-check-and-submit", Name = RouteConstants.AddLearnerRecordCheckAndSubmit)]
        public async Task<IActionResult> AddLearnerRecordCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            var viewModel = new CheckAndSubmitViewModel { LearnerRecordModel = cacheModel };

            if (!viewModel.IsCheckAndSubmitPageValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpGet]
        [Route("add-learner-record-cancel", Name = RouteConstants.AddLearnerRecordCancel)]
        public async Task<IActionResult> AddLearnerRecordCancelAsync()
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.LearnerRecord == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = new LearnerRecordCancelViewModel { LearnerName = cacheModel.LearnerRecord.Name, CancelLearnerRecord = true };
            return View(viewModel);
        }

        [HttpPost]
        [Route("add-learner-record-cancel", Name = RouteConstants.SubmitLearnerRecordCancel)]
        public async Task<IActionResult> AddLearnerRecordCancelAsync(LearnerRecordCancelViewModel viewModel)
        {
            if (viewModel.CancelLearnerRecord)
            {
                await _cacheService.RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
                return RedirectToRoute(RouteConstants.ManageLearnerRecordsDashboard);
            }
            else
                return RedirectToRoute(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }


        private async Task SyncCacheUln(EnterUlnViewModel model, FindLearnerRecord learnerRecord = null)
        {
            var cacheModel = await _cacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey);

            if (cacheModel?.Uln != null && cacheModel?.Uln?.EnterUln == model.EnterUln)
            {
                cacheModel.LearnerRecord = learnerRecord;
                cacheModel.Uln = model;
            }
            else
                cacheModel = new AddLearnerRecordViewModel { LearnerRecord = learnerRecord, Uln = model };

            await _cacheService.SetAsync(CacheKey, cacheModel);
        }
    }
}