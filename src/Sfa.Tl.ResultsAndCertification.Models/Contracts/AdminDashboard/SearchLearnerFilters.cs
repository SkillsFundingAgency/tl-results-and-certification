using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class SearchLearnerFilters
    {
        public IList<FilterLookupData> AwardingOrganisations { get; set; }

        public IList<FilterLookupData> AcademicYears { get; set; }
    }
}