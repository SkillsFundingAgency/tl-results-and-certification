using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class ValidationResultExtensions
    {
        // Replacement for the deprecated FluentValidation.AspNetCore AddToModelState extension.
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState, string prefix = null)
        {
            if (result == null || modelState == null || result.IsValid)
            {
                return;
            }

            foreach (var error in result.Errors)
            {
                var key = string.IsNullOrEmpty(prefix)
                    ? error.PropertyName
                    : string.IsNullOrEmpty(error.PropertyName) ? prefix : $"{prefix}.{error.PropertyName}";

                modelState.AddModelError(key, error.ErrorMessage);
            }
        }
    }
}
