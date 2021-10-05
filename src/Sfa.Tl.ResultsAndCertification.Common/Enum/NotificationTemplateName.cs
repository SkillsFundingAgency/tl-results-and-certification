using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum NotificationTemplateName
    {
        [Description("Tlevel Details Queried")]
        TlevelDetailsQueriedUserNotification,
        TlevelDetailsQueriedTechnicalTeamNotification,
        EnglishAndMathsLrsDataQueried,
        FunctionJobFailedNotification,
        PrintingJobFailedNotification,
        GradeChangeRequestUserNotification,
        GradeChangeRequestTechnicalTeamNotification,
        AppealGradeAfterDeadlineRequestUserNotification,
        AppealGradeAfterDeadlineRequestTechnicalTeamNotification
    }
}