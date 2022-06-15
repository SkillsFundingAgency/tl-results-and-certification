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
                    new object[] { "07/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "17/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "24/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "08/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "15/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "22/06/2022", DayOfWeek.Wednesday, false },
                    new object[] { "29/06/2022", DayOfWeek.Wednesday, true },
                };
            }
        }
    }
}