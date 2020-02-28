using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Enum
{
    public enum EnumTlRoute
    {
        [Display(Name = "Construction")]
        Construction = 1,
        Digital = 2,
        [Display(Name = "Education and Childcare")]
        EducationAndChildcare = 3
    }
}
