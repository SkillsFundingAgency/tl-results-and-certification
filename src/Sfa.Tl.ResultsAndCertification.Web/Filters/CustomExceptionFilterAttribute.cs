using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Error;
using System.Diagnostics;
using System.Net;

namespace Sfa.Tl.ResultsAndCertification.Web.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public CustomExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CustomExceptionFilterAttribute>();
        }

        public override void OnException(ExceptionContext context)
        {
            var user = context.HttpContext?.User?.GetUserEmail();
            _logger.LogError(LogEvent.UnhandledException, context.Exception, $"{context.Exception.Message}, User: {user}");

            var result = new ViewResult { ViewName = "~/Views/Error/ProblemWithService.cshtml" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(modelMetadata, context.ModelState)
            {
                {
                    "Exception", context.Exception
                }
            };

            result.ViewData.Model = new ProblemWithServiceViewModel { RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier };
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
