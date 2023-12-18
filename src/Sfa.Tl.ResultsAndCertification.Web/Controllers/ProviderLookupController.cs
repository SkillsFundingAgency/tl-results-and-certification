using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireProviderEditorOrAdminDashboardAccess)]
    public class ProviderLookupController : Controller
    {
        private readonly IProviderLoader _providerLoader;

        public ProviderLookupController(IProviderLoader providerLoader)
        {
            _providerLoader = providerLoader;
        }

        [HttpGet]
        [Route("search-provider/{name}", Name = RouteConstants.ProviderNameLookup)]
        public async Task<JsonResult> GetProviderLookupDataAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Json(string.Empty);

            var providersData = await _providerLoader.GetProviderLookupDataAsync(name, isExactMatch: false);
            return Json(providersData);
        }
    }
}