using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class SearchLearnerFilters
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public IList<FilterLookupData> Status { get; set; }
    }
}
