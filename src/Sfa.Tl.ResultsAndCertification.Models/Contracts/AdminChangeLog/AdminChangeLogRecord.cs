using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog
{
    public class AdminChangeLogRecord
    {
        public int ChangeLogId { get; set; }

        public int RegistrationPathwayId { get; set; }

        public string PathwayName { get; set; }

        public string CoreExamPeriod { get; set; }

        public string CoreCode { get; set; }

        public string SpecialismName { get; set; }

        public string SpecialismCode { get; set; }

        public string SpecialismExamPeriod { get; set; }

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