using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
