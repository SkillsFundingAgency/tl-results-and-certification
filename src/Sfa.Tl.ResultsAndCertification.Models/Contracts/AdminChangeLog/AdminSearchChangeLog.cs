using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminSearchChangeLog
    {
        public int ChangeLogId { get; set; }

        public DateTime DateAndTimeOfChange { get; set; }

        public string ZendeskTicketID { get; set; }

        public string LearnerFirstname { get; set; }

        public string LearnerLastname { get; set; }

        public long Uln { get; set; }

        public string ProviderName { get; set; }

        public long ProviderUkprn { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}