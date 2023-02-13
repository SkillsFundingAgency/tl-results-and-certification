using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public enum IndustryPlacementStatus
    {
        [Display(Name = "Placement completed")]
        Completed = 1,

        [Display(Name = "Placement completed with special consideration")]
        CompletedWithSpecialConsideration = 2,

        [Display(Name = "Placement still to be completed")]
        NotCompleted = 3,

        [Display(Name = "Placement will not be completed")]
        WillNotComplete = 4
    }
}