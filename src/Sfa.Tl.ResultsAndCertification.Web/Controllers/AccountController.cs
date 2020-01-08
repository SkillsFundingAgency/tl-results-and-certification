using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            var returnUrl = Url.Action("PostSignIn", "Account");
            await HttpContext.ChallengeAsync(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        public IActionResult PostSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "TlevelHome");
            }
            else
            {
                return RedirectToAction("FailedLogin", "Home");
            }
        }
    }
}
