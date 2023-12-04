using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminLearnerRecord
    {
        public string FirstName { get;set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string Provider { get; set; }
        public string TLevel { get; set; }
        public string StartYear { get; set; }
    }
}
