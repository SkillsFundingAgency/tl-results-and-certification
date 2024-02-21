using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Utils;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class InputDateValidationAttribute : ValidationAttribute
    {
        public string Property { get; set; }

        public Type ResourceType { get; set; }

        public string BlankTextResourceName { get; set; }

        public string InvalidDateResourceName { get; set; }

        public string FutureDateResourceName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null || value is not InputDate)
            {
                return ValidationResult.Success;
            }

            var inputDate = (InputDate)value;

            if (ContainsBlankText(inputDate))
            {
                return CreateValidationResult(BlankTextResourceName);
            }

            if (!IsValidDate(inputDate, out DateTime dateTime))
            {
                return CreateValidationResult(InvalidDateResourceName);
            }

            var systemProvider = (ISystemProvider)validationContext.GetService(typeof(ISystemProvider));

            bool isFutureDate = dateTime > systemProvider.Today;
            if (isFutureDate)
            {
                return CreateValidationResult(FutureDateResourceName);
            }

            return ValidationResult.Success;
        }

        private static bool ContainsBlankText(InputDate inputDate)
            => IsBlankText(inputDate.Day) || IsBlankText(inputDate.Month) || IsBlankText(inputDate.Year);

        private static bool IsBlankText(string value)
            => string.IsNullOrWhiteSpace(value);

        private static bool IsValidDate(InputDate inputDate, out DateTime parsed)
        {
            parsed = DateTime.MinValue;

            bool allNumeric = IsNumericText(inputDate.Day, out int day) & IsNumericText(inputDate.Month, out int month) & IsNumericText(inputDate.Year, out int year);
            if (!allNumeric)
            {
                return false;
            }

            try
            {
                parsed = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        private static bool IsNumericText(string value, out int parsed)
            => int.TryParse(value, out parsed);

        private ValidationResult CreateValidationResult(string resourceName)
        {
            string message = CommonHelper.GetResourceMessage(resourceName, ResourceType);
            return new ValidationResult(message, new[] { Property });
        }
    }
}