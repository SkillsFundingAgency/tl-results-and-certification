using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public  class ReviewChangeRequest
    {
        public int RegistrationPathwayId { get; set; }
        public string ContactName { get; set; }
        public string RequestDate { get; set; }
        public string ChangeReason { get; set; }
        public string ZendeskId { get; set; }
        public string CreatedBy { get; set; }
        public string Details { get; set; }
        public ChangeType ChangeType { get; set;}
      
    }
}
