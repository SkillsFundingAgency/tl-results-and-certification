using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminAssessmentSeriesDatesController : Controller
    {
        private readonly IAdminAssessmentSeriesDatesLoader _loader;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AdminAssessmentSeriesDatesController> _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminAssessmentSeriesDatesCacheKey);

        public AdminAssessmentSeriesDatesController(
            IAdminAssessmentSeriesDatesLoader loader,
            ICacheService cacheService,
            ILogger<AdminAssessmentSeriesDatesController> logger)
        {
            _loader = loader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/assessment-series-dates-clear", Name = RouteConstants.AdminAssessmentSeriesDatesClear)]
        public async Task<IActionResult> AdminAssessmentSeriesDatesClearAsync()
        {
            await _cacheService.RemoveAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminAssessmentSeriesDates);
        }

        [HttpGet]
        [Route("admin/assessment-series-dates", Name = RouteConstants.AdminAssessmentSeriesDates)]
        public async Task<IActionResult> AdminAssessmentSeriesDatesAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                AdminAssessmentSeriesDatesCriteriaViewModel criteria = new();
                criteria = _loader.LoadFilters();

                //viewModel = await _loader.SearchNotificationAsync(criteria);

                await _cacheService.SetAsync(CacheKey, viewModel);
                return View(viewModel);
            }

            //AdminAssessmentSeriesDatesViewModel viewModel = new();

            viewModel.SearchCriteria = _loader.LoadFilters();
            viewModel.Series = await _loader.GetAssessmentSeriesDatesAsync();

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/find-assessment-series-dates-apply-filters", Name = RouteConstants.SubmitAdminAssessmentSeriesDatesApplyFilters)]
        public async Task<IActionResult> AdminAssessmentSeriesDatesApplyFiltersAsync(AdminAssessmentSeriesDatesCriteriaViewModel searchCriteriaViewModel)
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminAssessmentSeriesDatesViewModel cache data found. Method: {RouteConstants.SubmitAdminAssessmentSeriesDatesApplyFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = searchCriteriaViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminAssessmentSeriesDates, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpPost]
        [Route("admin/find-assessment-series-dates-clear-filters", Name = RouteConstants.SubmitAdminAssessmentSeriesDatesClearFilters)]
        public async Task<IActionResult> AdminAssessmentSeriesDatesClearFiltersAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminAssessmentSeriesDatesViewModel cache data found. Method: {RouteConstants.SubmitAdminAssessmentSeriesDatesClearFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = _loader.LoadFilters();

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminAssessmentSeriesDates, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpGet]
        [Route("admin/assessment-series-date-details/{assessmentId}", Name = RouteConstants.AdminAssessmentSeriesDateDetails)]
        public async Task<IActionResult> AdminAssessmentSeriesDatesDetailsAsync(int assessmentId)
        {
            AdminAssessmentSeriesDateDetailsViewModel viewModel = await _loader.GetAssessmentSeriesDatesDetailsViewModel(assessmentId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }
    }
}