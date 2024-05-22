using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Common.Enum
{
    public enum NotificationTemplateName
    {
        [Description("Tlevel Details Queried")]
        TlevelDetailsQueriedUserNotification,
        TlevelDetailsQueriedTechnicalTeamNotification,
        FunctionJobFailedNotification,
        PrintingJobFailedNotification,
        GradeChangeRequestUserNotification,
        GradeChangeRequestTechnicalTeamNotificationCoreComponent,
        GradeChangeRequestTechnicalTeamNotificationSpecialism,
        AppealGradeAfterDeadlineRequestUserNotification,
        AppealGradeAfterDeadlineRequestTechnicalTeamNotification
    }
}