using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Help;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    [AllowAnonymous]
    public class HelpController : Controller
    {
        protected readonly IHelpLoader _helpLoader;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public HelpController(ResultsAndCertificationConfiguration configuration, IHelpLoader helpLoader)
        {
            _configuration = configuration;
            _helpLoader = helpLoader;
        }

        [HttpGet]
        [Route("cookie-details", Name = RouteConstants.CookieDetails)]
        public IActionResult CookieDetails()
        {
            return View();
        }

        [HttpGet]
        [Route("cookies", Name = RouteConstants.Cookies)]
        public IActionResult Cookies()
        {
            return View();
        }

        [HttpGet]
        [Route("accessibility-statement", Name = RouteConstants.AccessibilityStatement)]
        public IActionResult AccessibilityStatement()
        {
            return View();
        }


        [HttpGet]
        [Route("contact-us", Name = RouteConstants.Contact)]
        public IActionResult Contact()
        {
            var viewmodel = new ContactViewModel();
            return View(viewmodel);
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

        [HttpGet]
        [Route("service-unavailable", Name = RouteConstants.ServiceUnavailable)]
        public IActionResult ServiceUnavailable()
        {
            ServiceUnavailableViewModel viewModel = new();
            var userType = _helpLoader.GetLoginUserType(User);

            DateTime freezePeriodEndDateUtc = GetFreezerPeriodEndDate(userType);
            DateTime freezePeriodEndDateUkTime = GetUkTimeFromUtc(freezePeriodEndDateUtc);

            viewModel.ServiceAvailableFrom = $"{freezePeriodEndDateUkTime.AddMinutes(1).ToString("HH:mmtt").ToLower()} on {freezePeriodEndDateUkTime.DayOfWeek} {freezePeriodEndDateUkTime:dd MMMM yyyy}";

            return View(viewModel);
        }

        private DateTime GetFreezerPeriodEndDate(LoginUserType type) => type switch
        {
            LoginUserType.AwardingOrganisation => _configuration.ServiceFreezePeriodsSettings.AwardingOrganisation.EndDate.AddSeconds(1),
            LoginUserType.TrainingProvider => _configuration.ServiceFreezePeriodsSettings.TrainingProvider.EndDate.AddSeconds(1),
            _ => throw new InvalidOperationException("Invalid user type")
        };

        private static DateTime GetUkTimeFromUtc(DateTime utc)
        {
            const string GMTStandardTimeId = "GMT Standard Time";

            TimeZoneInfo gmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(GMTStandardTimeId);
            DateTime ukTime = TimeZoneInfo.ConvertTimeFromUtc(utc, gmtTimeZoneInfo);

            return ukTime;
        }
    }
}