using System.Collections.Generic;
using DateContent = Sfa.Tl.ResultsAndCertification.Web.Content.Helpers.Date;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.DateValidationHelperTests
{
    public abstract class When_ValidateDate_Is_Called
    {        
        private const string DayProperty = "Day";
        private const string MonthProperty = "Month";
        private const string YearProperty = "Year";

        protected const string PropertyName = "Date of birth";
        
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Valid
                    new object[] { "01/01/2000", new Dictionary<string, string>()},
                    new object[] { "29/02/2000", new Dictionary<string, string>()},
                    new object[] { "31/12/2000", new Dictionary<string, string>()},
                    
                    // All Required
                    new object[] { "//", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_All_Required, PropertyName.ToLower()) },
                        { MonthProperty, string.Empty },
                        { YearProperty, string.Empty }
                    } },

                    // Day empty
                    new object[] { "/01/2020", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Day_Required, PropertyName) },
                    } },
                    
                    // Month empty
                    new object[] { "01//2020", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Month_Required, PropertyName) },
                    } },

                    // Year empty
                    new object[] { "01/01/", new Dictionary<string, string>() {
                        { YearProperty, string.Format(DateContent.Validation_Message_Year_Required, PropertyName) },
                    } },

                    // Day and Month empty
                    new object[] { "//2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Day_Month_Required, PropertyName) },
                        { MonthProperty, string.Empty }
                    } },

                    // Day and Year empty
                    new object[] { "/01/", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Day_Year_Required, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    // Month and Year empty
                    new object[] { "01//", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Month_Year_Required, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    // Month and Year Invalid
                    new object[] { "01/13/123", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Year, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    new object[] { "01/aa/xyz", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Month, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    new object[] { "01/44/12345", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Month, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    // Day and Year invalid
                    new object[] { "aa/01/123", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    new object[] { "aa/01/xyz", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    new object[] { "33/01/xyz", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { YearProperty, string.Empty }
                    } },

                    new object[] { "33/01/123", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { YearProperty, string.Empty }
                    } },
                    

                    // Day and Month invalid
                    new object[] { "aa/aa/2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { MonthProperty, string.Empty }
                    } },

                    new object[] { "32/aa/2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { MonthProperty, string.Empty }
                    } },

                    new object[] { "32/13/2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { MonthProperty, string.Empty }
                    } },

                    // Day only invalid
                    new object[] { "hello/01/2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Day, PropertyName) },
                    } },

                    new object[] { "32/01/2000", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Day, PropertyName) },
                    } },

                    new object[] { "29/02/2019", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Day, PropertyName) },
                    } },

                    new object[] { "31/11/2019", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Day, PropertyName) },
                    } },

                    // Month only invalid
                    new object[] { "1/mo/2019", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Month, PropertyName) },
                    } },

                    new object[] { "01/13/2019", new Dictionary<string, string>() {
                        { MonthProperty, string.Format(DateContent.Validation_Message_Invalid_Month, PropertyName) },
                    } },

                    // Year only invalid
                    new object[] { "1/1/year", new Dictionary<string, string>() {
                        { YearProperty, string.Format(DateContent.Validation_Message_Invalid_Year, PropertyName) },
                    } },

                    new object[] { "1/1/123", new Dictionary<string, string>() {
                        { YearProperty, string.Format(DateContent.Validation_Message_Invalid_Year, PropertyName) },
                    } },

                    // Invalid date
                    new object[] { "dd/mm/yyyy", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Invalid_Date, PropertyName) },
                        { MonthProperty, string.Empty },
                        { YearProperty, string.Empty }
                    } },

                    // Future date
                    new object[] { "01/01/9999", new Dictionary<string, string>() {
                        { DayProperty, string.Format(DateContent.Validation_Message_Must_Not_Future_Date, PropertyName) },
                        { MonthProperty, string.Empty },
                        { YearProperty, string.Empty }
                    } },
                };
            }
        }
    }
}