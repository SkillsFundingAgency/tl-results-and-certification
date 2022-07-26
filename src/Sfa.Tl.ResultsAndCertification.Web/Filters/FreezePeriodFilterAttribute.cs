using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Filters
{
    public class FreezePeriodFilterAttribute : IAsyncActionFilter
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly ILogger _logger;

        public FreezePeriodFilterAttribute(ResultsAndCertificationConfiguration configuration, ILogger<FreezePeriodFilterAttribute> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {               
                if (context.Controller.GetType() != typeof(HelpController) && context.Controller.GetType() != typeof(ErrorController) && IsFreezePeriodActive())
                {
                    var routeValues = new RouteValueDictionary
                    {
                        { "controller", Constants.HelpController },
                        { "action", nameof(HelpController.ServiceUnavailable) }
                    };
                    context.Result = new RedirectToRouteResult(routeValues);
                    await context.Result.ExecuteResultAsync(context);
                }
                else
                {
                    await next();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private bool IsFreezePeriodActive()
        {
            return DateTime.UtcNow >= _configuration.FreezePeriodStartDate && DateTime.UtcNow <= _configuration.FreezePeriodEndDate;
        }
    }
}
