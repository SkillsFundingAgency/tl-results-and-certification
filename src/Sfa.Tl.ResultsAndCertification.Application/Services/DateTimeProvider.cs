using System;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public string GetUtcNowString(string format)
        {
            return GetUtcNow().ToString(format).Replace("Z", string.Empty);
        }
    }
}
