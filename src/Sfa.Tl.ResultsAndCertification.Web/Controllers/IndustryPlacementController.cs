using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class IndustryPlacementController : Controller
    {
        private readonly IIndustryPlacementLoader _industryPlacementLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.IpCacheKey); }
        }

        public IndustryPlacementController(IIndustryPlacementLoader industryPlacementLoader, ICacheService cacheService, ILogger<IndustryPlacementController> logger)
        {
            _industryPlacementLoader = industryPlacementLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("industry-placement-completion/{profileId}", Name = RouteConstants.IpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(int profileId)
        {
            var viewModel = await _industryPlacementLoader.GetLearnerRecordDetailsAsync<IpCompletionViewModel>(User.GetUkPrn(), profileId);

            if (viewModel == null || !viewModel.IsValid)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-completion/{profileId}", Name = RouteConstants.SubmitIpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(IpCompletionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await SyncCacheIp(model);

            if (model.IndustryPlacementStatus == IndustryPlacementStatus.Completed)
            {
                return RedirectToRoute(RouteConstants.IpModelUsed, new { profileId = model.ProfileId });
            }

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        #region Ip Models

        [HttpGet]
        [Route("industry-placement-model-used", Name = RouteConstants.IpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == null || cacheModel.IpCompletion.IndustryPlacementStatus != IndustryPlacementStatus.Completed)//todo - second entry point requires CompletedWithSpecialConsideration
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpModelUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpModelUsedViewModel>(cacheModel.IpCompletion);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-model-used", Name = RouteConstants.SubmitIpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync(IpModelUsedViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (cacheModel?.IpModelViewModel == null)
                cacheModel.IpModelViewModel = new IpModelViewModel();

            cacheModel.IpModelViewModel.IpModelUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpMultiEmployerUsed);
        }

        [HttpGet]
        [Route("industry-placement-multiple-employer-model", Name = RouteConstants.IpMultiEmployerUsed)]
        public async Task<IActionResult> IpMultiEmployerUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);           

            if (cacheModel?.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == null || cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpMultiEmployerUsedViewModel>(cacheModel?.IpCompletion);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-multiple-employer-model", Name = RouteConstants.SubmitIpMultiEmployerUsed)]
        public async Task<IActionResult> IpMultiEmployerUsedAsync(IpMultiEmployerUsedViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);
            
            cacheModel.IpModelViewModel.IpMultiEmployerUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if(model.IsMultiEmployerModelUsed.Value)
            {
                return RedirectToRoute(RouteConstants.IpMultiEmployerOther);
            }
            return View(model);
        }

        [HttpGet]
        [Route("industry-placement-other-models", Name = RouteConstants.IpMultiEmployerOther)]
        public async Task<IActionResult> IpMultiEmployerOtherAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null || cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = await _industryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(IpLookupType.IndustryPlacementModel, cacheModel.IpCompletion.LearnerName, cacheModel.IpCompletion.PathwayId, true);

            return View(viewModel);
        }

        #endregion

        #region SpecialConsideration

        [HttpGet]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.IpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialConsideration == null ? new SpecialConsiderationHoursViewModel() : cacheModel.SpecialConsideration.Hours;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.SubmitIpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync(SpecialConsiderationHoursViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (cacheModel?.SpecialConsideration == null)
                cacheModel.SpecialConsideration = new SpecialConsiderationViewModel();

            cacheModel.SpecialConsideration.Hours = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpSpecialConsiderationReasons);
        }

        [HttpGet]
        [Route("industry-placement-special-consideration-reasons", Name = RouteConstants.IpSpecialConsiderationReasons)]
        public async Task<IActionResult> IpSpecialConsiderationReasonsAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialConsideration?.Reasons == null ? new SpecialConsiderationReasonsViewModel() : cacheModel.SpecialConsideration.Reasons;
            // TODO: Assigin viewMoel with List of Reasons. 

            viewModel.ReasonsList = await _industryPlacementLoader.GetIpLookupDataAsync(IpLookupType.SpecialConsideration);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-reasons", Name = RouteConstants.SubmitIpSpecialConsiderationReasons)]
        public async Task<IActionResult> SubmitIpSpecialConsiderationReasonsAsync(SpecialConsiderationReasonsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            // TODO: We might need to Assigin model with List of Reasons. 

            cacheModel.SpecialConsideration.Reasons = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpModelUsed);
        }

        #endregion

        private async Task SyncCacheIp(IpCompletionViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion != null)
                cacheModel.IpCompletion = model;
            else
                cacheModel = new IndustryPlacementViewModel { 
                    IpCompletion = model 
                };

            await _cacheService.SetAsync(CacheKey, cacheModel);
        }
    }
}
