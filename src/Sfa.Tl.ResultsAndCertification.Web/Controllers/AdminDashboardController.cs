using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Linq;
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
        [Route("admin/search-learner-records/{pageNumber:int?}", Name = RouteConstants.AdminSearchLearnersRecords)]
        public async Task<IActionResult> AdminSearchLearnersAsync(int? pageNumber = default)
        {
            AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();

            if (filters == null)
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            var searchCriteria = await _cacheService.GetAsync<AdminSearchLearnerCriteriaViewModel>(CacheKey);

            if (searchCriteria == null)
            {
                return View(new AdminSearchLearnerViewModel(filters));
            }

            searchCriteria.PageNumber = pageNumber;

            if (searchCriteria.SearchLearnerFilters != null)
            {
                searchCriteria.SearchLearnerFilters.AwardingOrganisations?.ToList().ForEach(tl => tl.Name = filters.AwardingOrganisations.FirstOrDefault(x => x.Id == tl.Id)?.Name);
                searchCriteria.SearchLearnerFilters.AcademicYears?.ToList().ForEach(s => s.Name = filters.AcademicYears.FirstOrDefault(x => x.Id == s.Id)?.Name);
            }
            else
            {
                searchCriteria.SearchLearnerFilters = filters;
            }

            AdminSearchLearnerDetailsListViewModel learnerDetailsListViewModel = await _loader.GetAdminSearchLearnerDetailsListAsync(searchCriteria);
            return View(new AdminSearchLearnerViewModel(searchCriteria, learnerDetailsListViewModel));

        }

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
        }
    }
}