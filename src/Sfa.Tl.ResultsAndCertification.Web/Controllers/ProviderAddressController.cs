using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
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

        [HttpGet]
        [Route("add-postal-address-postcode", Name = RouteConstants.AddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync()
        {
            await Task.CompletedTask;
            return View(new AddAddressPostcodeViewModel());
        }

        [HttpPost]
        [Route("add-postal-address-postcode", Name = RouteConstants.SubmitAddAddressPostcode)]
        public async Task<IActionResult> AddAddressPostcodeAsync(AddAddressPostcodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await Task.CompletedTask;
            return View(model);
        }

        [HttpGet]
        [Route("add-postal-address-manual", Name = RouteConstants.AddPostalAddressManul)]
        public async Task<IActionResult> AddPostalAddressManulAsync()
        {
            await Task.CompletedTask;
            return View(new AddPostalAddressManualViewModel());
        }

        [HttpPost]
        [Route("add-postal-address-manual", Name = RouteConstants.SubmitAddPostalAddressManul)]
        public async Task<IActionResult> AddPostalAddressManulAsync(AddPostalAddressManualViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await Task.CompletedTask;
            return View(model);
        }
    }
}
