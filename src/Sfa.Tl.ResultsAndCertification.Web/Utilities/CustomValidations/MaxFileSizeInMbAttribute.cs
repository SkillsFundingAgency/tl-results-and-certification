using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class MaxFileSizeInMbAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeInMbAttribute(int maxFileSizeInMb)
        {
            _maxFileSize = maxFileSizeInMb * 1024 * 1024;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
