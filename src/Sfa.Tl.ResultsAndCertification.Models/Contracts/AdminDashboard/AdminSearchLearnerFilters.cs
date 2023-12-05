using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminSearchLearnerFilters
    {
        public IList<FilterLookupData> AwardingOrganisations { get; set; }

        public IList<FilterLookupData> AcademicYears { get; set; }
    }
}