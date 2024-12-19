using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class Notification : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NotificationTarget Target { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}