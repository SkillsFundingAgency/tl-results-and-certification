using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class SubjectStatusStringHelper
    {
        public static string ToDisplayText(this SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => "Achieved",
            SubjectStatus.NotAchieved => "Not achieved",
            SubjectStatus.AchievedByLrs => "Achieved (LRS)",
            SubjectStatus.NotAchievedByLrs => "Not achieved (LRS)",
            SubjectStatus.NotSpecified => "Not specified",
            _ => "Not yet received",
        };
    }
}
