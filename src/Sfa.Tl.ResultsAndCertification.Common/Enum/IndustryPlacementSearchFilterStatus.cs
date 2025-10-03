using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum IndustryPlacementSearchFilterStatus
    {
        [Display(Name = "Completed")]
        IndustryPlacementCompleted = 1,

        [Display(Name = "Completed with Special Consideration")]
        IndustryPlacementCompletedWithConsideration = 2,

        [Display(Name = "Not completed")]
        IndustryPlacementNotCompleted = 3,

        [Display(Name = "Will not complete")]
        IndustryPlacementWillNotComplete = 4,

        [Display(Name = "Not yet reported")]
        IndustryPlacementNotReported = 5

    }
}
