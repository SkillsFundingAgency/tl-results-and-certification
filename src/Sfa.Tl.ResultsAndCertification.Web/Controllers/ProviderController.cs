using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireProviderEditorAccess)]
    public class ProviderController : Controller
    {
        private readonly IProviderLoader _providerLoader;
        private readonly ILogger _logger;

        public ProviderController(IProviderLoader providerLoader, ILogger<AccountController> logger)
        {
            _providerLoader = providerLoader;
            _logger = logger;
        }

        [Route("providers", Name = RouteConstants.Providers)]
        public async Task<IActionResult> IndexAsync()
        {
            var providers = await _providerLoader.GetAllProvidersByUkprnAsync(User.GetUkPrn());

            if (providers?.Count() > 0)
            {
                return RedirectToRoute(RouteConstants.YourProviders); // TODO: redirect to AddProvider.
            }

            return RedirectToRoute(RouteConstants.FindProvider);
        }

        [Route("your-providers", Name = RouteConstants.YourProviders)]
        public async Task<IActionResult> ViewAllAsync()
        {
            return await Task.Run(() => View());
        }

        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProviderAsync(string provider)
        {
            // TODO: get by Ukprn or string
            var result = await Task.Run(() => true);
            return RedirectToRoute(RouteConstants.YourProviders);
        }

        public async Task<JsonResult> GetProviders(string term)
        {
            // TODO: IsExact Match or Contains Match param as well?
            var providers = await _providerLoader.SearchByTokenAsync(term);

            List<string> students = new List<string>
            {
                "test", "Acobat", "bobbili", "Chandra"
            };

            return Json(students.Where(x => x.Contains(term)));
        }
    }
}