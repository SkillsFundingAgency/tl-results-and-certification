using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class SearchLearnerFilters
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public IList<FilterLookupData> Status { get; set; }
    }

    public class FilterLookupData
    {
        // TODO: Move this to class
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
