using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Help;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class HelpController : Controller
    {
        private readonly ResultsAndCertificationConfiguration _configuration;

        public HelpController(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("cookie-policy", Name = RouteConstants.CookiePolicy)]
        public IActionResult Cookies()
        {
            return View();
        }

        [HttpGet]
        [Route("privacy-policy", Name = RouteConstants.PrivacyPolicy)]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("terms-and-conditions", Name = RouteConstants.TermsAndConditions)]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpGet]
        [Route("user-guide", Name = RouteConstants.UserGuide)]
        public IActionResult UserGuide()
        {
            var viewModel = new UserGuideViewModel { TechnicalSupportEmailAddress = _configuration.TechnicalSupportEmailAddress };
            return View(viewModel);
        }
    }
}