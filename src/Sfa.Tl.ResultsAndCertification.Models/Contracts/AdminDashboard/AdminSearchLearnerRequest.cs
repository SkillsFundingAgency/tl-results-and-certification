using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminSearchLearnerRequest
    {
        public string SearchKey { get; set; }

        public long? ProviderUkprn { get; set; }

        public IList<int> AcademicYears { get; set; }

        public IList<int> AwardingOrganisations { get; set; }

        public int? PageNumber { get; set; }
    }
}