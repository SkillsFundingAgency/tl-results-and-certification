using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum EnglishAndMathsStatus
    {
        [Display(Name = "Not Specified")]
        NotSpecified = 0,
        [Display(Name = "Achieved minimum standard")]
        Achieved = 1,
        [Display(Name = "Achieved minimum standard with SEND adjustments")]
        AchievedWithSend = 2,
        [Display(Name = "Not achieved minimum standard")]
        NotAchieved = 3
    }
}
