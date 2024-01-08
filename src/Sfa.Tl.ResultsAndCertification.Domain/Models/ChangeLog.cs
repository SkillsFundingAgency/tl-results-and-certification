using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class ChangeLog : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public int ChangeType { get; set; }

        public string Details { get; set; }

        public string Name { get; set;}

        public DateTime DateOfRequest { get; set; }

        public string ReasonForChange { get; set;}

        public string ZendeskTicketID { get; set; }

    }
}
