﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Help;
using System;

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
            DateTime freezePeriodEndDateUtc = _configuration.FreezePeriodEndDate.AddSeconds(1);
            DateTime freezePeriodEndDateUkTime = GetUkTimeFromUtc(freezePeriodEndDateUtc);

            var viewModel = new ServiceUnavailableViewModel
            {
                ServiceAvailableFrom = $"{freezePeriodEndDateUkTime.AddMinutes(1).ToString("HH:mmtt").ToLower()} on {freezePeriodEndDateUkTime.DayOfWeek} {freezePeriodEndDateUkTime:dd MMMM yyyy}"
            };

            return View(viewModel);
        }

        private static DateTime GetUkTimeFromUtc(DateTime utc)
        {
            const string GMTStandardTimeId = "GMT Standard Time";

            TimeZoneInfo gmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(GMTStandardTimeId);
            DateTime ukTime = TimeZoneInfo.ConvertTimeFromUtc(utc, gmtTimeZoneInfo);

            return ukTime;
        }
    }
}