using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum SearchLearnerFilterStatus
    {
        [Display(Name = "English level")]
        EnglishIncomplete = 1,

        [Display(Name = "Maths level")]
        MathsIncomplete = 2
    }
}
