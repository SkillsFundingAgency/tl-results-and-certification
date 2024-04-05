using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class ChangeLog : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }

        public ChangeType ChangeType { get; set; }

        public string Details { get; set; }

        public string Name { get; set; }

        public DateTime DateOfRequest { get; set; }

        public string ReasonForChange { get; set; }

        public string ZendeskTicketID { get; set; }

        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}
