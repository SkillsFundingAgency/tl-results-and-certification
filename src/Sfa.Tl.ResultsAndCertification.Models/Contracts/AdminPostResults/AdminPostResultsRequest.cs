using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults
{
    public abstract class AdminPostResultsRequest
    {
        public int RegistrationPathwayId { get; set; }

        public string ContactName { get; set; }

        public DateTime DateOfRequest { get; set; }

        public string ChangeReason { get; set; }

        public string ZendeskTicketId { get; set; }

        public string CreatedBy { get; set; }
    }
}
