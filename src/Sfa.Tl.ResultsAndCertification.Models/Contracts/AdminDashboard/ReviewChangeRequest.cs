using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public abstract class ReviewChangeRequest
    {
        public int RegistrationPathwayId { get; set; }
        public string ContactName { get; set; }
        public DateTime RequestDate { get; set; }
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public string CreatedBy { get; set; }

        public abstract ChangeType ChangeType { get; }
    }
}