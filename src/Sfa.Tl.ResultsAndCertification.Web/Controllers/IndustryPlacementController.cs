using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class IndustryPlacementController : Controller
    {
        private readonly IIndustryPlacementLoader _industryPlacementLoader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.IpCacheKey);
        private string TrainingProviderCacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.TrainingProviderCacheKey);

        public IndustryPlacementController(IIndustryPlacementLoader industryPlacementLoader, ICacheService cacheService, ILogger<IndustryPlacementController> logger)
        {
            _industryPlacementLoader = industryPlacementLoader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("add-industry-placement/{profileId}", Name = RouteConstants.AddIndustryPlacement)]
        public async Task<IActionResult> AddIndustryPlacementAsync(int profileId)
        {
            await _cacheService.RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.IpCompletion, new { profileId });
        }

        [HttpGet]
        [Route("industry-placement-completion/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.IpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(int profileId, bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            var viewModel = cacheModel?.IpCompletion;
            if (viewModel == null)
            {
                viewModel = await _industryPlacementLoader.GetLearnerRecordDetailsAsync<IpCompletionViewModel>(User.GetUkPrn(), profileId);
                if (viewModel == null || !viewModel.IsValid)
                    return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.IsChangeMode = (isChangeMode || (cacheModel?.IpCompletion?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;
            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-completion/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(IpCompletionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.IsChangeMode)
            {
                // TODO: revisit below logic when we pick associate story. 
                var existingCacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

                if (existingCacheModel == null)
                    return RedirectToRoute(RouteConstants.PageNotFound);

                // if selection doesn't change then redirect to Ip check and submit page
                if (model.IndustryPlacementStatus == existingCacheModel.IpCompletion.IndustryPlacementStatus.Value)
                {
                    // check based on selection to see if it is valid to redirect to check and submit page
                    if (model.IndustryPlacementStatus == IndustryPlacementStatus.Completed ||
                        (model.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration &&
                            existingCacheModel?.SpecialConsideration?.Hours != null && existingCacheModel?.SpecialConsideration?.Reasons != null))
                        return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
                }
            }

            var cacheModel = await SyncCacheIp(model);

            if (model.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.IpSpecialConsiderationHours);

            return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
        }

        #region SpecialConsideration

        [HttpGet]
        [Route("industry-placement-special-consideration-hours/{isChangeMode:bool?}", Name = RouteConstants.IpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.SpecialConsideration?.Hours) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<SpecialConsiderationHoursViewModel>(cacheModel.IpCompletion);
            viewModel.IsChangeMode = (isChangeMode || (cacheModel?.SpecialConsideration?.Hours.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;
            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-hours/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync(SpecialConsiderationHoursViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus !=
                IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (cacheModel?.SpecialConsideration == null)
                cacheModel.SpecialConsideration = new SpecialConsiderationViewModel();

            cacheModel.SpecialConsideration.Hours = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(model.IsChangeMode ? RouteConstants.IpCheckAndSubmit : RouteConstants.IpSpecialConsiderationReasons);
        }

        [HttpGet]
        [Route("industry-placement-special-consideration-reasons/{isChangeMode:bool?}", Name = RouteConstants.IpSpecialConsiderationReasons)]
        public async Task<IActionResult> IpSpecialConsiderationReasonsAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = cacheModel.SpecialConsideration?.Reasons;

            if (viewModel == null)
            {
                viewModel = await _industryPlacementLoader.TransformIpCompletionDetailsTo<SpecialConsiderationReasonsViewModel>(cacheModel.IpCompletion);
                viewModel.ReasonsList = await _industryPlacementLoader.GetSpecialConsiderationReasonsListAsync(viewModel.AcademicYear);
            }

            viewModel.IsChangeMode = (isChangeMode || (cacheModel?.SpecialConsideration?.Reasons?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-reasons/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpSpecialConsiderationReasons)]
        public async Task<IActionResult> IpSpecialConsiderationReasonsAsync(SpecialConsiderationReasonsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.SpecialConsideration.Reasons = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
        }

        #endregion

        #region CheckAndSubmit
        [HttpGet]
        [Route("industry-placement-check-your-answers", Name = RouteConstants.IpCheckAndSubmit)]
        public async Task<IActionResult> IpCheckAndSubmitAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = await _industryPlacementLoader.GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(User.GetUkPrn(), cacheModel.IpCompletion.ProfileId);
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            // Item1 contain - Questions List & Item2 contain IsValid flag
            var ipDetailsList = _industryPlacementLoader.GetIpSummaryDetailsListAsync(cacheModel);
            if (!ipDetailsList.Item2 || ipDetailsList.Item1 == null || !ipDetailsList.Item1.Any())
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IpDetailsList = ipDetailsList.Item1;

            viewModel.SetDeclarationText(cacheModel);
            viewModel.SetBackLink(cacheModel);

            cacheModel.ResetChangeMode();
            cacheModel.IsChangeModeAllowed = true; // TODO: Significance.
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-check-your-answers", Name = RouteConstants.SubmitIpCheckAndSubmit)]
        public async Task<IActionResult> IpCheckAndSubmitSaveAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var isSuccess = await _industryPlacementLoader.ProcessIndustryPlacementDetailsAsync(User.GetUkPrn(), cacheModel);

            if (isSuccess)
            {
                await _cacheService.RemoveAsync<IndustryPlacementViewModel>(CacheKey);

                var notificationBanner = new NotificationBannerModel
                {
                    HeaderMessage = IndustryPlacementBanner.Banner_HeaderMesage,
                    Message = IndustryPlacementBanner.Success_Message,
                    DisplayMessageBody = true,
                    IsRawHtml = true
                };

                await _cacheService.SetAsync(TrainingProviderCacheKey, notificationBanner, CacheExpiryTime.XSmall);
                return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = cacheModel.IpCompletion.ProfileId });
            }
            else
            {
                _logger.LogWarning(LogEvent.ManualIndustryPlacementDetailsProcessFailed, $"Unable to add industry placement status for ProfileId = {cacheModel.IpCompletion.ProfileId}. Method: IpCheckAndSubmitSave, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }
        }

        [HttpGet]
        [Route("industry-placement-check-your-answers-cancel/{profileId}", Name = RouteConstants.IpCheckAndSubmitCancel)]
        public async Task<IActionResult> IpCheckAndSubmitCancelAsync(int profileId)
        {
            await _cacheService.RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId });
        }

        #endregion

        private async Task<IndustryPlacementViewModel> SyncCacheIp(IpCompletionViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion != null)
            {
                cacheModel.IpCompletion = model;

                if (model.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                    cacheModel.SpecialConsideration = null;
            }
            else
                cacheModel = new IndustryPlacementViewModel
                {
                    IpCompletion = model
                };

            await _cacheService.SetAsync(CacheKey, cacheModel);
            return cacheModel;
        }
    }
}
