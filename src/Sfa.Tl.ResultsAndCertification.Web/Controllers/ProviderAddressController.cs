using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [Authorize(Policy = RolesExtensions.RequireLearnerRecordsEditorAccess)]
    public class ProviderAddressController : Controller
    {
        [HttpGet]
        [Route("manage-postal-address", Name = RouteConstants.ManagePostalAddress)]
        public async Task<IActionResult> ManagePostalAddressAsync()
        {
            var isAlreadyAdded = await FindPostalAddress();
            if (isAlreadyAdded)
                return View(); //TODO: redirect to ShowAddressPage.

            return View(new ManagePostalAddressViewModel());

            static async Task<bool> FindPostalAddress()
            {
                await Task.CompletedTask;
                return false;
            }
        }
    }
}
