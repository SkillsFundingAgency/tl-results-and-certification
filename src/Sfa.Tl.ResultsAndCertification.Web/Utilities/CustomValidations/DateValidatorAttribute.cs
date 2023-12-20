using System;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class DateValidatorAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public Type ErrorResourceType { get; set; }
        public string ErrorResourceName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            var propVal = type.GetProperty(Property).GetValue(instance);

            // Validate input parameters
            var dateTokens = value.ToString().Split("/");
            if (dateTokens.Count() != 3)
                throw new Exception($"Invalid usage of DateValidationAttribute. Parameters Value: {value}, PropertyName {Property}");

            var day = dateTokens[0];
            var month = dateTokens[1];
            var year = dateTokens[2];

            // All empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Day and Month empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Day and Year empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(year))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Month and Year empty
            if (string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Day empty
            if (string.IsNullOrWhiteSpace(day))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Month empty
            if (string.IsNullOrWhiteSpace(month))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            // Year empty
            if (string.IsNullOrWhiteSpace(year))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Blank_Text"), propVal));

            day = day.PadLeft(2, '0');
            month = month.PadLeft(2, '0');

            // Invalid Day/Month/Year
            var isYearValid = (year.TrimStart('0').Length == 4) && string.Concat("01", "01", year).IsDateTimeWithFormat();
            var isMonthValid = string.Concat("01", month, 2020).IsDateTimeWithFormat();

            bool isDayValid;
            if (isMonthValid && isYearValid)
                isDayValid = string.Concat(day, month, year).IsDateTimeWithFormat();
            else
                isDayValid = string.Concat(day, "01", 2020).IsDateTimeWithFormat();

            // Month and Year invalid
            if (!isMonthValid && !isYearValid && int.TryParse(day, out int intDay) && (intDay >= 1 && intDay <= 31))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Day and Year invalid
            if (!isDayValid && !isYearValid && int.TryParse(month, out int intMonth) && (intMonth >= 1 && intMonth <= 12))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Day and Month invalid
            if (!isDayValid && !isMonthValid && int.TryParse(year, out int intYear) && (intYear >= 1000 && intYear <= 9999))
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Day only invalid
            if (isMonthValid && isYearValid && !isDayValid)
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Month only invalid
            if (isDayValid && isYearValid && !isMonthValid)
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Year only invalid
            if (isDayValid && isMonthValid && !isYearValid)
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Invalid date
            if (!string.Concat(day, month, year).IsDateTimeWithFormat())
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Invalid_Text"), propVal));

            // Future date
            var date = string.Concat(day, month, year).ParseStringToDateTime();
            if (date > DateTime.UtcNow)
                return new ValidationResult(string.Format(GetResourceMessage("Validation_Date_When_Change_Requested_Future_Date_Text"), propVal));

            return ValidationResult.Success;
        }

        private string GetResourceMessage(string errorResourceName)
        {
            return CommonHelper.GetResourceMessage(errorResourceName, ErrorResourceType);
        }
    }
}
