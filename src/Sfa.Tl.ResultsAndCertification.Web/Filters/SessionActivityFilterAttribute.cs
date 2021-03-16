using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
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
                if (context.Controller.GetType() != typeof(HomeController) && context.Controller.GetType() != typeof(TimeoutController) && !string.IsNullOrWhiteSpace(context.HttpContext.User.GetUserId()))
                {
                    var cacheKey = CacheKeyHelper.GetCacheKey(context.HttpContext.User.GetUserId(), CacheConstants.UserSessionActivityCacheKey);
                    await _cacheService.SetAsync(cacheKey, DateTime.UtcNow);
                }

                if (context.HttpContext.User.Identity.IsAuthenticated && !context.HttpContext.User.HasAccessToService() && IsAccessDenied(context))
                {
                    var routeValues = new RouteValueDictionary
                    {
                        { "controller", Constants.ErrorController },
                        { "action", nameof(ErrorController.ServiceAccessDenied) }
                    };
                    context.Result = new RedirectToRouteResult(routeValues);
                    await context.Result.ExecuteResultAsync(context);
                }
                else
                {
                    await next();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }

        private static bool IsAccessDenied(ActionContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor && controllerActionDescriptor.ControllerName == Constants.ErrorController && controllerActionDescriptor.ActionName == nameof(ErrorController.AccessDenied);
        }        
    }
}
