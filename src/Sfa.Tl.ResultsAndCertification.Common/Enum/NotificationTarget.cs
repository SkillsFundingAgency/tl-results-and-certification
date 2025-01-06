using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum NotificationTarget
    {
        [Display(Name = "Not specified")]
        NotSpecified = 0,

        [Display(Name = "Awarding organisation")]
        AwardingOrganisation = 1,

        [Display(Name = "Provider")]
        Provider = 2,

        [Display(Name = "Both")]
        Both = 3
    }
}