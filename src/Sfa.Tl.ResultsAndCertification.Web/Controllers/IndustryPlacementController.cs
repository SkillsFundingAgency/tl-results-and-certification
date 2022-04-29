﻿using Microsoft.AspNetCore.Authorization;
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

            if (model.IndustryPlacementStatus == Common.Enum.IndustryPlacementStatus.Completed)
            {
                return RedirectToRoute(RouteConstants.IpModelUsed, new { profileId = model.ProfileId });
            }

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        [HttpGet]
        [Route("industry-placement-model-used", Name = RouteConstants.IpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == null || cacheModel.IpCompletion.IndustryPlacementStatus != IndustryPlacementStatus.Completed)//todo - second entry point requires CompletedWithSpecialConsideration
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = await _industryPlacementLoader.TransformFromLearnerDetailsTo<IpModelUsedViewModel>(cacheModel.IpCompletion);

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

            cacheModel.IpModelViewModel = new IpModelViewModel { IpModelUsed = model };
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return View(model);
        }

        #region SpecialConsideration

        [HttpGet]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.IpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel?.SpecialConsideration == null ? new SpecialConsiderationViewModel().PlacementHours : cacheModel.SpecialConsideration.PlacementHours;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.SubmitIpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync(PlacementHoursViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await Task.CompletedTask;
            return View(model);
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
