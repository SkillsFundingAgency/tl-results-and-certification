using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardLoader _loader;
        private readonly ICacheService _cacheService;

        private string CacheKey => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminDashboardCacheKey);

        public AdminDashboardController(IAdminDashboardLoader loader, ICacheService cacheService)
        {
            _loader = loader;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("admin/search-learner-records/{pageNumber}", Name = RouteConstants.AdminSearchLearnersRecords)]
        public async Task<IActionResult> AdminSearchLearnersAsync(int pageNumber = default)
        {
            AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();

            if (filters == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var searchCriteria = await _cacheService.GetAsync<AdminSearchLearnerCriteriaViewModel>(CacheKey);

            if (searchCriteria == null)
            {
                var viewModel = new AdminSearchLearnerViewModel(filters);
                return View(viewModel);
            }
            else
            {
                AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel = await _loader.GetAdminSearchLearnerDetailsListAsync(searchCriteria);

                var viewModel = new AdminSearchLearnerViewModel(filters)
                {
                    SearchLearnerDetailsList = learnerDetailsListViewModel
                };

                return View(viewModel);
            }
        }

        //[HttpGet]
        //[Route("admin/search-learner-records/{pageNumber}", Name = RouteConstants.AdminSearchLearnersRecords)]
        //public async Task<IActionResult> AdminSearchLearnersAsync(int pageNumber)
        //{
        //    AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();

        //    if (filters == null)
        //    {
        //        return RedirectToRoute(RouteConstants.PageNotFound);
        //    }

        //    var searchCriteria = await _cacheService.GetAsync<AdminSearchLearnerCriteriaViewModel>(CacheKey);

        //    if (searchCriteria == null)
        //    {
        //        searchCriteria = new AdminSearchLearnerCriteriaViewModel { PageNumber = pageNumber };
        //    }
        //    else
        //    {
        //        searchCriteria.PageNumber = pageNumber;

        //        AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel = await _loader.GetAdminSearchLearnerDetailsListAsync(searchCriteria);

        //        var viewModel = new AdminSearchLearnerViewModel(filters)
        //        {
        //            SearchLearnerDetailsList = learnerDetailsListViewModel
        //        };

        //        return View(viewModel);

        //        //if (searchCriteria.SearchLearnerFilters != null)
        //        //{
        //        //    searchCriteria.SearchLearnerFilters.Tlevels?.ToList().ForEach(tl => tl.Name = searchFilters.Tlevels.FirstOrDefault(x => x.Id == tl.Id)?.Name);
        //        //    searchCriteria.SearchLearnerFilters.Status?.ToList().ForEach(s => s.Name = searchFilters.Status.FirstOrDefault(x => x.Id == s.Id)?.Name);
        //        //}
        //    }

            
        //}

        [HttpPost]
        [Route("admin/search-learner-records", Name = RouteConstants.SubmitAdminSearchLearnersRecords)]
        public async Task<IActionResult> SubmitAdminSearchLearnerApplyFiltersAsync(AdminSearchLearnerCriteriaViewModel viewModel)
        {
            var searchCriteria = await _cacheService.GetAsync<AdminSearchLearnerCriteriaViewModel>(CacheKey);

            // populate if any filter are applied from cache
            if (searchCriteria != null)
            {
                viewModel.SearchLearnerFilters = searchCriteria.SearchLearnerFilters;
            }

            viewModel.IsSearchKeyApplied = true;

            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.AdminSearchLearnersRecords, new { pageNumber = viewModel.PageNumber });

            //var request = new AdminSearchLearnerRequest
            //{
            //    SearchKey = viewModel.SearchKey,
            //    PageNumber = viewModel.PageNumber,
            //    SelectedAcademicYears = viewModel.SearchLearnerFilters.AcademicYears.Where(p => p.IsSelected).Select(p => p.Id).ToList(),
            //    SelectedAwardingOrganisations = viewModel.SearchLearnerFilters.AwardingOrganisations.Where(p => p.IsSelected).Select(p => p.Id).ToList()
            //};

            //AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel = await _loader.GetAdminSearchLearnerDetailsListAsync(request);

            //return View(viewModel);
        }

        /*
         * [HttpPost]
        [Route("manage-learners/{academicYear}", Name = RouteConstants.SubmitSearchLearnerDetails)]
        public async Task<IActionResult> SearchLearnerDetailsAsync(SearchCriteriaViewModel viewModel)
        {
            var searchCriteria = await _cacheService.GetAsync<SearchCriteriaViewModel>(CacheKey);

            // populate if any filter are applied from cache
            if (searchCriteria != null)
                viewModel.SearchLearnerFilters = searchCriteria.SearchLearnerFilters;

            viewModel.IsSearchKeyApplied = true;
            await _cacheService.SetAsync(CacheKey, viewModel);
            return RedirectToRoute(RouteConstants.SearchLearnerDetails, new { academicYear = viewModel.AcademicYear });
        }
         */
    }
}