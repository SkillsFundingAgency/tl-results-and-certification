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
    }
}