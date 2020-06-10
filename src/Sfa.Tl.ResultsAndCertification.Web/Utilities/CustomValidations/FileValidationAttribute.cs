using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class FileValidationAttribute : ValidationAttribute
    {
        public string AllowedExtensions { get; set; }
        public int MaxFileNameLength { get; set; }
        public int MaxFileSizeInMb { get; set; }
        public int MaxRecordCount { get; set; }

        private int MaxFileSize { get { return MaxFileSizeInMb * 1024 * 1024; } }
        private string[] Extensions { get { return AllowedExtensions?.Split(","); } }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file?.FileName);
                if (!Extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(ErrorResource.Upload.Must_Be_Csv_Validation_Message);
                }

                var fileName = Path.GetFileNameWithoutExtension(file?.FileName);
                if (fileName.Length > MaxFileNameLength)
                {
                    return new ValidationResult("File name must have no more than 256 characters");
                }

                if (file.Length > MaxFileSize)
                {
                    return new ValidationResult($"File size too large – must be no more than {MaxFileSizeInMb}MB");
                }

                var recordCount = 0;
                using var reader = new StreamReader(file.OpenReadStream());
                while (reader.ReadLine() != null)
                {
                    if (recordCount++ > MaxRecordCount)
                        return new ValidationResult("File must contain between one and 10,000 registrations");
                }
            }

            return ValidationResult.Success;
        }
    }
}
