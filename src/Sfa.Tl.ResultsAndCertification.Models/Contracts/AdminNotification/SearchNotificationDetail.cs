using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification
{
    public class SearchNotificationDetail
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public NotificationTarget Target { get; set; }

        public DateTime End { get; set; }

        public bool IsActive { get; set; }
    }
}