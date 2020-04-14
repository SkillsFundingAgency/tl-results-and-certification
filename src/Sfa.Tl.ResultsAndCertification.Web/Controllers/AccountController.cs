using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{    
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public AccountController(ResultsAndCertificationConfiguration configuration, ILogger<AccountController> logger)
        {
            _configuration = configuration;
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

        [HttpGet]
        [Route("account-profile", Name=RouteConstants.AccountProfile)]
        public IActionResult Profile()
        {
            if (_configuration == null || 
                _configuration.DfeSignInSettings == null || 
                string.IsNullOrEmpty(_configuration.DfeSignInSettings.ProfileUrl))
            {
                return RedirectToRoute(RouteConstants.PageNotFound);
            }

            return Redirect(_configuration.DfeSignInSettings.ProfileUrl);
        }
    }
}
