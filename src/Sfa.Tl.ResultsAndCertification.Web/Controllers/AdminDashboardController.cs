using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardLoader _loader;

        public AdminDashboardController(IAdminDashboardLoader loader)
        {
            _loader = loader;
        }

        [HttpGet]
        [Route("admin/search-learner-records", Name = RouteConstants.SearchLearners)]
        public async Task<IActionResult> AdminSearchLearnersAsync()
        {
            AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();

            var viewModel = new AdminSearchLearnerViewModel
            {
                SearchCriteria = new AdminSearchLearnerCriteriaViewModel
                {
                    SearchLearnerFilters = filters
                }
            };

            return View(viewModel);
        }
    }
}