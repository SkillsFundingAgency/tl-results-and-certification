using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification
{
    public class AddNotificationRequest
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NotificationTarget Target { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string CreatedBy { get; set; }

    }
}
