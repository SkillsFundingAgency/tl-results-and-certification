using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.DateTimeExtensions
{
    public abstract class TestSetup
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Valid
                    new object[] { new DateTime(2022,06,07), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 17), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 24), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 08), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 15), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 22), DayOfWeek.Wednesday, Months.June, false },
                    new object[] { new DateTime(2022, 06, 29), DayOfWeek.Wednesday, Months.June, true },
                };
            }
        }

        public static IEnumerable<object[]> NthWeekdayOfMonthData
        {
            get
            {
                return new[]
                {
                    // Valid
                    new object[] { new DateTime(2022,07,05), DayOfWeek.Wednesday, Months.July, 1, false },
                    new object[] { new DateTime(2022,07,06), DayOfWeek.Wednesday, Months.July, 1, true },

                    new object[] { new DateTime(2022,07,12), DayOfWeek.Wednesday, Months.July, 2, false },
                    new object[] { new DateTime(2022,07,13), DayOfWeek.Wednesday, Months.July, 2, true },

                    new object[] { new DateTime(2022,07,19), DayOfWeek.Wednesday, Months.July, 3, false },
                    new object[] { new DateTime(2022,07,20), DayOfWeek.Wednesday, Months.July, 3, true },

                    new object[] { new DateTime(2022,07,26), DayOfWeek.Wednesday, Months.July, 4, false },
                    new object[] { new DateTime(2022,07,27), DayOfWeek.Wednesday, Months.July, 4, true },

                    new object[] { new DateTime(2022,08,03), DayOfWeek.Thursday, Months.August, 1, false },
                    new object[] { new DateTime(2022,08,04), DayOfWeek.Thursday, Months.August, 1, true },

                    new object[] { new DateTime(2022,08,10), DayOfWeek.Thursday, Months.August, 2, false },
                    new object[] { new DateTime(2022,08,11), DayOfWeek.Thursday, Months.August, 1, false },
                    new object[] { new DateTime(2022,08,11), DayOfWeek.Thursday, Months.August, 2, true }                    
                };
            }
        }


        public static IEnumerable<object[]> NthDateOfMonthData
        {
            get
            {
                return new[]
                {
                    // Valid
                    new object[] { new DateTime(2022,07,05), DayOfWeek.Wednesday, Months.July, 1, new DateTime(2022, 07, 06) }, // "06/07/2022"
                    new object[] { new DateTime(2022,07,05), DayOfWeek.Monday, Months.July, 2, new DateTime(2022, 07, 11) }, // "11/07/2022"

                    new object[] { new DateTime(2022,07,12), DayOfWeek.Tuesday, Months.July, 3, new DateTime(2022, 07, 19) }, // "19/07/2022"
                    new object[] { new DateTime(2022,07,13), DayOfWeek.Thursday, Months.July, 4, new DateTime(2022, 07, 28) }, // "28/07/2022"

                    new object[] { new DateTime(2022,07,19), DayOfWeek.Wednesday, Months.August, 3, new DateTime(2022, 08, 17) }, // "17/08/2022"
                    new object[] { new DateTime(2023,01,20), DayOfWeek.Monday, Months.January, 1, new DateTime(2023, 01, 02) }, // "02/01/2023"
                    new object[] { new DateTime(2023,02,20), DayOfWeek.Tuesday, Months.April, 3, new DateTime(2023, 04, 18) }, // "18/04/2023"

                    new object[] { new DateTime(2023,07,26), DayOfWeek.Wednesday, Months.July, 7, null }
                };
            }
        }
    }
}