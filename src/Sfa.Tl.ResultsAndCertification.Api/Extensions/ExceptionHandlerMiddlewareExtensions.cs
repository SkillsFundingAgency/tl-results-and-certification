using Microsoft.AspNetCore.Builder;
using Sfa.Tl.ResultsAndCertification.InternalApi.Middleware;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Extensions
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void ConfigureExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
