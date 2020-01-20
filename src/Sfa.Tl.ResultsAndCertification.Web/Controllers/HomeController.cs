using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{    
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(AccountController.PostSignIn), Constants.AccountController);
            }
            return View();
        }               
    }
}
