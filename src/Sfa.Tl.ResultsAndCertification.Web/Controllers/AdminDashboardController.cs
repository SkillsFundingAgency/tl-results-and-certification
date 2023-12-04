using Lrs.LearnerService.Api.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminDashboardLoader _loader;

        public AdminDashboardController(IAdminDashboardLoader loader)
        {
            _loader = loader;
        }

        [HttpGet]
        [Route("admin/search-learner-records", Name = RouteConstants.AdminSearchLearners)]
        public async Task<IActionResult> AdminSearchLearnersAsync()
        {
            AdminSearchLearnerFiltersViewModel filters = await _loader.GetAdminSearchLearnerFiltersAsync();
            
            var viewModel = new AdminSearchLearnerViewModel(filters);
            return View(viewModel);
        }

        [HttpGet]
        [Route("admin/change-start-year/{profileId}", Name = RouteConstants.AdminChangeStartYear)]
        public async Task<IActionResult> AdminChangeStartYearAsync(int profileId)
        {
            var viewModel = await _loader.GetAdminLearnerDetailsAsync(profileId);
            return View(viewModel);
        }
    }
}