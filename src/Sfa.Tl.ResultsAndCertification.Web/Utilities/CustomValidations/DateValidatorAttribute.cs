using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class DateValidatorAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public Type ErrorResourceType { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null || value is not string)
            {
                return ValidationResult.Success;
            }

            string[] tokens = ((string)value).Split("/");

            if (tokens.Length != 3)
            {
                return ValidationResult.Success;
            }

            string year = tokens[0];
            string month = tokens[1];
            string day = tokens[2];

            if (ContainsBlankText(year, month, day))
            {
                return CreateValidationResult("Validation_Date_When_Change_Requested_Blank_Text");
            }

            if (!IsValidDate(year, month, day, out DateTime dateTime))
            {
                return CreateValidationResult("Validation_Date_When_Change_Requested_Invalid_Text");
            }

            var systemProvider = (ISystemProvider)validationContext.GetService(typeof(ISystemProvider));

            bool isFutureDate = dateTime > systemProvider.Today;
            if (isFutureDate)
            {
                return CreateValidationResult("Validation_Date_When_Change_Requested_Future_Date_Text");
            }

            return ValidationResult.Success;
        }

        private static bool ContainsBlankText(string year, string month, string day)
            => IsBlankText(year) || IsBlankText(month) || IsBlankText(day);

        private static bool IsBlankText(string value)
            => string.IsNullOrWhiteSpace(value);

        private static bool IsValidDate(string year, string month, string day, out DateTime parsed)
        {
            parsed = DateTime.MinValue;

            bool allNumeric = IsNumericText(year, out int parsedYear) & IsNumericText(month, out int parsedMonth) & IsNumericText(day, out int parsedDay);
            if (!allNumeric)
            {
                return false;
            }

            bool validNumbers = IsBetween(parsedYear, 1000, 9999) && IsBetween(parsedMonth, 1, 12) && IsBetween(parsedDay, 1, 31);
            if (!validNumbers)
            {
                return false;
            }

            try
            {
                parsed = new DateTime(parsedYear, parsedMonth, parsedDay);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        private static bool IsNumericText(string value, out int parsed)
            => int.TryParse(value, out parsed);

        private static bool IsBetween(int number, int from, int to)
            => number >= from && number <= to;

        private ValidationResult CreateValidationResult(string resourceName)
        {
            string message = CommonHelper.GetResourceMessage(resourceName, ErrorResourceType);
            return new ValidationResult(message, new[] { Property });
        }
    }
}