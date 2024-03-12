using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogRecord
    {
        public int ChangeLogId { get; set; }

        public int RegistrationPathwayId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long Uln { get; set; }

        public string CreatedBy { get; set; }

        public ChangeType ChangeType { get; set; }

        public string ChangeDetails { get; set; }

        public string ChangeRequestedBy { get; set; }

        public DateTime ChangeDateOfRequest { get; set; }

        public string ReasonForChange { get; set; }

        public string ZendeskTicketID { get; set; }

        public DateTime DateAndTimeOfChange { get; set; }
    }
}