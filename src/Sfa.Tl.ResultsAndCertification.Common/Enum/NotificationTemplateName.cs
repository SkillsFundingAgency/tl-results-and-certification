using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum NotificationTemplateName
    {
        [Description("Tlevel Details Queried")]
        TlevelDetailsQueried,
        TlevelDetailsQueriedUserNotification,
        EnglishAndMathsLrsDataQueried,
        FunctionJobFailedNotification,
        PrintingJobFailedNotification,
        GradeChangeRequestUserNotification,
        GradeChangeRequestTechnicalTeamNotification,
        AppealGradeAfterDeadlineRequestUserNotification,
        AppealGradeAfterDeadlineRequestTechnicalTeamNotification
    }
}