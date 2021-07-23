using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System;
using PrsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsStatus;

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

        public static string GetPrsStatusDisplayText(PrsStatus? prsStatus, DateTime? appealsEndDate = null)
        {
            if (prsStatus == PrsStatus.Final || !IsAppealsAllowed(appealsEndDate))
                return FormatPrsStatusDisplayHtml(Constants.RedTagClassName, PrsStatusContent.Final_Display_Text);
            
            if (prsStatus == PrsStatus.BeingAppealed)
                return FormatPrsStatusDisplayHtml(Constants.PurpleTagClassName, PrsStatusContent.Being_Appealed_Display_Text);

            return string.Empty;
        }

        private static string FormatPrsStatusDisplayHtml(string tagClassName, string statusText) => string.Format(PrsStatusContent.PrsStatus_Display_Html, tagClassName, statusText);
    }
}