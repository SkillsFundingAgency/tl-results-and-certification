using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum PrintCertificateType
    {
        NotSpecified = 0,

        [Display(Name = "Statement of achievement")]
        StatementOfAchievement = 1,

        Certificate = 2
    }
}