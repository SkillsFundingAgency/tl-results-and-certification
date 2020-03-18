using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireProviderEditorAccess)]
    public class ProviderController : Controller
    {
        private readonly IProviderLoader _providerLoader;

        public ProviderController(IProviderLoader providerLoader)
        {
            _providerLoader = providerLoader;
        }

        [Route("providers", Name = RouteConstants.Providers)]
        public async Task<IActionResult> IndexAsync()
        {
            return RedirectToRoute(RouteConstants.FindProvider);

            // TODO: 
            var providers  = await _providerLoader.GetAllProvidersByUkprnAsync(User.GetUkPrn());

            if (providers?.Count() > 0)
            {
                return RedirectToRoute(RouteConstants.YourProviders);
            }

            return RedirectToRoute(RouteConstants.FindProvider);
        }

        [Route("your-providers", Name = RouteConstants.YourProviders)]
        public async Task<IActionResult> ViewAllAsync()
        {
            return await Task.Run(() => View());
        }

        [Route("find-provider", Name = RouteConstants.FindProvider)]
        public async Task<IActionResult> FindProvider()
        {
            return await Task.Run(() => View());
        }
    }
}