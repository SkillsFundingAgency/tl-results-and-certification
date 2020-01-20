using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{    
    public class AccountController : Controller
    {
        private readonly ILogger _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task SignIn()
        {
            var returnUrl = Url.Action(nameof(AccountController.PostSignIn), Constants.AccountController);
            await HttpContext.ChallengeAsync(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet]
        public IActionResult PostSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return !HttpContext.User.HasAccessToService()
                    ? RedirectToAction(nameof(ErrorController.ServiceAccessDenied), Constants.ErrorController)
                    : RedirectToAction(nameof(DashboardController.Index), Constants.DashboardController);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), Constants.HomeController);
            }
        }
        
        [HttpGet]
        public async Task SignedOut()
        {
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignOutComplete()
        {
            return RedirectToAction(nameof(HomeController.Index), Constants.HomeController);
        }
    }
}
