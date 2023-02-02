using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public enum IndustryPlacementStatus
    {
        [Display(Name = "Placement completed")]
        Completed,

        [Display(Name = "Placement completed with special consideration")]
        CompletedWithSpecialConsideration,

        [Display(Name = "Placement still to be completed")]
        NotCompleted,

        [Display(Name = "Placement will not be completed")]
        WillNotComplete
    }
}