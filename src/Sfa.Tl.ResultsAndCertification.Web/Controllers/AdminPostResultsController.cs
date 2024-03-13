using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireAdminDashboardAccess)]
    public class AdminPostResultsController : Controller
    {
        private readonly IAdminPostResultsLoader _loader;
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        private string CacheKey
            => CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.AdminPostResultsCacheKey);

        public AdminPostResultsController(
            IAdminPostResultsLoader loader,
            ICacheService cacheService,
            ILogger<AdminChangeLogController> logger)
        {
            _loader = loader;
            _cacheService = cacheService;
            _logger = logger;
        }

        [HttpGet]
        [Route("admin/open-romm-core-clear", Name = RouteConstants.AdminOpenPathwayRommClear)]
        public async Task<IActionResult> AdminOpenPathwayRommClearAsync(int registrationPathwayId, int assessmentId)
        {
            await _cacheService.RemoveAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            return RedirectToRoute(RouteConstants.AdminOpenPathwayRomm, new { registrationPathwayId, assessmentId });
        }

        [HttpGet]
        [Route("admin/open-romm-core", Name = RouteConstants.AdminOpenPathwayRomm)]
        public async Task<IActionResult> AdminOpenPathwayRommAsync(int registrationPathwayId, int assessmentId)
        {
            var cachedModel = await _cacheService.GetAsync<AdminOpenPathwayRommViewModel>(CacheKey);
            if (cachedModel != null)
            {
                return View(cachedModel);
            }

            AdminOpenPathwayRommViewModel viewModel = await _loader.GetAdminOpenRommAsync(registrationPathwayId, assessmentId);
            if (viewModel == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No core result details found. Method: AdminOpenRommAsync({registrationPathwayId}, {assessmentId}), User: {User.GetUserEmail()}");
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return View(viewModel);
        }
    }
}