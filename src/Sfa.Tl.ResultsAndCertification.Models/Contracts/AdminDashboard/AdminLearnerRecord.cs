using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminLearnerRecord
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string Provider { get; set; }
        public int Ukprn { get; set; }
        public string TLevel { get; set; }
        public int TLevelStartYear { get; set; }
        public int AcademicYear { get; set; }
        public string DisplayAcademicYear { get; set; }
        public List<int> AcademicStartYearsToBe { get; set; }
    }
}
