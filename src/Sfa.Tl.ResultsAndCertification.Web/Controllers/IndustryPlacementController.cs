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
using System;
using System.Linq;
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
        [Route("add-industry-placement/{profileId}", Name = RouteConstants.AddIndustryPlacement)]
        public async Task<IActionResult> AddIndustryPlacementAsync(int profileId)
        {
            await _cacheService.RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.IpCompletion, new { profileId });
        }

        [HttpGet]
        [Route("industry-placement-completion/{profileId}", Name = RouteConstants.IpCompletion)]
        public async Task<IActionResult> IpCompletionAsync(int profileId)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            var viewModel = cacheModel?.IpCompletion;
            if (viewModel == null)
            {
                viewModel = await _industryPlacementLoader.GetLearnerRecordDetailsAsync<IpCompletionViewModel>(User.GetUkPrn(), profileId);
                if (viewModel == null || !viewModel.IsValid)
                    return RedirectToRoute(RouteConstants.PageNotFound);
            }

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

            if (model.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.IpSpecialConsiderationHours);

            return RedirectToRoute(RouteConstants.LearnerRecordDetails, new { profileId = model.ProfileId });
        }

        #region Ip Models

        [HttpGet]
        [Route("industry-placement-model-used", Name = RouteConstants.IpModelUsed)]
        public async Task<IActionResult> IpModelUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == null ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }
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
            if (cacheModel == null ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted ||
                cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (cacheModel?.IpModelViewModel == null)
                cacheModel.IpModelViewModel = new IpModelViewModel();

            cacheModel.IpModelViewModel.IpModelUsed = model;

            string redirectRouteName;

            if (model.IsIpModelUsed == false)
            {
                cacheModel.IpModelViewModel.IpMultiEmployerUsed = null;
                cacheModel.IpModelViewModel.IpMultiEmployerOther = null;
                cacheModel.IpModelViewModel.IpMultiEmployerSelect = null;
                redirectRouteName = RouteConstants.IpTempFlexibilityUsed;
            }
            else
            {
                redirectRouteName = RouteConstants.IpMultiEmployerUsed;
            }

            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(redirectRouteName);
        }

        [HttpGet]
        [Route("industry-placement-multiple-employer-model", Name = RouteConstants.IpMultiEmployerUsed)]
        public async Task<IActionResult> IpMultiEmployerUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == null || cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpMultiEmployerUsedViewModel>(cacheModel?.IpCompletion);

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
        [Route("industry-placement-other-models", Name = RouteConstants.IpMultiEmployerOther)]
        public async Task<IActionResult> IpMultiEmployerOtherAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null || cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpMultiEmployerOther) ?? await _industryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(IpLookupType.IndustryPlacementModel, cacheModel.IpCompletion.LearnerName, cacheModel.IpCompletion.PathwayId, true);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-other-models", Name = RouteConstants.SubmitIpMultiEmployerOther)]
        public async Task<IActionResult> IpMultiEmployerOtherAsync(IpMultiEmployerOtherViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.IpModelViewModel.IpMultiEmployerOther = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpTempFlexibilityUsed);
        }

        [HttpGet]
        [Route("industry-placement-models", Name = RouteConstants.IpMultiEmployerSelect)]
        public async Task<IActionResult> IpMultiEmployerSelectAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null || cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed == true)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.IpModelViewModel?.IpMultiEmployerSelect) ?? await _industryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerSelectViewModel>(IpLookupType.IndustryPlacementModel, cacheModel.IpCompletion.LearnerName, cacheModel.IpCompletion.PathwayId, false);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-models", Name = RouteConstants.SubmitIpMultiEmployerSelect)]
        public async Task<IActionResult> IpMultiEmployerSelectAsync(IpMultiEmployerSelectViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.IpModelViewModel.IpMultiEmployerSelect = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpTempFlexibilityUsed);
        }

        #endregion

        #region SpecialConsideration

        [HttpGet]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.IpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.SpecialConsideration?.Hours) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<SpecialConsiderationHoursViewModel>(cacheModel.IpCompletion);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-hours", Name = RouteConstants.SubmitIpSpecialConsiderationHours)]
        public async Task<IActionResult> IpSpecialConsiderationHoursAsync(SpecialConsiderationHoursViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
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
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.SpecialConsideration?.Reasons) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<SpecialConsiderationReasonsViewModel>(cacheModel.IpCompletion);
            viewModel.ReasonsList = await _industryPlacementLoader.GetSpecialConsiderationReasonsListAsync(viewModel.AcademicYear);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-special-consideration-reasons", Name = RouteConstants.SubmitIpSpecialConsiderationReasons)]
        public async Task<IActionResult> IpSpecialConsiderationReasonsAsync(SpecialConsiderationReasonsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel == null || cacheModel.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration || cacheModel.SpecialConsideration?.Hours == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            cacheModel.SpecialConsideration.Reasons = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return RedirectToRoute(RouteConstants.IpModelUsed);
        }

        #endregion

        #region TemporaryFlexibility
        [HttpGet]
        [Route("industry-placement-temporary-flexibility", Name = RouteConstants.IpTempFlexibilityUsed)]
        public async Task<IActionResult> IpTempFlexibilityUsedAsync()
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

            if (!navigation.AskTempFlexibility && navigation.AskBlendedPlacement)
                return RedirectToRoute(RouteConstants.IpBlendedPlacementUsed);

            var viewModel = (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpTempFlexibilityUsedViewModel>(cacheModel?.IpCompletion);

            viewModel.SetBackLink(cacheModel.IpModelViewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-temporary-flexibility", Name = RouteConstants.SubmitIpTempFlexibilityUsed)]
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

            if (cacheModel?.TempFlexibility == null)
                cacheModel.TempFlexibility = new IpTempFlexibilityViewModel();

            cacheModel.TempFlexibility.IpTempFlexibilityUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            if (cacheModel.TempFlexibility.IpTempFlexibilityUsed.IsTempFlexibilityUsed == false)
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);

            return RedirectToRoute(RouteConstants.IpBlendedPlacementUsed);
        }

        [HttpGet]
        [Route("industry-placement-temporary-flexibility-blended", Name = RouteConstants.IpBlendedPlacementUsed)]
        public async Task<IActionResult> IpBlendedPlacementUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.IpModelViewModel?.IpModelUsed == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var navigation = await _industryPlacementLoader.GetTempFlexNavigationAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear);
            if (navigation == null)
                return RedirectToRoute(RouteConstants.IpCheckAndSubmit);

            if (!navigation.AskBlendedPlacement)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var viewModel = (cacheModel?.TempFlexibility?.IpBlendedPlacementUsed) ?? await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpBlendedPlacementUsedViewModel>(cacheModel?.IpCompletion);
            viewModel.SetBackLink(cacheModel.IpModelViewModel, navigation.AskTempFlexibility);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-temporary-flexibility-blended", Name = RouteConstants.SubmitIpBlendedPlacementUsed)]
        public async Task<IActionResult> IpBlendedPlacementUsedAsync(IpBlendedPlacementUsedViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpModelViewModel?.IpModelUsed == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (!ModelState.IsValid)
            {
                var navigation = await _industryPlacementLoader.GetTempFlexNavigationAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear);
                model.SetBackLink(cacheModel.IpModelViewModel, navigation.AskTempFlexibility);
                return View(model);
            }

            if (cacheModel?.TempFlexibility == null)
                cacheModel.TempFlexibility = new IpTempFlexibilityViewModel();

            cacheModel.TempFlexibility.IpBlendedPlacementUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);
            return RedirectToRoute(RouteConstants.IpEmployerLedUsed);
        }

        [HttpGet]
        [Route("industry-placement-temporary-flexibility-employer-led", Name = RouteConstants.IpEmployerLedUsed)]
        public async Task<IActionResult> IpEmployerLedUsedAsync()
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == null
                || cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == false
                || cacheModel?.TempFlexibility?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed == null
                || cacheModel?.TempFlexibility?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed.Value == false)
                return RedirectToRoute(RouteConstants.PageNotFound);

            IpEmployerLedUsedViewModel viewModel;

            if (cacheModel?.TempFlexibility?.IpEmployerLedUsed == null)
            {
                viewModel = await _industryPlacementLoader.TransformIpCompletionDetailsTo<IpEmployerLedUsedViewModel>(cacheModel.IpCompletion);

                var tempFlexibilities = await _industryPlacementLoader.GetTemporaryFlexibilitiesAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear, true);

                viewModel.TemporaryFlexibilities = tempFlexibilities?.Where(t => t.Name.Equals(Constants.EmployerLedActivities, StringComparison.InvariantCultureIgnoreCase) || t.Name.Equals(Constants.BlendedPlacements, StringComparison.InvariantCultureIgnoreCase))?.ToList();

                if (viewModel.TemporaryFlexibilities == null || viewModel.TemporaryFlexibilities.Count == 0)
                    return RedirectToRoute(RouteConstants.PageNotFound);
            }
            else
            {
                viewModel = cacheModel?.TempFlexibility?.IpEmployerLedUsed;
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-temporary-flexibility-employer-led", Name = RouteConstants.SubmitIpEmployerLedUsed)]
        public async Task<IActionResult> IpEmployerLedUsedAsync(IpEmployerLedUsedViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);

            if (cacheModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            var tempFlexibilities = await _industryPlacementLoader.GetTemporaryFlexibilitiesAsync(cacheModel.IpCompletion.PathwayId, cacheModel.IpCompletion.AcademicYear, true);

            model.TemporaryFlexibilities = tempFlexibilities?.Where(t => t.Name.Equals(Constants.EmployerLedActivities, StringComparison.InvariantCultureIgnoreCase) || t.Name.Equals(Constants.BlendedPlacements, StringComparison.InvariantCultureIgnoreCase))?.ToList();

            if (model.TemporaryFlexibilities == null || !model.TemporaryFlexibilities.Any())
                return RedirectToRoute(RouteConstants.PageNotFound);

            if (model.IsEmployerLedUsed.Value)
            {
                model.TemporaryFlexibilities.ToList().ForEach(tf => tf.IsSelected = true);
            }
            else
            {
                model.TemporaryFlexibilities.ToList().ForEach(tf =>
                {
                    tf.IsSelected = tf.Name.Equals(Constants.BlendedPlacements, StringComparison.InvariantCultureIgnoreCase);
                });
            }

            cacheModel.TempFlexibility.IpEmployerLedUsed = model;
            await _cacheService.SetAsync(CacheKey, cacheModel);

            return View(model);
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

            var viewModel = await _industryPlacementLoader.GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(User.GetUkPrn(), cacheModel.IpCompletion.PathwayId);
            if (viewModel == null)
                return RedirectToRoute(RouteConstants.PageNotFound);

            return View(viewModel);
        }

        [HttpPost]
        [Route("industry-placement-check-your-answers", Name = RouteConstants.SubmitIpCheckAndSubmit)]
        public IActionResult IpCheckAndSubmitSave()
        {
            return RedirectToRoute(RouteConstants.IpCheckAndSubmit);
        }

        #endregion

        private async Task SyncCacheIp(IpCompletionViewModel model)
        {
            var cacheModel = await _cacheService.GetAsync<IndustryPlacementViewModel>(CacheKey);
            if (cacheModel?.IpCompletion != null)
                cacheModel.IpCompletion = model;
            else
                cacheModel = new IndustryPlacementViewModel
                {
                    IpCompletion = model
                };

            await _cacheService.SetAsync(CacheKey, cacheModel);
        }
    }
}
