﻿using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    // Note: Text from the Display attribute is being used in the OverallResult Json data.
    public enum IndustryPlacementStatus
    {
        NotSpecified = 0,

        [Display(Name = "Completed")]
        Completed = 1,

        [Display(Name = "Completed with special consideration")]
        CompletedWithSpecialConsideration = 2,

        [Display(Name = "Not completed")]
        NotCompleted = 3,

        [Display(Name = "Will not complete")]
        WillNotComplete = 4
    }
}