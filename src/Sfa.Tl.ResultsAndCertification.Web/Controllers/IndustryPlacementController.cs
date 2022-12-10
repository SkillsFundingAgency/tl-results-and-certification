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

        //[HttpPost]
        //[Route("industry-placement-completion/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpCompletion)]
        //public async Task<IActionResult> IpCompletionAsync(IpCompletionViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    if (model.IsChangeMode)
        //    {
        //        var existingCacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

        //        if (existingCacheModel == null)
        //            return RedirectToRoute(RouteConstants.PageNotFound);

        //        // if selection doesn't change then redirect to Ip check and submit page
        //        if (model.IndustryPlacementStatus == existingCacheModel.IpCompletion.IndustryPlacementStatus.Value)
        //        {
        //            // check based on selection to see if it is valid to redirect to check and submit page
        //            if (model.IndustryPlacementStatus == IndustryPlacementStatus.Completed ||
        //                (model.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration &&
        //                    existingCacheModel?.SpecialConsideration?.Hours != null && existingCacheModel?.SpecialConsideration?.Reasons != null))
        //                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
        //        }
        //    }

        //    var cacheModel = await SyncCacheIp(model);

        //    switch (model.IndustryPlacementStatus)
        //    {
        //        case IndustryPlacementStatus.Completed:
        //            return RedirectToRoute(model.IsChangeMode ? RouteConstants.IpCheckAndSubmit : RouteConstants.IpModelUsed);
        //        case IndustryPlacementStatus.CompletedWithSpecialConsideration:
        //            return RedirectToRoute(RouteConstants.IpSpecialConsiderationHours);
        //        case IndustryPlacementStatus.NotCompleted:
        //            {
        //                var isSuccess = await _industryPlacementLoader.ProcessIndustryPlacementDetailsAsync(User.GetUkPrn(), cacheModel);

        //                if (isSuccess)
        //                {
        //                    await _cacheService.RemoveAsync<IndustryPlacementViewModel>(CacheKey);

        //                    var notificationBanner = new NotificationBannerModel
        //                    {
        //                        HeaderMessage = IndustryPlacementBanner.Banner_HeaderMesage,
        //                        Message = IndustryPlacementBanner.Success_Message,
        //                        DisplayMessageBody = true,
        //                        IsRawHtml = true
        //                    };

        //                    await _cacheService.SetAsync(TrainingProviderCacheKey, notificationBanner, CacheExpiryTime.XSmall);
        //                    return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        //                }
        //                else
        //                {
        //                    _logger.LogWarning(LogEvent.ManualIndustryPlacementDetailsProcessFailed, $"Unable to add industry placement status for ProfileId = {cacheModel.IpCompletion.ProfileId}. Method: IpCompletionAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
        //                    return RedirectToRoute(RouteConstants.ProblemWithService);
        //                }
        //            }
        //        default:
        //            return RedirectToRoute(RouteConstants.PageNotFound);
        //    }
        //}

        [HttpPost]
        [Route("industry-placement-completion/{profileId}/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(IpCompletionViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToRoute(RouteConstants.ProblemWithService); // TODO: Work in progress. 


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

            switch (model.IndustryPlacementStatus)
            {
                case IndustryPlacementStatus.Completed:
                    return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
                case IndustryPlacementStatus.CompletedWithSpecialConsideration:
                    return RedirectToRoute(RouteConstants.IpSpecialConsiderationHours);
                case IndustryPlacementStatus.NotCompleted:
                    {
                        // TODO: Other story?
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
                            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
                        }
                        else
                        {
                            _logger.LogWarning(LogEvent.ManualIndustryPlacementDetailsProcessFailed, $"Unable to add industry placement status for ProfileId = {cacheModel.IpCompletion.ProfileId}. Method: IpCompletionAsync, Ukprn: {User.GetUkPrn()}, User: {User.GetUserEmail()}");
                            return RedirectToRoute(RouteConstants.ProblemWithService);
                        }
                    }
                // TODO: Another option to be added. 
                default:
                    return RedirectToRoute(RouteConstants.PageNotFound);
            }
        }

        #region Ip Models

        [HttpGet]
        [Route("industry-placement-model/{isChangeMode:bool?}", Name = RouteConstants.IpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == null ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var viewModel = cacheModel.IpModelViewModel?.IpModelUsed ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpModelUsedViewModel>(cacheModel.IpCompletion);
            viewModel.IsChangeMode = (isChangeMode || (cacheModel.IpModelViewModel?.IpModelUsed?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;

            viewModel.SetBackLink(cacheModel.SpecialConsideration);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-model/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync(IpModelUsedViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (!ModelState.IsValid)
            {
                model.SetBackLink(cacheModel?.SpecialConsideration);
                return View(model);
            }

            if (cacheModel == null ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.IsChangeMode && //option not changed in changemode
                (cacheModel?.IpModelViewModel?.IpModelUsed.IsIpModelUsed == model.IsIpModelUsed))
            {
                // If subsequent journey already been populated then goto CheckAndSubmit
                if (IsIpModelJourneyAlreadyPopulated(cacheModel))
                    return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
            }

            if (cacheModel?.IpModelViewModel == null)   
                cacheModel.IpModelViewModel = new IpModelViewModel();

            cacheModel.IpModelViewModel.IpModelUsed = model;

            string redirectRouteName;

            if (model.IsIpModelUsed == false)
            {
                cacheModel.IpModelViewModel.IpMultiEmployerUsed = null;
                cacheModel.IpModelViewModel.IpMultiEmployerOther = null;
                cacheModel.IpModelViewModel.IpMultiEmployerSelect = null;
                redirectRouteName = model.IsChangeMode ? RouteConstants.IpCheckAndSubmit : RouteConstants.IpTempFlexibilityUsed;
            }
            else
            {
                redirectRouteName = RouteConstants.IpMultiEmployerUsed;
            }

            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(redirectRouteName);
        }

        [HttpGet]
        [Route("industry-placement-multiple-employer-model/{isChangeMode:bool?}", Name = RouteConstants.IpMultiEmployerUsed)]
        public async Task<IActionResult> IpMultiEmployerUsedAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == null || cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpMultiEmployerUsedViewModel>(cacheModel?.IpCompletion);
            viewModel.IsChangeMode = (isChangeMode || (cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-multiple-employer-model/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpMultiEmployerUsed)]
        public async Task<IActionResult> IpMultiEmployerUsedAsync(IpMultiEmployerUsedViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.IsChangeMode && //option not changed in changemode
                (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed.IsMultiEmployerModelUsed == model.IsMultiEmployerModelUsed))
            {
                // If subsequent journey already been populated then goto CheckAndSubmit
                if (IsMultiEmpModelJourneyAlreadyPopulated(cacheModel))
                    return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
            }

            cacheModel.IpModelViewModel.IpMultiEmployerUsed = model;

            string redirectRouteName;

            if (model.IsMultiEmployerModelUsed.Value)
            {
                cacheModel.IpModelViewModel.IpMultiEmployerSelect = null;
                redirectRouteName = RouteConstants.IpMultiEmployerOther;
            }
            else
            {
                cacheModel.IpModelViewModel.IpMultiEmployerOther = null;
                redirectRouteName = RouteConstants.IpMultiEmployerSelect;
            }

            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(redirectRouteName);
        }

        [HttpGet]
        [Route("industry-placement-other-models/{isChangeMode:bool?}", Name = RouteConstants.IpMultiEmployerOther)]
        public async Task<IActionResult> IpMultiEmployerOtherAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null || cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel.IpModelViewModel?.IpMultiEmployerOther) ?? await _industryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(IpLookupType.IndustryPlacementModel, cacheModel.IpCompletion.LearnerName, cacheModel.IpCompletion.PathwayId, true);
            viewModel.IsChangeMode = (isChangeMode || (cacheModel.IpModelViewModel?.IpMultiEmployerOther?.IsChangeMode ?? false)) && cacheModel.IsChangeModeAllowed == true;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-other-models/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpMultiEmployerOther)]
        public async Task<IActionResult> IpMultiEmployerOtherAsync(IpMultiEmployerOtherViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == true)
            {
                model.OtherIpPlacementModels.Where(ip => ip.Name.Equals(Constants.MultipleEmployer, StringComparison.InvariantCultureIgnoreCase)).ToList().ForEach(ip =>
                {
                    ip.IsSelected = true;
                });
            }

            cacheModel.IpModelViewModel.IpMultiEmployerOther = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if (cacheModel.IpModelViewModel?.IpModelUsed?.IsChangeMode == true ||
                cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsChangeMode == true ||
                cacheModel.IpModelViewModel?.IpMultiEmployerOther?.IsChangeMode == true)
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);

            return RedirectToRoute(RouteConstants.IpTempFlexibilityUsed);
        }

        [HttpGet]
        [Route("industry-placement-models/{isChangeMode:bool?}", Name = RouteConstants.IpMultiEmployerSelect)]
        public async Task<IActionResult> IpMultiEmployerSelectAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null || cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == true)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpMultiEmployerSelect) ?? await _industryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerSelectViewModel>(IpLookupType.IndustryPlacementModel, cacheModel.IpCompletion.LearnerName, cacheModel.IpCompletion.PathwayId, false);
            viewModel.IsChangeMode = (isChangeMode || (cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-models/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpMultiEmployerSelect)]
        public async Task<IActionResult> IpMultiEmployerSelectAsync(IpMultiEmployerSelectViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.IpModelViewModel.IpMultiEmployerSelect = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if (cacheModel.IpModelViewModel?.IpModelUsed?.IsChangeMode == true ||
                cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsChangeMode == true ||
                cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.IsChangeMode == true)
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);

            return RedirectToRoute(RouteConstants.IpTempFlexibilityUsed);
        }

        #endregion

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

            return RedirectToRoute((model.IsChangeMode || cacheModel.IpCompletion.IsChangeMode) ? RouteConstants.IpCheckAndSubmit : RouteConstants.IpModelUsed);
        }

        #endregion

        #region TemporaryFlexibility
        [HttpGet]
        [Route("industry-placement-temporary-flexibility/{isChangeMode:bool?}", Name = RouteConstants.IpTempFlexibilityUsed)]
        public async Task<IActionResult> IpTempFlexibilityUsedAsync(bool isChangeMode = false)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpModelUsed == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var navigation = await _industryPlacementLoader.GetTempFlexNavigationAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear);
            if (navigation == null)
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);

            if (cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == false)
            {
                cacheModel.IpModelViewModel.IpMultiEmployerUsed = null;
                cacheModel.IpModelViewModel.IpMultiEmployerOther = null;
                cacheModel.IpModelViewModel.IpMultiEmployerSelect = null;
                await _cacheService.SetAsync(CacheKey, cacheModel);
            }

           
            var viewModel = (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpTempFlexibilityUsedViewModel>(cacheModel?.IpCompletion);

            viewModel.IsChangeMode = (isChangeMode || (cacheModel.TempFlexibility?.IpTempFlexibilityUsed?.IsChangeMode ?? false)) && cacheModel?.IsChangeModeAllowed == true;
            viewModel.SetBackLink(cacheModel.IpModelViewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-temporary-flexibility/{isChangeMode:bool?}", Name = RouteConstants.SubmitIpTempFlexibilityUsed)]
        public async Task<IActionResult> IpTempFlexibilityUsedAsync(IpTempFlexibilityUsedViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpModelViewModel?.IpModelUsed == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
            {
                model.SetBackLink(cacheModel.IpModelViewModel);
                return View(model);
            }

            if (model.IsChangeMode)
            {
                // if selection doesn't change then redirect to Ip check and submit page
                if (model.IsTempFlexibilityUsed == cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed)
                {
                    // check based on selection to see if it is valid to redirect to check and submit page
                    if (model.IsTempFlexibilityUsed != true)
                        return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
                    else
                    {
                            return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
                    }
                }
            }

            if (cacheModel?.TempFlexibility == null)
                cacheModel.TempFlexibility = new IpTempFlexibilityViewModel();

            cacheModel.TempFlexibility.IpTempFlexibilityUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if (cacheModel.TempFlexibility.IpTempFlexibilityUsed.IsTempFlexibilityUsed == false)
            {
                cacheModel.TempFlexibility.IpBlendedPlacementUsed = null;
                cacheModel.TempFlexibility.IpEmployerLedUsed = null;
                cacheModel.TempFlexibility.IpGrantedTempFlexibility = null;
                await _cacheService.SetAsync(CacheKey, cacheModel);
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
            }

            return RedirectToRoute("TODO");
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

            //var navigation = await _industryPlacementLoader.GetTempFlexNavigationAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear); // TODO: Remove this. 

            // Item1 contain - Questions List & Item2 contain IsValid flag
            var ipDetailsList = _industryPlacementLoader.GetIpSummaryDetailsListAsync(cacheModel, null);
            if (!ipDetailsList.Item2 || ipDetailsList.Item1 == null || !ipDetailsList.Item1.Any())
                return RedirectToRoute(RouteConstants.PageNotFound);

            viewModel.IpDetailsList = ipDetailsList.Item1;

            viewModel.SetBackLink(cacheModel, null);

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

        private static bool IsIpModelJourneyAlreadyPopulated(IndustryPlacementViewModel cacheModel)
        {
            return cacheModel?.IpModelViewModel?.IpMultiEmployerUsed != null &&
                    ((cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == true && cacheModel.IpModelViewModel.IpMultiEmployerOther != null) ||
                     (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == false && cacheModel.IpModelViewModel.IpMultiEmployerSelect != null));
        }

        private static bool IsMultiEmpModelJourneyAlreadyPopulated(IndustryPlacementViewModel cacheModel)
        {
            return (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == true && cacheModel.IpModelViewModel.IpMultiEmployerOther != null) ||
                     (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == false && cacheModel.IpModelViewModel.IpMultiEmployerSelect != null);
        }
    }
}
