using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class SearchLearnerFiltersViewModel
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
        public IList<FilterLookupData> Tlevels { get; set; }
        public IList<FilterLookupData> Status { get; set; }
    }
}
