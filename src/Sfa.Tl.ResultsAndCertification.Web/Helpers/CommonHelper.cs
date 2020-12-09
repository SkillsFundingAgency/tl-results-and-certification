using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class CommonHelper
    {
        public static string GetResourceMessage(string errorResourceName, Type errorResourceType)
        {
            return (string)errorResourceType?.GetProperty(errorResourceName)?.GetValue(null, null);
        }
    }
}
