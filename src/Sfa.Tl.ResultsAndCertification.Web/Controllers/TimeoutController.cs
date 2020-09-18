using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TimeoutController : Controller
    {
        public readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ICacheService _cacheService;

        private string CacheKey
        {
            get { return CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.UserSessionActivityCacheKey); }
        }

        public TimeoutController(ResultsAndCertificationConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("active-duration", Name = RouteConstants.ActiveDuration)]
        public async Task<JsonResult> GetActiveDurationAsync()
        {
            var registeredSessionTime = await _cacheService.GetAsync<DateTime>(CacheKey);
            var remainingActiveDuration = (registeredSessionTime != null && registeredSessionTime != DateTime.MinValue) ? (registeredSessionTime.AddMinutes(_configuration.DfeSignInSettings.Timeout) - DateTime.UtcNow) : new TimeSpan(0, 0, 0);
            return Json(new SessionActivityData { Minutes = remainingActiveDuration.Minutes < 0 ? 0 : remainingActiveDuration.Minutes, Seconds = remainingActiveDuration.Seconds < 0 ? 0 : remainingActiveDuration.Seconds });
        }

        [HttpGet]
        [Route("renew-activity", Name = RouteConstants.RenewSessionActivity)]
        public async Task<JsonResult> RenewSessionActivityAsync()
        {
            await _cacheService.SetAsync(CacheKey, DateTime.UtcNow);
            return Json(new SessionActivityData { Minutes = _configuration.DfeSignInSettings.Timeout, Seconds = 0 });
        }

        [HttpGet]
        [Route("activity-timeout", Name = RouteConstants.ActivityTimeout)]
        public async Task ActivityTimeout()
       {
            var userId = User.GetUserId();
            TempData.Set(Constants.UserSessionActivityId, userId);
            await _cacheService.RemoveAsync<DateTime>(CacheKeyHelper.GetCacheKey(userId, CacheConstants.UserSessionActivityCacheKey));
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("timeout", Name = RouteConstants.Timeout)]
        public IActionResult TimeoutConfirmation()
        {
            return View();
        }
    }
}