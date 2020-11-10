using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class FileValidationAttribute : ValidationAttribute
    {
        public string AllowedExtensions { get; set; }
        public int MaxFileNameLength { get; set; }
        public int MaxFileSizeInMb { get; set; }
        public int MaxRecordCount { get; set; }
        public Type ErrorResourceType { get; set; }

        private int MaxFileSize => MaxFileSizeInMb * 1024 * 1024;
        private string[] Extensions => AllowedExtensions?.Split(",");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file?.FileName);
                if (!Extensions.Contains(extension.ToLower()))
                {                    
                    return new ValidationResult(GetResourceMessage("Must_Be_Csv_Validation_Message"));
                }

                var fileName = Path.GetFileNameWithoutExtension(file?.FileName);
                if (fileName.Length > MaxFileNameLength)
                {
                    return new ValidationResult(string.Format(GetResourceMessage("File_Name_Length_Validation_Message"), MaxFileNameLength));
                }

                if (file.Length > MaxFileSize)
                {
                    return new ValidationResult(string.Format(GetResourceMessage("File_Size_Too_Large_Validation_Message"), MaxFileSizeInMb));
                }

                var recordCount = 0;
                using var reader = new StreamReader(file.OpenReadStream());
                while (reader.ReadLine() != null)
                {
                    if (recordCount++ > MaxRecordCount)
                        return new ValidationResult(string.Format(GetResourceMessage("File_Max_Record_Count_Validation_Message"), MaxRecordCount.ToString("N0")));
                }
            }
            return ValidationResult.Success;
        }

        private string GetResourceMessage(string errorResourceName)
        {
            return (string)ErrorResourceType.GetProperty(errorResourceName)?.GetValue(null, null);
        }
    }
}
