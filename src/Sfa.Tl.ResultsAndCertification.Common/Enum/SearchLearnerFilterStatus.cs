using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum SearchLearnerFilterStatus
    {
        [Display(Name = "English incomplete")]
        EnglishIncomplete = 1,

        [Display(Name = "Maths incomplete")]
        MathsIncomplete = 2,

        [Display(Name = "Industry placement")]
        IndustryPlacementIncomplete = 3,

        [Display(Name = "All incompleted records")]
        AllIncompletedRecords = 4
    }
}
