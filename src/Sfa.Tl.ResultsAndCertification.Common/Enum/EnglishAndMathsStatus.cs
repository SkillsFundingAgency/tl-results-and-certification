using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum EnglishAndMathsStatus
    {
        // TODO: Ravi
        [Display(Name = "Not Specified")]
        NotSpecified = 0,
        [Display(Name = "Completed")]
        Completed = 1,
        [Display(Name = "Completed with special consideration")]
        CompletedWithSpecialConsideration = 2,
        [Display(Name = "Still to be completed")]
        NotCompleted = 3
    }
}
