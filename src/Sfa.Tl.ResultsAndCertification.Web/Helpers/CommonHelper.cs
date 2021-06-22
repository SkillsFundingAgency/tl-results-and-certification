using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class CommonHelper
    {
        public static string GetResourceMessage(string errorResourceName, Type errorResourceType)
        {
            return (string)errorResourceType?.GetProperty(errorResourceName)?.GetValue(null, null);
        }

        public static bool IsSoaAlreadyRequested(int reRequestAllowedInDays, DateTime? requestedDate)
        {
            return requestedDate.HasValue && DateTime.Today < requestedDate.Value.Date.AddDays(reRequestAllowedInDays);
        }

        public static bool IsAppealsAllowed(DateTime? appealsEndDate)
        {
            return appealsEndDate.HasValue && DateTime.Today <= appealsEndDate.Value;
        }
    }
}