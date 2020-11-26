using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class ModelStateHelper
    {
        public static Tuple<string, string> GetUploadErrorMessage(RequestErrorType errorType, Type errorResourceType)
        {
            Tuple<string, string> errorMessage = null;
            if (errorResourceType != null)
            {
                errorMessage = errorType switch
                {
                    RequestErrorType.FileType => new Tuple<string, string>("File", CommonHelper.GetResourceMessage("Must_Be_Csv_Validation_Message", errorResourceType)),
                    RequestErrorType.NotSpecified => null,
                    _ => null
                };
            }
            return errorMessage;
        }

        public static void AddModelStateError(ModelStateDictionary modelState, string key, string errorMessage)
        {
            if(modelState != null && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(errorMessage))
            {
                modelState.AddModelError(key, errorMessage);
            }
        }
    }
}
