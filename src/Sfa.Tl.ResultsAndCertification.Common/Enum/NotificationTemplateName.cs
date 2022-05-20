using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum NotificationTemplateName
    {
        [Description("Tlevel Details Queried")]
        TlevelDetailsQueriedUserNotification,
        TlevelDetailsQueriedTechnicalTeamNotification,
        EnglishAndMathsLrsDataQueried, // TODO: To be removed
        FunctionJobFailedNotification,
        PrintingJobFailedNotification,
        GradeChangeRequestUserNotification,
        GradeChangeRequestTechnicalTeamNotification,
        AppealGradeAfterDeadlineRequestUserNotification,
        AppealGradeAfterDeadlineRequestTechnicalTeamNotification
    }
}