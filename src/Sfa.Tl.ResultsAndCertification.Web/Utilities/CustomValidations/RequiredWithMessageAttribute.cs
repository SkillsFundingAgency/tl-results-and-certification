using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class RequiredWithMessageAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public Type ErrorResourceType { get; set; }
        public string ErrorResourceName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                object instance = validationContext.ObjectInstance;
                Type type = instance.GetType();
                var propVal = type.GetProperty(Property).GetValue(instance);
                
                return new ValidationResult(string.Format(GetResourceMessage(ErrorResourceName), propVal));
            }
           
            return ValidationResult.Success;
        }

        private string GetResourceMessage(string errorResourceName)
        {
            return CommonHelper.GetResourceMessage(errorResourceName, ErrorResourceType);
        }
    }
}
