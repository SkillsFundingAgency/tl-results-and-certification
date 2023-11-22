using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminSearchLearnerFilters
    {
        public IList<AdminFilter> AwardingOrganisations { get; set; }

        public IList<AdminFilter> AcademicYears { get; set; }
    }
}