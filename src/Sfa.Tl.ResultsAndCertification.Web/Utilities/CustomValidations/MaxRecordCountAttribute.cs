using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class MaxRecordCountAttribute : ValidationAttribute
    {
        private readonly int _maxRecordCount;
        public MaxRecordCountAttribute(int maxRecordCount)
        {
            _maxRecordCount = maxRecordCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var recordCount = 0;

                using var reader = new StreamReader(file.OpenReadStream());
                while (reader.ReadLine() != null)
                {
                    if (recordCount++ > _maxRecordCount)
                        return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
