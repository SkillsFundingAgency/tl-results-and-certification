using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using DateContent = Sfa.Tl.ResultsAndCertification.Web.Content.Helpers.Date;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class DateValidationHelper
    {
        private const string DayProperty = "Day";
        private const string MonthProperty = "Month";
        private const string YearProperty = "Year";

        public static IDictionary<string, string> ValidateDate(this string value, string propertyName)
        {
            // Validate input parameters
            var dateTokens = value.Split("/");
            if (dateTokens.Count() != 3)
                throw new Exception($"Invalid usage of DateValidationHelper. Parameters Value: {value}, PropertyName {propertyName}");

            var day = dateTokens[0];
            var month = dateTokens[1];
            var year = dateTokens[2];

            var validationResults = new Dictionary<string, string>();

            // All empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_All_Required, propertyName.ToLower()));
                validationResults.Add(MonthProperty, string.Empty);
                validationResults.Add(YearProperty, string.Empty);
                
                return validationResults;
            }

            // Day and Month empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(month))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Day_Month_Required, propertyName));
                validationResults.Add(MonthProperty, string.Empty);
                return validationResults;
            }

            // Day and Year empty
            if (string.IsNullOrWhiteSpace(day) && string.IsNullOrWhiteSpace(year))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Day_Year_Required, propertyName));
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            // Month and Year empty
            if (string.IsNullOrWhiteSpace(month) && string.IsNullOrWhiteSpace(year))
            {
                validationResults.Add(MonthProperty, string.Format(DateContent.Validation_Message_Month_Year_Required, propertyName));
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            // Day empty
            if (string.IsNullOrWhiteSpace(day))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Day_Required, propertyName));
                return validationResults;
            }

            // Month empty
            if (string.IsNullOrWhiteSpace(month))
            {
                validationResults.Add(MonthProperty, string.Format(DateContent.Validation_Message_Month_Required, propertyName));
                return validationResults;
            }

            // Year empty
            if (string.IsNullOrWhiteSpace(year))
            {
                validationResults.Add(YearProperty, string.Format(DateContent.Validation_Message_Year_Required, propertyName));
                return validationResults;
            }

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
            {
                validationResults.Add(MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Date, propertyName));
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            // Day and Year invalid
            if (!isDayValid && !isYearValid && int.TryParse(month, out int intMonth) && (intMonth >= 1 && intMonth <= 12))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, propertyName));
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            // Day and Month invalid
            if (!isDayValid && !isMonthValid && int.TryParse(year, out int intYear) && (intYear >= 1000 && intYear <= 9999))
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, propertyName));
                validationResults.Add(MonthProperty, string.Empty);
                return validationResults;
            }

            // Day only invalid
            if (isMonthValid && isYearValid && !isDayValid)
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Invalid_Day, propertyName));
                return validationResults;
            }

            // Month only invalid
            if (isDayValid && isYearValid && !isMonthValid)
            {
                validationResults.Add(MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Month, propertyName));
                return validationResults;
            }

            // Year only invalid
            if (isDayValid && isMonthValid && !isYearValid)
            {
                validationResults.Add(YearProperty, string.Format(DateContent.Validation_Message_Invalid_Year, propertyName));
                return validationResults;
            }

            // Invalid date
            if (!string.Concat(day, month, year).IsDateTimeWithFormat())
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, propertyName));
                validationResults.Add(MonthProperty, string.Empty);
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            // Future date
            var date = string.Concat(day, month, year).ParseStringToDateTime();
            if (date > DateTime.UtcNow)
            {
                validationResults.Add(DayProperty, string.Format(DateContent.Validation_Message_Must_Not_Future_Date, propertyName));
                validationResults.Add(MonthProperty, string.Empty);
                validationResults.Add(YearProperty, string.Empty);
                return validationResults;
            }

            return validationResults;
        }
    }
}
