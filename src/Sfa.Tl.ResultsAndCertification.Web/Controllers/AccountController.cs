using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        public IActionResult PostSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(TlevelHomeController.Index), Constants.TlevelHomeController);
            }
            else
            {
                return RedirectToAction("FailedLogin", "Home");
            }
        }

        [HttpGet]
        public async Task SignedOut()
        {
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public IActionResult SignOutComplete()
        {
            return RedirectToAction(nameof(HomeController.Index), Constants.HomeController);
        }
    }
}
