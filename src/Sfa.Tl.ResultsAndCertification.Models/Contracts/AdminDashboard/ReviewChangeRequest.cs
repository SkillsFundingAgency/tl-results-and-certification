using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public abstract class ReviewChangeRequest
    {
        public int RegistrationPathwayId { get; set; }
        public string ContactName { get; set; }
        public string RequestDate { get; set; }
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public string CreatedBy { get; set; }
        public string Details { get; set; }
        public abstract ChangeType ChangeType { get; }
    }
}