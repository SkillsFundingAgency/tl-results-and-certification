using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
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
        [Route("admin/search-assessment-series-dates-clear", Name = RouteConstants.SearchAssessmentSeriesDatesClear)]
        public async Task<IActionResult> SearchAssessmentSeriesDatesClearAsync()
        {
            await _cacheService.RemoveAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.SearchAssessmentSeriesDates);
        }

        [HttpGet]
        [Route("admin/search-assessment-series-dates", Name = RouteConstants.SearchAssessmentSeriesDates)]
        public async Task<IActionResult> SearchAssessmentSeriesDatesAsync(int? pageNumber = default)
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                AdminAssessmentSeriesDatesCriteriaViewModel criteria;

                criteria = _loader.LoadFilters();

                viewModel = new AdminAssessmentSeriesDatesViewModel();

                viewModel.SearchCriteria = criteria;
                viewModel = await _loader.SearchAssessmentSeriesDatesAsync(criteria);

                await _cacheService.SetAsync(CacheKey, viewModel);
                return View(viewModel);
            }

            AdminAssessmentSeriesDatesCriteriaViewModel searchCriteria = viewModel.SearchCriteria;
            searchCriteria.PageNumber = pageNumber;

            viewModel = await _loader.SearchAssessmentSeriesDatesAsync(searchCriteria);

            return View(viewModel);
        }

        [HttpPost]
        [Route("admin/search-assessment-series-dates-apply-filters", Name = RouteConstants.SubmitSearchAssessmentSeriesDatesApplyFilters)]
        public async Task<IActionResult> SearchAssessmentSeriesDatesApplyFiltersAsync(AdminAssessmentSeriesDatesCriteriaViewModel searchCriteriaViewModel)
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminAssessmentSeriesDatesViewModel cache data found. Method: {RouteConstants.SubmitSearchAssessmentSeriesDatesApplyFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = searchCriteriaViewModel;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchAssessmentSeriesDates, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpPost]
        [Route("admin/search-assessment-series-dates-clear-filters", Name = RouteConstants.SubmitSearchAssessmentSeriesDatesClearFilters)]
        public async Task<IActionResult> SearchAssessmentSeriesDatesClearFiltersAsync()
        {
            var viewModel = await _cacheService.GetAsync<AdminAssessmentSeriesDatesViewModel>(CacheKey);

            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No AdminAssessmentSeriesDatesViewModel cache data found. Method: {RouteConstants.SubmitSearchAssessmentSeriesDatesClearFilters}, User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            viewModel.SearchCriteria = _loader.LoadFilters();

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchAssessmentSeriesDates, new { pageNumber = viewModel.SearchCriteria.PageNumber });
        }

        [HttpGet]
        [Route("admin/assessment-series-date/{assessmentId}", Name = RouteConstants.AdminAssessmentSeriesDateDetails)]
        public async Task<IActionResult> AdminAssessmentSeriesDateAsync(int assessmentId)
        {
            AdminAssessmentSeriesDetailsViewModel viewModel = await _loader.GetAssessmentSeriesDateViewModel(assessmentId);

            if (viewModel == null)
            {
                return RedirectToRoute(RouteConstants.ProblemWithService);
            }

            return View(viewModel);
        }
    }
}