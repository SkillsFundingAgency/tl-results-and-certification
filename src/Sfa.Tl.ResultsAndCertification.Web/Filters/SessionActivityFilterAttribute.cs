using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Filters
{
    public class SessionActivityFilterAttribute : IAsyncActionFilter
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger _logger;

        public SessionActivityFilterAttribute(ICacheService cacheService, ILogger<SessionActivityFilterAttribute> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (context.Controller.GetType() != typeof(Controllers.HomeController) && context.Controller.GetType() != typeof(Controllers.TimeoutController) && !string.IsNullOrWhiteSpace(context.HttpContext.User.GetUserId()))
                {
                    var cacheKey = CacheKeyHelper.GetCacheKey(context.HttpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);
                    await _cacheService.SetAsync(cacheKey, DateTime.UtcNow);
                }

                await next();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
