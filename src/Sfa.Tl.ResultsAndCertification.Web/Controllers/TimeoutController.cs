using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Controllers
{
    public class TimeoutController : Controller
    {
        public readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ICacheService _cacheService;

        public TimeoutController(ResultsAndCertificationConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("active-duration", Name = RouteConstants.ActiveDuration)]
        public async Task<JsonResult> GetActiveDurationAsync()
        {
            var cacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);
            var registeredSessionTime = await _cacheService.GetAsync<DateTime>(cacheKey);
            var remainingActiveDuration = registeredSessionTime != null ? (registeredSessionTime.AddMinutes(_configuration.DfeSignInSettings.Timeout) - DateTime.UtcNow) : new TimeSpan(0,0,0);

            return Json(new { minutes = remainingActiveDuration.Minutes, seconds = remainingActiveDuration.Seconds });
        }

        [HttpGet]
        [Route("renew-activity", Name = RouteConstants.RenewSessionActivity)]
        public async Task<JsonResult> RenewSessionActivityAsync()
        {
            var cacheKey = CacheKeyHelper.GetCacheKey(User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);
            await _cacheService.SetAsync(cacheKey, DateTime.UtcNow);
            var registeredSessionTime = await _cacheService.GetAsync<DateTime>(cacheKey);
            var remainingActiveDuration = registeredSessionTime != null ? (registeredSessionTime.AddMinutes(_configuration.DfeSignInSettings.Timeout) - DateTime.UtcNow) : new TimeSpan(0, 0, 0);

            return Json(new { minutes = remainingActiveDuration.Minutes, seconds = remainingActiveDuration.Seconds });
        }
    }
}