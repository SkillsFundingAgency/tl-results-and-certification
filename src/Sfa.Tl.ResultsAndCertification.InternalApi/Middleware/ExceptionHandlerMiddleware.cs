using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.InternalApi.Infrastructure;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == 404)
                {
                    await HandleFor404Async(httpContext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LogEvent.UnhandledException, ex, $"{ex.Message}, User: {httpContext?.User?.GetUserEmail()}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var jsonResponse = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode));

                await context.Response.WriteAsync(jsonResponse);
            }
        }

        private static async Task HandleFor404Async(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var jsonResponse = JsonConvert.SerializeObject(new ApiResponse(context.Response.StatusCode));
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
