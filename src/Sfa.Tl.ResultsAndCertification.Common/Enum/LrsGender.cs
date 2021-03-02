using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum LrsGender
    {
        [Description("Not Known")]
        NotKnown = 0,
        Male = 1,
        Female = 2,
        [Description("Not Specified")]
        NotSpecified = 9
    }
}